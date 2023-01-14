namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IQueue
{
	Task Enqueue(IWork work, CancellationToken cancellation = default);

	Task<IWork> Dequeue(CancellationToken cancellation = default);
}