using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Workers;

public record TodoItemCreatedWork(int Id) : IWork;

public class TodoItemCreatedWorkHandler : IWorkHandler<TodoItemCreatedWork>
{
	private readonly ILogger<TodoItemCreatedWorkHandler> _logger;

	public TodoItemCreatedWorkHandler(ILogger<TodoItemCreatedWorkHandler> logger)
	{
		_logger = logger;
	}

	public Task Handle(TodoItemCreatedWork work, CancellationToken cancellation)
	{
		_logger.LogMessage($"A new TODO Item was created with ID: '{work.Id}'");

		return Task.CompletedTask;
	}
}