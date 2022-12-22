namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IEventDispatcher
{
	Task Dispatch<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
}