using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFUnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ITimeProvider _timeProvider;
	private readonly IDispatcher _dispatcher;

	public EFUnitOfWork(
		ApplicationDbContext dbContext,
		ITimeProvider timeProvider,
		IDispatcher dispatcher)
	{
		_dbContext = dbContext;
		_timeProvider = timeProvider;
		_dispatcher = dispatcher;
	}

	public async Task SaveAsync(CancellationToken cancellation = default)
	{
		Audit();

		await _dbContext.SaveChangesAsync(cancellation);
	}

	private void Audit()
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
	}

	public async Task DispatchAsync(CancellationToken cancellation = default)
	{
		foreach (var entry in _dbContext.ChangeTracker.Entries<IHasDomainEvents>())
		{
			foreach (var domainEvent in entry.Entity.DomainEvents)
			{
				await _dispatcher.Dispatch(domainEvent, cancellation);
			}
		}
	}
}