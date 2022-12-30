namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Host;

public interface IHostProcessor
{
	Task ProcessAsync(CancellationToken cancellation);
}