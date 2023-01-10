using DotnetCoreTemplate.Application.Shared.Interfaces;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

public abstract class WorkHandlerWrapperBase
{
	public abstract Task Handler(Container container, IWork work, CancellationToken cancellation);
}

public class WorkHandlerWrapperImpl<TWork> : WorkHandlerWrapperBase
	where TWork : IWork
{
	public override async Task Handler(Container container, IWork work, CancellationToken cancellation)
	{
		var worker = container.GetInstance<IWorkHandler<TWork>>();

		await worker.Handle((TWork)work, cancellation);
	}
}