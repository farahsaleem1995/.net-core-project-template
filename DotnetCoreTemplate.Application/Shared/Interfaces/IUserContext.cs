using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IUserContext
{
	bool Authorized { get; }

	string UserId { get; }

	string AccessToken { get; }

	bool IsInRole(UserRole role);

	Task<User> CurrentUser();
}