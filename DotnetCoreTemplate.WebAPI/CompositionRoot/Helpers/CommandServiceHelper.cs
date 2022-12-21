using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using System.Reflection;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

public static class CommandServiceHelper
{
	private static readonly MethodInfo _callCommandServiceExecuteOpenGenericMethod =
		typeof(CommandServiceHelper).GetTypeInfo().GetDeclaredMethod(nameof(CallCommandServiceExecute))!;

	public static Type MakeGenericType<TResult>(Type commandType)
	{
		return typeof(ICommandService<,>).MakeGenericType(commandType, typeof(TResult));
	}

	public static Delegate MakeFastExecutionDelegate<TResult>(Type serviceType)
	{
		var executeMethod = serviceType.GetMethod("Execute")!;

		var executeReturnType = executeMethod.ReturnType;
		var executeParameters = executeMethod.GetParameters().ToArray();

		var commandParameterType = executeParameters[0].ParameterType;
		var canellationParameterType = executeParameters[1].ParameterType;

		var executeDelegateType = typeof(Func<,,,>).MakeGenericType(
			serviceType, commandParameterType, canellationParameterType, executeReturnType);

		var executeDelegate = executeMethod!.CreateDelegate(executeDelegateType);

		var wrapperDelegateMethod = _callCommandServiceExecuteOpenGenericMethod.MakeGenericMethod(
			serviceType, commandParameterType, executeReturnType);

		return wrapperDelegateMethod.CreateDelegate(
			typeof(Func<object, object, CancellationToken, Task<TResult>>), executeDelegate);
	}

	private static TResult CallCommandServiceExecute<TService, TCommand, TResult>(
		Func<TService, TCommand, CancellationToken, TResult> deleg,
		object service,
		object command,
		CancellationToken cancellation)
	{
		return deleg((TService)service, (TCommand)command, cancellation)!;
	}
}