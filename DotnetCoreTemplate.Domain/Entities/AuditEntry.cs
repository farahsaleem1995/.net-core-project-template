namespace DotnetCoreTemplate.Domain.Entities;

public class AuditEntry
{
	public AuditEntry(string userId, string operation, DateTime executedDate)
	{
		UserId = userId;
		Operation = operation;
		ExecutedDate = executedDate;
	}

	public string Id { get; } = null!;

	public string UserId { get; private set; }

	public string Operation { get; private set; }

	public object? Data { get; set; }

	public DateTime ExecutedDate { get; private set; }

	public User User { get; private set; } = null!;
}