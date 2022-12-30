using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using System.Threading.Channels;

namespace DotnetCoreTemplate.Infrastructure.Background;

public class DefaultWorkQueue : IWorkQueue
{
	private readonly Channel<QueuedWork> _queue;

	public DefaultWorkQueue(QueueSettings settings)
	{
		var options = new BoundedChannelOptions(settings.Cpacity)
		{
			FullMode = BoundedChannelFullMode.Wait
		};

		_queue = Channel.CreateBounded<QueuedWork>(options);
	}

	public async Task Enqueue<TWork>(TWork work, CancellationToken cancellation = default)
		 where TWork : IWork
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

	public class QueueSettings
	{
		public QueueSettings(int cpacity)
		{
			Cpacity = cpacity;
		}

		public int Cpacity { get; }
	}
}