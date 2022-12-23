using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFUnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IEventDispatcher _eventDispatcher;

	public EFUnitOfWork(
		ApplicationDbContext dbContext,
		IEventDispatcher eventDispatcher)
	{
		_dbContext = dbContext;
		_eventDispatcher = eventDispatcher;
	}

	public async Task SaveAsync(CancellationToken cancellation = default)
	{
		await _dbContext.SaveChangesAsync(cancellation);

		await Dispatch(cancellation);
	}

	private async Task Dispatch(CancellationToken cancellation)
	{
		foreach (var entry in _dbContext.ChangeTracker.Entries<IHasDomainEvents>())
		{
			foreach (var domainEvent in entry.Entity.DomainEvents)
			{
				if (!domainEvent.IsDispatched)
				{
					domainEvent.IsDispatched = true;
					await _eventDispatcher.Dispatch(domainEvent, cancellation);
				}
			}
		}
	}
}