namespace DotnetCoreTemplate.Infrastructure.Background;

public interface IWorkExecutor
{
	Task Execute<TWork>(TWork work, CancellationToken cancellation);
}