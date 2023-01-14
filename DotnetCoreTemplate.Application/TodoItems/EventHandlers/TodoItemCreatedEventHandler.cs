using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.TodoItems.Events;
using DotnetCoreTemplate.Application.TodoItems.Workers;

namespace DotnetCoreTemplate.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler : IEventHandler<TodoItemCreatedEvent>
{
	private readonly ILogger<TodoItemCreatedEventHandler> _logger;
	private readonly ITimeProvider _timeProvider;
	private readonly IScheduler _scheduler;
	private readonly IQueue _queue;

	public TodoItemCreatedEventHandler(
		ILogger<TodoItemCreatedEventHandler> logger,
		ITimeProvider timeProvider,
		IScheduler scheduler,
		IQueue queue)
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

		await _scheduler.Schedule(new TodoItemCreatedWork(domainEvent.Id), firingTime, cancellation);

		await _queue.Enqueue(new TodoItemCreatedWork(domainEvent.Id), cancellation);
	}
}