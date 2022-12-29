using DotnetCoreTemplate.Infrastructure.Background;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Adapters;

public class WorkExecutorAdapter : IWorkExecutor
{
	private readonly IWorkHandler _workHandler;

	public WorkExecutorAdapter(IWorkHandler workHandler)
	{
		_workHandler = workHandler;
	}

	public async Task Execute<TWork>(TWork work, CancellationToken cancellation)
	{
		if (work == null)
		{
			throw new ArgumentNullException(nameof(work));
		}

		await _workHandler.HandleWork(typeof(TWork), work, cancellation);
	}
}