using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IAuditTrailRetriever
{
	Task<IReadOnlyList<AuditEntry>> Get(int limit);
}