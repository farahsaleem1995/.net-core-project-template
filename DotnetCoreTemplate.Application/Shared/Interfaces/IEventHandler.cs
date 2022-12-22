namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IEventHandler<TEvent> where TEvent : IDomainEvent
{
	Task Handle(TEvent domainEvent);
}