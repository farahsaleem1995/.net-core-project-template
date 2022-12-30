using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Host;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public class WorkQueueProcessor : IProcessor
{
	private readonly IWorkQueue _queue;
	private readonly IDirector _director;

	public WorkQueueProcessor(IWorkQueue queue, IDirector director)
	{
		_queue = queue;
		_director = director;
	}

	public async Task ProcessAsync(CancellationToken cancellation)
	{
		var work = await _queue.Dequeue(cancellation);

		await _director.ExecuteWork(work, cancellation);
	}
}