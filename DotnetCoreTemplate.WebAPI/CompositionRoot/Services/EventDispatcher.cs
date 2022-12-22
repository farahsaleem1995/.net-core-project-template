using DotnetCoreTemplate.Application.Shared.Interfaces;
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

	public async Task Dispatch<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
	{
		var handler = _container.GetInstance<IEventHandler<TEvent>>();

		await handler.Handle(domainEvent);
	}
}