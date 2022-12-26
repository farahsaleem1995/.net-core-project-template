using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public delegate object FastRequestHandler(object handler, object request, CancellationToken cancellation);

public class Director : IDirector
{
	private readonly Container _container;
	private readonly ILocalCache<Type, FastRequestHandler> _handlerDelegates;

	public Director(
		Container container,
		ILocalCache<Type, FastRequestHandler> handlerDelegates)
	{
		_container = container;
		_handlerDelegates = handlerDelegates;
	}

	public async Task<TResult> Send<TResult>(IRequest<TResult> request, CancellationToken cancellation)
	{
		var requestType = request.GetType();

		var handlerType = RequestHelper.MaketHandlerType<TResult>(requestType);

		return await CallHandler<TResult>(handlerType, request, cancellation);
	}

	private async Task<TResult> CallHandler<TResult>(
		Type handlerType, object request, CancellationToken cancellation)
	{
		var handlerInstance = _container.GetInstance(handlerType);

		var fastHandler = MakeFastHandler<TResult>(handlerType);

		var result = fastHandler(handlerInstance, request, cancellation);

		return await (Task<TResult>)result;
	}

	private FastRequestHandler MakeFastHandler<TResult>(Type handlerType)
	{
		return _handlerDelegates.Get(handlerType, RequestHelper.MakeFastHandler<TResult>);
	}
}