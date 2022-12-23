using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public record DomainEventHandlerType(Type Type);

public class EventDispatcher : IEventDispatcher
{
	private readonly Container _container;

	public EventDispatcher(Container container)
	{
		_container = container;
	}

	public async Task Dispatch<TEvent>(TEvent domainEvent, CancellationToken cancellation)
		where TEvent : DomainEvent
	{
		var handler = _container.GetInstance<IEventHandler<TEvent>>();

		await handler.Handle(domainEvent, cancellation);
	}
}