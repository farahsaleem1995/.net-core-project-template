using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Adapters;

public delegate object FastWorkExecutor(object worker, object work, CancellationToken cancellation);

public class WorkExecutor : IWorkExecutor
{
	private readonly Container _container;
	private readonly ILocalCache<Type, FastWorkExecutor> _workExecutors;

	public WorkExecutor(
		Container container,
		ILocalCache<Type, FastWorkExecutor> workExecutors)
	{
		_container = container;
		_workExecutors = workExecutors;
	}

	public async Task Execute(object work, CancellationToken cancellation)
	{
		var workType = work.GetType();

		var workerType = WorkerHelper.MaketWorkerType(workType);

		await CallWorker(workerType, work, cancellation);
	}

	private async Task CallWorker(Type workerType, object work, CancellationToken cancellation)
	{
		var workerInstance = _container.GetInstance(workerType);

		var fastExecutor = MakeFastExecutor(workerType);

		var result = fastExecutor(workerInstance, work, cancellation);

		await (Task)result;
	}

	private FastWorkExecutor MakeFastExecutor(Type workerType)
	{
		return _workExecutors.Get(workerType, WorkerHelper.MakeFastExecutor);
	}
}