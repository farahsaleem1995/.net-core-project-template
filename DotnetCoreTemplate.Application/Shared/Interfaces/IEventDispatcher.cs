using DotnetCoreTemplate.Domain.Shared;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IEventDispatcher
{
	Task Dispatch<TEvent>(TEvent domainEvent, CancellationToken cancellation) where TEvent : DomainEvent;
}