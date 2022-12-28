namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IWorker<TWork>
{
	Task Execute(TWork work, CancellationToken cancellation);
}