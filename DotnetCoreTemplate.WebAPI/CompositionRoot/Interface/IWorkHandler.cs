namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;

public interface IWorkHandler
{
	Task HandleWork(Type workType, object work, CancellationToken cancellation);
}