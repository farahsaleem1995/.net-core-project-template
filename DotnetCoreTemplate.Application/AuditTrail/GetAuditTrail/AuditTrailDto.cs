namespace DotnetCoreTemplate.Application.AuditTrail.GetAuditTrail;

public record AuditTrailDto(
	string Id, string UserId, string Operation, object? Data, DateTime ExecutedDate);