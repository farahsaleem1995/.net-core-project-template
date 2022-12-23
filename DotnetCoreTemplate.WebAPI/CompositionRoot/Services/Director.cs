using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public record RequestHandlerType(Type Type);

public delegate object FastRequestHandler(object handler, object request, CancellationToken cancellation);

public class Director : IDirector
{
	private readonly Container _container;
	private readonly ILocalCache<Type, RequestHandlerType> _handlerTypes;
	private readonly ILocalCache<Type, FastRequestHandler> _handlerDelegates;

	public Director(
		Container container,
		ILocalCache<Type, RequestHandlerType> handlerTypes,
		ILocalCache<Type, FastRequestHandler> handlerDelegates)
	{
		_container = container;
		_handlerTypes = handlerTypes;
		_handlerDelegates = handlerDelegates;
	}

	public async Task<TResult> Send<TResult>(IRequest<TResult> request, CancellationToken cancellation)
	{
		var requestType = request.GetType();
		var handlerType = GetHandlerType<TResult>(requestType);

		return await CallHandler<TResult>(request, handlerType.Type, cancellation);
	}

	private RequestHandlerType GetHandlerType<TResult>(Type requestType)
	{
		return _handlerTypes.Get(requestType, RequestHelper.MaketHandlerType<TResult>);
	}

	private async Task<TResult> CallHandler<TResult>(
		object request, Type requestType, CancellationToken cancellation)
	{
		var handlerInstance = _container.GetInstance(requestType);

		var fastHandler = MakeFastHandler<TResult>(requestType);

		var result = fastHandler(handlerInstance, request, cancellation);

		return await (Task<TResult>)result;
	}

	private FastRequestHandler MakeFastHandler<TResult>(Type handlerType)
	{
		return _handlerDelegates.Get(handlerType, RequestHelper.MakeFastHandler<TResult>);
	}
}