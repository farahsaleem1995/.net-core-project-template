using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.AuditTrail.GetAuditTrail;

public record GetAuditTrailQuery(byte Limit = 10) : IQuery<IEnumerable<AuditTrailDto>>;