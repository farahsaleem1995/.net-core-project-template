namespace DotnetCoreTemplate.Infrastructure.Interfaces;

public interface IWorkExecutor
{
	Task Execute<TWork>(TWork work, CancellationToken cancellation);
}