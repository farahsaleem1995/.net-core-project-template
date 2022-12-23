using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;

namespace DotnetCoreTemplate.Application.Shared.Services;

public class CompositeEventHandler<TEvent> : IEventHandler<TEvent>
	where TEvent : DomainEvent
{
	private readonly IEnumerable<IEventHandler<TEvent>> _handlers;

	public CompositeEventHandler(IEnumerable<IEventHandler<TEvent>> handlers)
	{
		_handlers = handlers;
	}

	public async Task Handle(TEvent domainEvent, CancellationToken cancellation)
	{
		foreach (var handler in _handlers)
		{
			await handler.Handle(domainEvent, cancellation);
		}
	}
}