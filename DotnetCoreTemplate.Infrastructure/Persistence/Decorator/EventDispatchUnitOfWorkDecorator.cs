using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Decorator;

public class EventDispatchUnitOfWorkDecorator : IUnitOfWork
{
	private readonly IUnitOfWork _decoratee;
	private readonly IDirector _director;
	private readonly ApplicationDbContext _dbContext;

	public EventDispatchUnitOfWorkDecorator(
		IUnitOfWork decoratee,
		IDirector director,
		ApplicationDbContext dbContext)
	{
		_decoratee = decoratee;
		_director = director;
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
					await _director.DispatchEvent(domainEvent, cancellation);
				}
			}
		}
	}
}