using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using System.Reflection;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

public static class RequestHelper
{
	private static readonly MethodInfo _callHandlerOpenGenericMethod =
		typeof(RequestHelper).GetTypeInfo().GetDeclaredMethod(nameof(CallHandler))!;

	public static RequestHandlerType MaketHandlerType<TResult>(Type requestType)
	{
		return new(typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResult)));
	}

	public static FastRequestHandler MakeFastHandler<TResult>(Type handlerType)
	{
		var methodInfo = handlerType.GetMethod("Handle")!;
		var parameters = methodInfo.GetParameters().ToArray();

		var requestType = parameters[0].ParameterType;
		var canellationType = parameters[1].ParameterType;
		var returnType = typeof(Task<TResult>);

		var delegateType = typeof(Func<,,,>).MakeGenericType(handlerType, requestType, canellationType, returnType);
		var handleDelegate = methodInfo!.CreateDelegate(delegateType);

		return CreateHandleWrapperDelegate(handleDelegate, handlerType, requestType, returnType);
	}

	private static FastRequestHandler CreateHandleWrapperDelegate(
		Delegate handleDelegate, Type handlerType, Type requestType, Type returnType)
	{
		var wrapperDelegateMethod = _callHandlerOpenGenericMethod.MakeGenericMethod(
			handlerType, requestType, returnType);

		var wrapperDelegate = wrapperDelegateMethod.CreateDelegate(typeof(FastRequestHandler), handleDelegate);

		return (FastRequestHandler)wrapperDelegate;
	}

	private static TResult CallHandler<THandler, TRequest, TResult>(
		Func<THandler, TRequest, CancellationToken, TResult> deleg,
		object handler,
		object request,
		CancellationToken cancellation)
	{
		return deleg((THandler)handler, (TRequest)request, cancellation)!;
	}
}