namespace DotnetCoreTemplate.Domain.Shared;

public interface IHasDomainEvents
{
	public List<DomainEvent> DomainEvents { get; }
}