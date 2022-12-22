using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using System.Reflection;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

public static class OperationServiceHelper
{
	private static readonly MethodInfo _callOperationServiceExecuteOpenGenericMethod =
		typeof(OperationServiceHelper).GetTypeInfo().GetDeclaredMethod(nameof(CallOperationServiceExecute))!;

	public static OperationServiceType MakeOperationServiceType(Type operationType, Type resultType)
	{
		return new(typeof(IOperationService<,>).MakeGenericType(operationType, resultType));
	}

	public static OperationExecutor MakeFastOperationExecutor(Type serviceType)
	{
		var methodInfo = serviceType.GetMethod("Execute")!;

		var returnType = methodInfo.ReturnType;
		var parameters = methodInfo.GetParameters().ToArray();

		var operationType = parameters[0].ParameterType;
		var canellationType = parameters[1].ParameterType;

		var delegateType = typeof(Func<,,,>).MakeGenericType(serviceType, operationType, canellationType, returnType);
		var executeDelegate = methodInfo!.CreateDelegate(delegateType);

		return CreateExecuteWrapperDelegate(executeDelegate, serviceType, operationType, returnType);
	}

	private static OperationExecutor CreateExecuteWrapperDelegate(
		Delegate executeDelegate, Type serviceType, Type operationType, Type returnType)
	{
		var wrapperDelegateMethod = _callOperationServiceExecuteOpenGenericMethod.MakeGenericMethod(
			serviceType, operationType, returnType);

		var wrapperDelegate = wrapperDelegateMethod.CreateDelegate(typeof(OperationExecutor), executeDelegate);

		return (OperationExecutor)wrapperDelegate;
	}

	private static TResult CallOperationServiceExecute<TService, TOperation, TResult>(
		Func<TService, TOperation, CancellationToken, TResult> deleg,
		object service,
		object operation,
		CancellationToken cancellation)
	{
		return deleg((TService)service, (TOperation)operation, cancellation)!;
	}
}