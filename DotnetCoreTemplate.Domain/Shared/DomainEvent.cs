namespace DotnetCoreTemplate.Domain.Shared;

public abstract class DomainEvent
{
	public bool IsDispatched { get; set; }
}