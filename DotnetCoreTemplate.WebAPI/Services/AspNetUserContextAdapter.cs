using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Entities;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;

namespace DotnetCoreTemplate.WebAPI.Services;

public class AspNetUserContextAdapter : IUserContext
{
	private readonly IHttpContextAccessor _accessor;
	private readonly IUserRetriever _userRetriever;

	public AspNetUserContextAdapter(
		IHttpContextAccessor accessor,
		IUserRetriever userRetriever)
	{
		_accessor = accessor;
		_userRetriever = userRetriever;
	}

	public bool Authorized => !string.IsNullOrEmpty(UserId);

	public string UserId
	{
		get
		{
			var idClaim = _accessor.HttpContext?.User.Claims
				.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

			return idClaim?.Value ?? string.Empty;
		}
	}

	public string AccessToken
	{
		get
		{
			var authorization = _accessor.HttpContext?.Request.Headers[HeaderNames.Authorization]
				.SingleOrDefault();

			return authorization?
				.Replace("bearer", "", StringComparison.InvariantCultureIgnoreCase)
				.Trim() ?? string.Empty;
		}
	}

	public bool IsInRole(SecurityRole role)
	{
		var roles = _accessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.Role) ?? new List<Claim>();

		return roles.Any(r => r.Value == role.ToString());
	}

	public async Task<User> CurrentUser()
	{
		var user = await _userRetriever.GetUserById(UserId);

		return user ?? throw new InvalidOperationException("The current context ha no user");
	}
}