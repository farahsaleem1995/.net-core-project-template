using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IIdentityProvider
{
	Task<Result> RegisterUsertAsync(string email, string password, SecurityRole role, CancellationToken cancellation = default);

	Task<Result> SignInAsync(string userId, string password, CancellationToken cancellation = default);
}