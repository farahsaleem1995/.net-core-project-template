using DotnetCoreTemplate.Domain.Shared;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IEventHandler<TEvent> where TEvent : DomainEvent
{
	Task Handle(TEvent domainEvent, CancellationToken cancellation);
}