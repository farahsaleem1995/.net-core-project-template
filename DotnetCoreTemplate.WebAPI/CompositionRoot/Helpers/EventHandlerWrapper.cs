using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

public abstract class EventHandlerWrapperBase
{
	public abstract Task Handle(Container container, DomainEvent domainEvent, CancellationToken cancellation);
}

public class EventHandlerWrapperImpl<TEvent> : EventHandlerWrapperBase
	where TEvent : DomainEvent
{
	public override async Task Handle(Container container, DomainEvent domainEvent, CancellationToken cancellation)
	{
		var handler = container.GetInstance<IEventHandler<TEvent>>();

		await handler.Handle((TEvent)domainEvent, cancellation);
	}
}