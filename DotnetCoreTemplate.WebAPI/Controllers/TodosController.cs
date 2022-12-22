﻿using DotnetCoreTemplate.Application.TodoItems.Commands.CreateTodoItem;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDoing;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDone;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsToDo;
using DotnetCoreTemplate.Application.TodoItems.Commands.UpdateTodoItem;
using DotnetCoreTemplate.WebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCoreTemplate.WebAPI.Controllers;

public class TodosController : ApiControllerBase
{
	public TodosController(ICommandDirector director) : base(director)
	{
	}

	[HttpPost]
	public async Task<IActionResult> Create([FromBody] CreateTodoItemCommand command,
		CancellationToken cancellation)
	{
		var id = await Director.Execute(command, cancellation);

		return Ok(id);
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromBody] UpdateTodoItemCommand command,
		CancellationToken cancellation)
	{
		await Director.Execute(command, cancellation);

		return NoContent();
	}

	[HttpPut("ToDo")]
	public async Task<IActionResult> SetAsToDo([FromBody] SetTodoItemAsToDoCommand command,
		CancellationToken cancellation)
	{
		await Director.Execute(command, cancellation);

		return NoContent();
	}

	[HttpPut("Doing")]
	public async Task<IActionResult> SetAsDoing([FromBody] SetTodoItemAsDoingCommand command,
		CancellationToken cancellation)
	{
		await Director.Execute(command, cancellation);

		return NoContent();
	}

	[HttpPut("Done")]
	public async Task<IActionResult> SetAsDone([FromBody] SetTodoItemAsDoneCommand command,
		CancellationToken cancellation)
	{
		await Director.Execute(command, cancellation);

		return NoContent();
	}
}