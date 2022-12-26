using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Users.CreateUser;
using DotnetCoreTemplate.Application.Users.GetUserById;
using DotnetCoreTemplate.Application.Users.GetUsers;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreTemplate.WebAPI.Controllers;

public class UsersController : ApiControllerBase
{
	public UsersController(IDirector director) : base(director)
	{
	}

	[HttpGet]
	public async Task<IActionResult> Get(
		[FromQuery] GetUsersQuery query, CancellationToken cancellation)
	{
		var users = await Director.Send(query, cancellation);

		return Ok(users);
	}

	[HttpPost]
	public async Task<IActionResult> Get(
		[FromBody] CreateUserCommand command, CancellationToken cancellation)
	{
		await Director.Send(command, cancellation);

		return NoContent();
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellation)
	{
		var user = await Director.Send(new GetUserByIdQuery(id), cancellation);

		return Ok(user);
	}
}