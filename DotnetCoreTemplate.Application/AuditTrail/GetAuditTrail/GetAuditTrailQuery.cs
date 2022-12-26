using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.AuditTrail.GetAuditTrail;

[Security(SecurityRole.Administrator)]
public record GetAuditTrailQuery(byte Limit = 10) : IQuery<IEnumerable<AuditTrailDto>>;