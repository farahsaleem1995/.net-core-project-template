namespace DotnetCoreTemplate.Domain.Shared;

public abstract class Auditable
{
	public DateTime CreatedDate { get; set; }

	public DateTime? LastUpdatedDate { get; set; }
}