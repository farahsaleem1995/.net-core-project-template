using DotnetCoreTemplate.Application.Auth.Commands.RefreshToken;
using DotnetCoreTemplate.Application.Auth.Commands.RegisterUser;
using DotnetCoreTemplate.Application.Auth.Commands.SignIn;
using DotnetCoreTemplate.Application.Auth.Commands.SignOut;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreTemplate.WebAPI.Controllers;

public class AuthController : ApiControllerBase
{
	public AuthController(ISender sender) : base(sender)
	{
	}

	[HttpPost]
	public async Task<IActionResult> RegisterAccount(
		[FromBody] RegisterUserCommand command, CancellationToken cancellation)
	{
		await Sender.Send(command, cancellation);

		return NoContent();
	}

	[HttpPost("sign-in")]
	public async Task<IActionResult> SignIn(
		[FromBody] SignInCommand command, CancellationToken cancellation)
	{
		var token = await Sender.Send(command, cancellation);

		return Ok(token);
	}

	[HttpPost("sign-out")]
	public async Task<IActionResult> SignOut(CancellationToken cancellation)
	{
		await Sender.Send(new SignOutCommand(), cancellation);

		return NoContent();
	}

	[HttpPost("refresh-token")]
	public async Task<IActionResult> RefreshToken(
		[FromBody] RefreshTokenCommand command, CancellationToken cancellation)
	{
		var token = await Sender.Send(command, cancellation);

		return Ok(token);
	}
}