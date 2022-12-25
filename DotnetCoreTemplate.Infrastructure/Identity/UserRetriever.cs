using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Entities;
using DotnetCoreTemplate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Infrastructure.Identity;

public class UserRetriever : IUserRetriever
{
	private readonly ApplicationDbContext _dbContext;

	public UserRetriever(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<User?> GetUserById(string userId, CancellationToken cancellation = default)
	{
		var aggregate = UserAggregate(u => u.Id == userId);

		return await aggregate.FirstOrDefaultAsync(cancellation);
	}

	public async Task<User?> GetUserByEmai(string email, CancellationToken cancellation = default)
	{
		var aggregate = UserAggregate(u => u.Email == email);

		return await aggregate.FirstOrDefaultAsync(cancellation);
	}

	private IQueryable<User> UserAggregate(Expression<Func<User, bool>> filter)
	{
		return _dbContext.UsersView
			.Include(u => u.Roles)
			.ThenInclude(ur => ur.Role)
			.Where(filter);
	}
}