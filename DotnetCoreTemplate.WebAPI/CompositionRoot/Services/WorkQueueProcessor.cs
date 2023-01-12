using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Host;
using SimpleInjector;
using System.Collections.Concurrent;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public class WorkQueueProcessor : IProcessor
{
	private static readonly ConcurrentDictionary<Type, WorkHandlerWrapperBase> _workHandlers = new();

	private readonly Container _container;
	private readonly IWorkQueue _queue;

	public WorkQueueProcessor(Container container, IWorkQueue queue)
	{
		_container = container;
		_queue = queue;
	}

	public async Task ProcessAsync(CancellationToken cancellation)
	{
		var work = await _queue.Dequeue(cancellation);

		var workType = work.GetType();

		var wrapper = GetOrAddWorkHandler(workType);

		await wrapper.Handler(_container, work, cancellation);
	}

	public WorkHandlerWrapperBase GetOrAddWorkHandler(Type workType)
	{
		return _workHandlers.GetOrAdd(workType, _ =>
		{
			var wrapper = Activator.CreateInstance(typeof(WorkHandlerWrapperImpl<>).MakeGenericType(workType));

			if (wrapper == null)
			{
				throw new InvalidOperationException($"Could not create wrapper type for {workType}");
			}

			return (WorkHandlerWrapperBase)wrapper;
		});
	}
}