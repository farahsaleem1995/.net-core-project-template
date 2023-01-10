using DotnetCoreTemplate.Application.Shared.Interfaces;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

public abstract class WorkerWrapperBase
{
	public abstract Task Execute(Container container, IWork work, CancellationToken cancellation);
}

public class WorkerWrapperImpl<TWork> : WorkerWrapperBase
	where TWork : IWork
{
	public override async Task Execute(Container container, IWork work, CancellationToken cancellation)
	{
		var worker = container.GetInstance<IWorker<TWork>>();

		await worker.Execute((TWork)work, cancellation);
	}
}