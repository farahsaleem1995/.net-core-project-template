using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;

public interface IWorkerInvoker
{
	Task Invoke(IWork work, CancellationToken cancellation);
}