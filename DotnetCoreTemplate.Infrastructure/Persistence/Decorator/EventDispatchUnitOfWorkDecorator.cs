using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Decorator;

public class EventDispatchUnitOfWorkDecorator : IUnitOfWork
{
	private readonly IUnitOfWork _decoratee;
	private readonly IEventDispatcher _dispatcher;
	private readonly ApplicationDbContext _dbContext;

	public EventDispatchUnitOfWorkDecorator(
		IUnitOfWork decoratee,
		IEventDispatcher dispatcher,
		ApplicationDbContext dbContext)
	{
		_decoratee = decoratee;
		_dispatcher = dispatcher;
		_dbContext = dbContext;
	}

	public async Task SaveAsync(CancellationToken cancellation = default)
	{
		await _decoratee.SaveAsync(cancellation);

		foreach (var entry in _dbContext.ChangeTracker.Entries<IHasDomainEvents>())
		{
			foreach (var domainEvent in entry.Entity.DomainEvents)
			{
				if (!domainEvent.IsDispatched)
				{
					domainEvent.IsDispatched = true;
					await _dispatcher.Dispatch(domainEvent, cancellation);
				}
			}
		}
	}
}