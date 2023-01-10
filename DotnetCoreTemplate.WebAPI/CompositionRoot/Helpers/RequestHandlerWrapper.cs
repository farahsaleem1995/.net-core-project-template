using DotnetCoreTemplate.Application.Shared.Interfaces;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

public abstract class RequestHandlerWrapperBase
{
	public abstract Task<object?> Handle(Container container, object request,
		CancellationToken cancellation);
}

public abstract class RequestHandlerWrapper<TResult> : RequestHandlerWrapperBase
{
	public abstract Task<TResult> Handle(Container container, IRequest<TResult> request,
		CancellationToken cancellation);
}

public class RequestHandlerWrapperImpl<TRequest, TResult> : RequestHandlerWrapper<TResult>
	where TRequest : IRequest<TResult>
{
	public override async Task<object?> Handle(Container container, object request,
		CancellationToken cancellation)
	{
		return await Handle(container, (IRequest<TResult>)request, cancellation);
	}

	public override async Task<TResult> Handle(Container container, IRequest<TResult> request,
		CancellationToken cancellation)
	{
		var handler = container.GetInstance<IRequestHandler<TRequest, TResult>>();

		return await handler.Handle((TRequest)request, cancellation);
	}
}