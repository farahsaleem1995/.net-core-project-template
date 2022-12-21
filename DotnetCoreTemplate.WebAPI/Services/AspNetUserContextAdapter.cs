using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using System.Security.Claims;

namespace DotnetCoreTemplate.WebAPI.Services;

public class AspNetUserContextAdapter : IUserContext
{
	private readonly IHttpContextAccessor _accessor;

	public AspNetUserContextAdapter(IHttpContextAccessor accessor)
	{
		_accessor = accessor;
	}

	public bool Authorized => _accessor.HttpContext?.User != null;

	public string UserId
	{
		get
		{
			var idClaim = _accessor.HttpContext?.User.Claims
				.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

			return idClaim?.Value ?? string.Empty;
		}
	}

	public bool IsInRole(UserRole role)
	{
		var roles = _accessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.Role) ?? new List<Claim>();

		return roles.Any(r => r.Value == role.ToString());
	}
}