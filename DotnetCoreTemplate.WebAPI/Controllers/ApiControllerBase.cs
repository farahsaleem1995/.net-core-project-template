using DotnetCoreTemplate.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreTemplate.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiControllerBase : Controller
{
	public ApiControllerBase(ICommandDirector director)
	{
		Director = director;
	}

	public ICommandDirector Director { get; }
}