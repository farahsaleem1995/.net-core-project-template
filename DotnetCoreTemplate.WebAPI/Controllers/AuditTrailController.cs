using DotnetCoreTemplate.Application.AuditTrail.GetAuditTrail;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreTemplate.WebAPI.Controllers;

[Route("audit-trail")]
public class AuditTrailController : ApiControllerBase
{
	public AuditTrailController(IDirector director) : base(director)
	{
	}

	[HttpGet]
	public async Task<IActionResult> Get(
		[FromQuery] GetAuditTrailQuery query, CancellationToken cancellation)
	{
		var tail = await Director.Send(query, cancellation);

		return Ok(tail);
	}
}