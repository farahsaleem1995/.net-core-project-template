using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Workers;

public record CreateTodoWork(int Id);

public class CreateTodoWorker : IWorker<CreateTodoWork>
{
	private readonly ILogger<CreateTodoWorker> _logger;

	public CreateTodoWorker(ILogger<CreateTodoWorker> logger)
	{
		_logger = logger;
	}

	public Task Execute(CreateTodoWork work, CancellationToken cancellation)
	{
		_logger.LogMessage($"A new TODO Item was created with ID: '{work.Id}'");

		return Task.CompletedTask;
	}
}