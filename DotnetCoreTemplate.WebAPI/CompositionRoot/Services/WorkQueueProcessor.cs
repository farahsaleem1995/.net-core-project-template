using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public class WorkQueueProcessor : IHostProcessor
{
	private readonly IWorkQueue _queue;
	private readonly IWorkHandler _handler;

	public WorkQueueProcessor(
		IWorkQueue queue,
		IWorkHandler handler)
	{
		_queue = queue;
		_handler = handler;
	}

	public async Task ProcessAsync(CancellationToken cancellation)
	{
		var queuedWork = await _queue.Dequeue(cancellation);

		await _handler.HandleWork(queuedWork.WorkType, queuedWork.WorkInstance, cancellation);
	}
}