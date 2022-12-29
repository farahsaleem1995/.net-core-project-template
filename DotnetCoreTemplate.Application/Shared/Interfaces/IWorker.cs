namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IWorker<TWork> where TWork : IWork
{
	Task Execute(TWork work, CancellationToken cancellation);
}