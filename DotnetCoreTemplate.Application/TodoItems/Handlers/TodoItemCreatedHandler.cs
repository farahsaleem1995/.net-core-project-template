using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.TodoItems.Events;

namespace DotnetCoreTemplate.Application.TodoItems.Handlers;

public class TodoItemCreatedHandler : IEventHandler<TodoItemCreatedEvent>
{
	private readonly IDomainLogger<TodoItemCreatedHandler> _logger;

	public TodoItemCreatedHandler(IDomainLogger<TodoItemCreatedHandler> logger)
	{
		_logger = logger;
	}

	public Task Handle(TodoItemCreatedEvent domainEvent)
	{
		_logger.LogMessage($"A new TODO Item was created with ID: '{domainEvent.Id}'");

		return Task.CompletedTask;
	}
}