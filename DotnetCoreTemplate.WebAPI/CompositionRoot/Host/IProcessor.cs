namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Host;

public interface IProcessor
{
	Task ProcessAsync(CancellationToken cancellation);
}