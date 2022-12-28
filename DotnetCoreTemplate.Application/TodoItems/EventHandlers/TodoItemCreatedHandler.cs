using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.TodoItems.Events;
using DotnetCoreTemplate.Application.TodoItems.Workers;

namespace DotnetCoreTemplate.Application.TodoItems.EventHandlers;

public class TodoItemCreatedHandler : IEventHandler<TodoItemCreatedEvent>
{
	private readonly ILogger<TodoItemCreatedHandler> _logger;
	private readonly ITimeProvider _timeProvider;
	private readonly IQueueProvider _queueProvider;

	public TodoItemCreatedHandler(
		ILogger<TodoItemCreatedHandler> logger,
		ITimeProvider timeProvider,
		IQueueProvider queueProvider)
	{
		_logger = logger;
		_timeProvider = timeProvider;
		_queueProvider = queueProvider;
	}

	public async Task Handle(TodoItemCreatedEvent domainEvent, CancellationToken cancellation)
	{
		_logger.LogMessage($"A new TODO Item was created with ID: '{domainEvent.Id}'");

		var firingTime = _timeProvider.Now.AddSeconds(5);

		await _queueProvider.Queue(new CreateTodoWork(domainEvent.Id), firingTime);
	}
}