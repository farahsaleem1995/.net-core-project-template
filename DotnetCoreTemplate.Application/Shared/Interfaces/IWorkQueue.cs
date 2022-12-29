using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IWorkQueue
{
	Task Enqueue<TWork>(TWork work, CancellationToken cancellation = default)
		where TWork : IWork;

	Task<QueuedWork> Dequeue(CancellationToken cancellation = default);
}