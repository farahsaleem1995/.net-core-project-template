using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public class WorkQueueProcessor : IHostProcessor
{
	private readonly IWorkQueue _queue;
	private readonly IWorkerInvoker _workerInvoker;

	public WorkQueueProcessor(IWorkQueue queue, IWorkerInvoker workerInvoker)
	{
		_queue = queue;
		_workerInvoker = workerInvoker;
	}

	public async Task ProcessAsync(CancellationToken cancellation)
	{
		var work = await _queue.Dequeue(cancellation);

		await _workerInvoker.Invoke(work, cancellation);
	}
}