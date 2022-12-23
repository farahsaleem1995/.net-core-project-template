using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Decorator;

public class AuditUnitOfWorkDecorator : IUnitOfWork
{
	private readonly IUnitOfWork _decoratee;
	private readonly ITimeProvider _timeProvider;
	private readonly ApplicationDbContext _dbContext;

	public AuditUnitOfWorkDecorator(
		IUnitOfWork decoratee,
		ITimeProvider timeProvider,
		ApplicationDbContext dbContext)
	{
		_decoratee = decoratee;
		_timeProvider = timeProvider;
		_dbContext = dbContext;
	}

	public async Task SaveAsync(CancellationToken cancellation = default)
	{
		foreach (var entry in _dbContext.ChangeTracker.Entries<Auditable>())
		{
			switch (entry.State)
			{
				case EntityState.Added:
					entry.Entity.CreatedDate = _timeProvider.Now;
					break;

				case EntityState.Modified:
					entry.Entity.LastUpdatedDate = _timeProvider.Now;
					break;
			}
		}

		await _decoratee.SaveAsync(cancellation);
	}
}