using DotnetCoreTemplate.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreTemplate.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiControllerBase : Controller
{
	public ApiControllerBase(IDirector director)
	{
		Director = director;
	}

	public IDirector Director { get; }
}