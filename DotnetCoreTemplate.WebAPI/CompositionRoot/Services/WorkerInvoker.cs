using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Extensions;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public delegate object FastWorkExecutor(object worker, object work, object cancellation);

public class WorkerInvoker : IWorkerInvoker
{
	private readonly Container _container;
	private readonly ILocalCache<Type, FastWorkExecutor> _workExecutors;

	public WorkerInvoker(
		Container container,
		ILocalCache<Type, FastWorkExecutor> workExecutors)
	{
		_container = container;
		_workExecutors = workExecutors;
	}

	public async Task Invoke(IWork work, CancellationToken cancellation)
	{
		if (work == null)
		{
			throw new ArgumentNullException(nameof(work));
		}

		var workType = work.GetType();

		await ExecuteWork(workType, work, cancellation);
	}

	private async Task ExecuteWork(Type workType, object work, CancellationToken cancellation)
	{
		var workerType = typeof(IWorker<>).MakeGenericType(workType);

		var workerInstance = _container.GetInstance(workerType);

		var fastExecutor = MakeFastExecutor(workerType);

		var result = fastExecutor(workerInstance, work, cancellation);

		await (Task)result;
	}

	private FastWorkExecutor MakeFastExecutor(Type workerType)
	{
		return _workExecutors.Get(workerType,
			_ => workerType.MakeFastMethodCaller<FastWorkExecutor>("Execute"));
	}
}