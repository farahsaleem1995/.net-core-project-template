using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.TodoItems.Commands.CreateTodoItem;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDoing;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDone;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsToDo;
using DotnetCoreTemplate.Application.TodoItems.Commands.UpdateTodoItem;
using DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;
using DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItems;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreTemplate.WebAPI.Controllers;

public class TodosController : ApiControllerBase
{
	public TodosController(ISender sender) : base(sender)
	{
	}

	[HttpGet]
	public async Task<IActionResult> Create([FromQuery] GetTodoItemsQuery query,
		CancellationToken cancellation)
	{
		var todoItems = await Sender.Send(query, cancellation);

		return Ok(todoItems);
	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Create([FromRoute] int id, CancellationToken cancellation)
	{
		var todoItems = await Sender.Send(new GetTodoItemByIdQuery(id), cancellation);

		return Ok(todoItems);
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand command,
		CancellationToken cancellation)
	{
		var id = await Sender.Send(command, cancellation);

		return Ok(id);
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromBody] UpdateTodoItemCommand command,
		CancellationToken cancellation)
	{
		await Sender.Send(command, cancellation);

		return NoContent();
	}

	[HttpPut("ToDo")]
	public async Task<IActionResult> SetAsToDo([FromBody] SetTodoItemAsToDoCommand command,
		CancellationToken cancellation)
	{
		await Sender.Send(command, cancellation);

		return NoContent();
	}

	[HttpPut("Doing")]
	public async Task<IActionResult> SetAsDoing([FromBody] SetTodoItemAsDoingCommand command,
		CancellationToken cancellation)
	{
		await Sender.Send(command, cancellation);

		return NoContent();
	}

	[HttpPut("Done")]
	public async Task<IActionResult> SetAsDone([FromBody] SetTodoItemAsDoneCommand command,
		CancellationToken cancellation)
	{
		await Sender.Send(command, cancellation);

		return NoContent();
	}
}