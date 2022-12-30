using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DotnetCoreTemplate.Infrastructure.Identity;

public class TokenProvider : ITokenProvider
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly ApplicationDbContext _dbContext;
	private readonly ITimeProvider _timeProvider;
	private readonly TokenSettings _settings;

	public TokenProvider(
		UserManager<ApplicationUser> userManager,
		ApplicationDbContext dbContext,
		ITimeProvider timeProvider,
		TokenSettings settings)
	{
		_userManager = userManager;
		_dbContext = dbContext;
		_timeProvider = timeProvider;
		_settings = settings;
	}

	public async Task<Result<Token>> GenerateTokenAsync(string userId, CancellationToken cancellation = default)
	{
		if (string.IsNullOrEmpty(userId))
		{
			throw new ArgumentNullException(nameof(userId));
		}

		return await GenerateToken(userId, cancellation);
	}

	private async Task<Result<Token>> GenerateToken(string userId, CancellationToken cancellation)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
		{
			return Result<Token>.Fail(new[] { $"User '{userId}' was not found." });
		}

		var jti = GenerateJti();
		var claims = await GetUserClaims(user, jti);
		var accessToken = GenerateAccessToken(claims);
		var refershToken = await CreateRefreshToken(user, jti, cancellation);

		await _dbContext.SaveChangesAsync(cancellation);

		return Result<Token>.Succeed(new Token(accessToken, refershToken));
	}

	private static string GenerateJti()
	{
		return Guid.NewGuid().ToString();
	}

	private async Task<List<Claim>> GetUserClaims(ApplicationUser user, string jti)
	{
		var roles = await _userManager.GetRolesAsync(user);

		var claims = new List<Claim>()
		{
			new Claim(JwtRegisteredClaimNames.Jti, jti),
			new Claim(JwtRegisteredClaimNames.Sub, user.Id),
		};

		foreach (var role in roles)
		{
			claims.Add(new Claim(ClaimTypes.Role, role));
		}

		return claims;
	}

	private string GenerateAccessToken(List<Claim> claims)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var expiredDate = _timeProvider.Now.Add(_settings.AccessLifetime);

		var descriptor = new SecurityTokenDescriptor
		{
			SigningCredentials = credentials,
			Subject = new ClaimsIdentity(claims),
			Expires = expiredDate,
		};

		var token = tokenHandler.CreateToken(descriptor);

		return tokenHandler.WriteToken(token);
	}

	private async Task<string> CreateRefreshToken(ApplicationUser user, string jti, CancellationToken cancellation)
	{
		var token = Guid.NewGuid().ToString();
		var expiredDate = _timeProvider.Now.Add(_settings.RefreshLifetime);
		var refreshToken = new RefreshToken(jti, token, user.Id, expiredDate);

		await _dbContext.RefreshTokens.AddAsync(refreshToken, cancellation);

		return token;
	}

	public async Task<Result<Token>> RefreshTokenAsync(Token token, CancellationToken cancellation = default)
	{
		if (token == null)
		{
			throw new ArgumentNullException(nameof(token));
		}

		return await RefreshToken(token, cancellation);
	}

	private async Task<Result<Token>> RefreshToken(Token token, CancellationToken cancellation)
	{
		var principal = GetClaimsPrincipal(token.AccessToken, false);
		if (principal == null)
		{
			return Result<Token>.Fail(new[] { "Invalid access token" });
		}

		var jti = principal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

		return await RefreshTokenByJti(token, jti, cancellation);
	}

	private ClaimsPrincipal? GetClaimsPrincipal(string token, bool validateLifetime)
	{
		try
		{
			return TryGetClaimsPrincipal(token, validateLifetime);
		}
		catch (Exception)
		{
			return null;
		}
	}

	private ClaimsPrincipal? TryGetClaimsPrincipal(string token, bool validateLifetime)
	{
		var tokenHandler = new JwtSecurityTokenHandler();

		var validationParameters = GetValidationParameters(validateLifetime);

		var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

		return principal;
	}

	private TokenValidationParameters GetValidationParameters(bool validateLifetime)
	{
		return new TokenValidationParameters()
		{
			ValidateLifetime = validateLifetime,
			ValidateIssuerSigningKey = true,
			ValidateIssuer = _settings.ValidateIssuer,
			ValidateAudience = _settings.ValidateAudience,
			ValidIssuer = _settings.Issuer,
			ValidAudience = _settings.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
		};
	}

	private async Task<Result<Token>> RefreshTokenByJti(Token token, string jti, CancellationToken cancellation)
	{
		var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(
			t => t.Token == token.RefreshToken && t.Jti == jti, cancellation);

		if (refreshToken == null || !IsRefreshTokenValid(refreshToken))
		{
			return Result<Token>.Fail(new[] { "Invalid access token" });
		}

		refreshToken.Invalidated = true;

		return await GenerateTokenAsync(refreshToken.UserId, cancellation);
	}

	private bool IsRefreshTokenValid(RefreshToken refreshToken)
	{
		var used = refreshToken.Used;
		var invalidated = refreshToken.Invalidated;
		var expired = IsTokenExpired(refreshToken);

		return !used && !invalidated && !expired;
	}

	private bool IsTokenExpired(RefreshToken refreshToken)
	{
		return refreshToken.ExpiredDate < _timeProvider.Now;
	}

	public async Task<Result> InvalidateTokenAsync(string token, CancellationToken cancellation = default)
	{
		if (string.IsNullOrEmpty(token))
		{
			throw new ArgumentNullException(nameof(token));
		}

		return await InvalidateToken(token, cancellation);
	}

	private async Task<Result> InvalidateToken(string token, CancellationToken cancellation)
	{
		var principal = GetClaimsPrincipal(token, true);
		if (principal == null)
		{
			return Result.Succeed();
		}

		var jti = principal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

		return await InvalidateRefreshToken(jti, cancellation);
	}

	private async Task<Result> InvalidateRefreshToken(string jti, CancellationToken cancellation)
	{
		await _dbContext.RefreshTokens
			.Where(t => t.Jti == jti)
			.ExecuteUpdateAsync(
				c => c.SetProperty(t => t.Invalidated, t => true), cancellation);

		return Result.Succeed();
	}

	public class TokenSettings
	{
		public string Key { get; init; } = string.Empty;

		public TimeSpan AccessLifetime { get; init; }

		public TimeSpan RefreshLifetime { get; init; }

		public string Issuer { get; init; } = string.Empty;

		public bool ValidateIssuer { get; init; }

		public string Audience { get; init; } = string.Empty;

		public bool ValidateAudience { get; init; }
	}
}