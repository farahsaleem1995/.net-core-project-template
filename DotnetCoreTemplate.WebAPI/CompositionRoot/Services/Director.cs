using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Extensions;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public delegate object FastInvoker(object instance, object parameterObject, object cancellationToken);

public class Director : IDirector
{
	private readonly Container _container;
	private readonly ILocalCache<Type, FastInvoker> _invokers;

	public Director(Container container, ILocalCache<Type, FastInvoker> invokers)
	{
		_container = container;
		_invokers = invokers;
	}

	public async Task<TResult> SendRequest<TResult>(IRequest<TResult> request, CancellationToken cancellation)
	{
		var requestType = request.GetType();

		var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResult));

		return await (Task<TResult>)FastInvoke(handlerType, "Handle", new object[] { request, cancellation });
	}

	public Task DispatchEvent(DomainEvent domainEvent, CancellationToken cancellation)
	{
		var eventType = domainEvent.GetType();

		var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

		return (Task)FastInvoke(handlerType, "Handle", new object[] { domainEvent, cancellation });
	}

	public Task ExecuteWork(IWork work, CancellationToken cancellation)
	{
		var workType = work.GetType();

		var workerType = typeof(IWorker<>).MakeGenericType(workType);

		return (Task)FastInvoke(workerType, "Execute", new object[] { work, cancellation });
	}

	private object FastInvoke(Type decalringType, string methodName, params object[] parameters)
	{
		var serviceInstance = _container.GetInstance(decalringType);

		var fastInvoker = MakeFastInvoker(decalringType, methodName);

		return fastInvoker(serviceInstance, parameters[0], parameters[1]);
	}

	private FastInvoker MakeFastInvoker(Type workerType, string methodName)
	{
		return _invokers.Get(workerType,
			_ => workerType.MakeFastMethodInvoker<FastInvoker>(methodName));
	}
}