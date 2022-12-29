namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;

public interface IHostProcessor
{
	Task ProcessAsync(CancellationToken cancellation);
}