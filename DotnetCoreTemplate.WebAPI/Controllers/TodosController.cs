using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.TodoItems.Commands.CreateTodoItem;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDoing;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDone;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsToDo;
using DotnetCoreTemplate.Application.TodoItems.Commands.UpdateTodoItem;
using DotnetCoreTemplate.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreTemplate.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController : Controller
{
	private readonly ICommandDirector _commandDirector;

	public TodosController(ICommandDirector commandDirector)
	{
		_commandDirector = commandDirector;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand command,
		CancellationToken cancellation)
	{
		var id = await _commandDirector.Execute(command, cancellation);

		return Ok(id);
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromBody] UpdateTodoItemCommand command,
		CancellationToken cancellation)
	{
		await _commandDirector.Execute(command, cancellation);

		return NoContent();
	}

	[HttpPut("ToDo")]
	public async Task<IActionResult> SetAsToDo([FromBody] SetTodoItemAsToDoCommand command,
		CancellationToken cancellation)
	{
		await _commandDirector.Execute(command, cancellation);

		return NoContent();
	}

	[HttpPut("Doing")]
	public async Task<IActionResult> SetAsDoing([FromBody] SetTodoItemAsDoingCommand command,
		CancellationToken cancellation)
	{
		await _commandDirector.Execute(command, cancellation);

		return NoContent();
	}

	[HttpPut("Done")]
	public async Task<IActionResult> SetAsDone([FromBody] SetTodoItemAsDoneCommand command,
		CancellationToken cancellation)
	{
		await _commandDirector.Execute(command, cancellation);

		return NoContent();
	}
}