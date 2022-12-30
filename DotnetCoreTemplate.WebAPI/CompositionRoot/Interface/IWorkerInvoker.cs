namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;

public interface IWorkerInvoker
{
	Task Invoke(Type workType, object work, CancellationToken cancellation);
}