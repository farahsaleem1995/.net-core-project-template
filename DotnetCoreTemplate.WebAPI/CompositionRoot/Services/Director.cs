using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Shared;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public class Director : IDirector
{
	private readonly Container _container;
	private readonly ILocalCache<Type, TwoParameterMethodInvoker> _invokers;

	public Director(Container container, ILocalCache<Type, TwoParameterMethodInvoker> invokers)
	{
		_container = container;
		_invokers = invokers;
	}

	public async Task<TResult> SendRequest<TResult>(IRequest<TResult> request, CancellationToken cancellation)
	{
		var requestType = request.GetType();

		var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResult));

		var invoker = MakeFastMethodInvoker(handlerType, "Handle", new[] { requestType, typeof(CancellationToken) });

		return await (Task<TResult>)Invoke(handlerType, invoker, new object[] { request, cancellation });
	}

	public async Task DispatchEvent(DomainEvent domainEvent, CancellationToken cancellation)
	{
		var eventType = domainEvent.GetType();

		var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

		var invoker = MakeFastMethodInvoker(handlerType, "Handle", new[] { eventType, typeof(CancellationToken) });

		await (Task)Invoke(handlerType, invoker, new object[] { domainEvent, cancellation });
	}

	public async Task ExecuteWork(IWork work, CancellationToken cancellation)
	{
		var workType = work.GetType();

		var workerType = typeof(IWorker<>).MakeGenericType(workType);

		var invoker = MakeFastMethodInvoker(workerType, "Execute", new[] { workType, typeof(CancellationToken) });

		await (Task)Invoke(workerType, invoker, new object[] { work, cancellation });
	}

	private TwoParameterMethodInvoker MakeFastMethodInvoker(Type declaringType, string methodName, Type[] parameters)
	{
		return _invokers.Get(declaringType, _ =>
		{
			var helper = ObjectMethodHelper.Create(declaringType);

			return helper.GetTwoParameterInvoker(methodName, parameters[0], parameters[1]);
		});
	}

	private object Invoke(Type decalringType, TwoParameterMethodInvoker invoker, params object[] args)
	{
		var serviceInstance = _container.GetInstance(decalringType);

		return invoker(serviceInstance, args[0], args[1]);
	}
}