using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.TodoItems.Events;

namespace DotnetCoreTemplate.Application.TodoItems.EventHandlers;

public class TodoItemCreatedHandler : IEventHandler<TodoItemCreatedEvent>
{
	private readonly ILogger<TodoItemCreatedHandler> _logger;

	public TodoItemCreatedHandler(ILogger<TodoItemCreatedHandler> logger)
	{
		_logger = logger;
	}

	public Task Handle(TodoItemCreatedEvent domainEvent, CancellationToken cancellation)
	{
		_logger.LogMessage($"A new TODO Item was created with ID: '{domainEvent.Id}'");

		return Task.CompletedTask;
	}
}