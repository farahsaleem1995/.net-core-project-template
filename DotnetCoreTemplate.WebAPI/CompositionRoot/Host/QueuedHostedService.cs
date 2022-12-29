using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Host;

public class QueuedHostedService<TQueue> : BackgroundService
	where TQueue : class, IWorkQueue
{
	private readonly Container _container;

	public QueuedHostedService(Container container)
	{
		_container = container;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		// Verify that TService can be resolved
		_container.GetRegistration(typeof(TQueue), true);

		var queue = _container.GetInstance<TQueue>();

		await DoWork(queue, stoppingToken);
	}

	private async Task DoWork(TQueue queue, CancellationToken stoppingToken)
	{
		try
		{
			await TryDoWork(queue, stoppingToken);
		}
		catch
		{
			// Ignore
		}
	}

	private async Task TryDoWork(TQueue queue, CancellationToken stoppingToken)
	{
		while (true)
		{
			using (AsyncScopedLifestyle.BeginScope(_container))
			{
				var queuedWork = await queue.Dequeue(stoppingToken);

				var handler = _container.GetInstance<IWorkHandler>();

				await handler.HandleWork(queuedWork.WorkType, queuedWork.WorkInstance, stoppingToken);
			}
		}
	}
}