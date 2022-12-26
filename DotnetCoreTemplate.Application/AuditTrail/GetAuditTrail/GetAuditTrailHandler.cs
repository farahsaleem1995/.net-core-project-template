using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.AuditTrail.GetAuditTrail;

public class GetAuditTrailHandler : IRequestHandler<GetAuditTrailQuery, IEnumerable<AuditTrailDto>>
{
	private readonly IAuditTrailRetriever _retriever;

	public GetAuditTrailHandler(IAuditTrailRetriever retriever)
	{
		_retriever = retriever;
	}

	public async Task<IEnumerable<AuditTrailDto>> Handle(GetAuditTrailQuery request, CancellationToken cancellation)
	{
		var trail = await _retriever.Get(request.Limit);

		return trail.Select(e => new AuditTrailDto(e.Id, e.UserId, e.Operation, e.Data, e.ExecutedDate));
	}
}