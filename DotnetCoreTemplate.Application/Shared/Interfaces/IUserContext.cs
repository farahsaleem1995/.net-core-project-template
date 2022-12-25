using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IUserContext
{
	bool Authorized { get; }

	string UserId { get; }

	string AccessToken { get; }

	bool IsInRole(SecurityRole role);

	Task<User> CurrentUser();
}