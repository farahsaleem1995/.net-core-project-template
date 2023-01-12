using DotnetCoreTemplate.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreTemplate.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiControllerBase : Controller
{
	public ApiControllerBase(ISender sender)
	{
		Sender = sender;
	}

	public ISender Sender { get; }
}