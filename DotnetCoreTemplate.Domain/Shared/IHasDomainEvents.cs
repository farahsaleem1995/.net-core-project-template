namespace DotnetCoreTemplate.Domain.Shared;

public interface IHasDomainEvents
{
	public ICollection<DomainEvent> DomainEvents { get; }
}