using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IWorkQueue
{
	Task Enqueue(IWork work, CancellationToken cancellation = default);

	Task<IWork> Dequeue(CancellationToken cancellation = default);
}