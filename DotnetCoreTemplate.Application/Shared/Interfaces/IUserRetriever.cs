using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IUserRetriever
{
	Task<User?> GetUserById(string userId, CancellationToken cancellation = default);

	Task<User?> GetUserByEmai(string email, CancellationToken cancellation = default);
}