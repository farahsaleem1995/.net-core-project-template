using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using System.Threading.Channels;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public class AspNetWorkQueue : IWorkQueue
{
	private readonly Channel<QueuedWork> _queue;

	public AspNetWorkQueue(int capacity)
	{
		var options = new BoundedChannelOptions(capacity)
		{
			FullMode = BoundedChannelFullMode.Wait
		};

		_queue = Channel.CreateBounded<QueuedWork>(options);
	}

	public async Task Enqueue<TWork>(TWork work, CancellationToken cancellation = default)
	{
		if (work == null)
		{
			throw new ArgumentNullException(nameof(work));
		}

		await _queue.Writer.WriteAsync(new QueuedWork(work), cancellation);
	}

	public async Task<QueuedWork> Dequeue(CancellationToken cancellation = default)
	{
		return await _queue.Reader.ReadAsync(cancellation);
	}
}