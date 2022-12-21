using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.TodoItems.Commands.CreateTodoItem;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDoing;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDone;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsToDo;
using DotnetCoreTemplate.Application.TodoItems.Commands.UpdateTodoItem;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreTemplate.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController : Controller
{
	private readonly ICommandService<CreateTodoItemCommand, int> _createTodoItemService;
	private readonly ICommandService<UpdateTodoItemCommand> _updateTodoItemService;
	private readonly ICommandService<SetTodoItemAsToDoCommand> _setTodoItemAsToDoService;
	private readonly ICommandService<SetTodoItemAsDoingCommand> _setTodoItemAsDoingService;
	private readonly ICommandService<SetTodoItemAsDoneCommand> _setTodoItemAsDoneService;

	public TodosController(
		ICommandService<CreateTodoItemCommand, int> createTodoItemService,
		ICommandService<UpdateTodoItemCommand> updateTodoItemService,
		ICommandService<SetTodoItemAsToDoCommand> setTodoItemAsToDoService,
		ICommandService<SetTodoItemAsDoingCommand> setTodoItemAsDoingService,
		ICommandService<SetTodoItemAsDoneCommand> setTodoItemAsDoneService)
	{
		_createTodoItemService = createTodoItemService;
		_updateTodoItemService = updateTodoItemService;
		_setTodoItemAsToDoService = setTodoItemAsToDoService;
		_setTodoItemAsDoingService = setTodoItemAsDoingService;
		_setTodoItemAsDoneService = setTodoItemAsDoneService;
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand command,
		CancellationToken cancellation)
	{
		var id = await _createTodoItemService.Execute(command, cancellation);

		return Ok(id);
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromBody] UpdateTodoItemCommand command,
		CancellationToken cancellation)
	{
		await _updateTodoItemService.Execute(command, cancellation);

		return NoContent();
	}

	[HttpPut("ToDo")]
	public async Task<IActionResult> SetAsToDo([FromBody] SetTodoItemAsToDoCommand command,
		CancellationToken cancellation)
	{
		await _setTodoItemAsToDoService.Execute(command, cancellation);

		return NoContent();
	}

	[HttpPut("Doing")]
	public async Task<IActionResult> SetAsDoing([FromBody] SetTodoItemAsDoingCommand command,
		CancellationToken cancellation)
	{
		await _setTodoItemAsDoingService.Execute(command, cancellation);

		return NoContent();
	}

	[HttpPut("Done")]
	public async Task<IActionResult> SetAsDone([FromBody] SetTodoItemAsDoneCommand command,
		CancellationToken cancellation)
	{
		await _setTodoItemAsDoneService.Execute(command, cancellation);

		return NoContent();
	}
}