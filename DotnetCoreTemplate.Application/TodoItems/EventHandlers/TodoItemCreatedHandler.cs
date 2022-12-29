using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.TodoItems.Events;
using DotnetCoreTemplate.Application.TodoItems.Workers;

namespace DotnetCoreTemplate.Application.TodoItems.EventHandlers;

public class TodoItemCreatedHandler : IEventHandler<TodoItemCreatedEvent>
{
	private readonly ILogger<TodoItemCreatedHandler> _logger;
	private readonly ITimeProvider _timeProvider;
	private readonly IWorkScheduler _scheduler;
	private readonly IWorkQueue _queue;

	public TodoItemCreatedHandler(
		ILogger<TodoItemCreatedHandler> logger,
		ITimeProvider timeProvider,
		IWorkScheduler scheduler,
		IWorkQueue queue)
	{
		_logger = logger;
		_timeProvider = timeProvider;
		_scheduler = scheduler;
		_queue = queue;
	}

	public async Task Handle(TodoItemCreatedEvent domainEvent, CancellationToken cancellation)
	{
		_logger.LogMessage($"A new TODO Item was created with ID: '{domainEvent.Id}'");

		var firingTime = _timeProvider.Now.AddSeconds(5);

		await _scheduler.Schedule(new CreateTodoWork(domainEvent.Id), firingTime, cancellation);

		await _queue.Enqueue(new CreateTodoWork(domainEvent.Id), cancellation);
	}
}