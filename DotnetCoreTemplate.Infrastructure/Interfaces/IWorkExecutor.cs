namespace DotnetCoreTemplate.Infrastructure.Interfaces;

public interface IWorkExecutor
{
	Task Execute(object work, CancellationToken cancellation);
}