using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using System.Reflection;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

public static class CommandServiceHelper
{
	private static readonly MethodInfo _callCommandServiceExecuteOpenGenericMethod =
		typeof(CommandServiceHelper).GetTypeInfo().GetDeclaredMethod(nameof(CallCommandServiceExecute))!;

	public static CommandServiceType MakeCommandServiceType(Type commandType, Type resultType)
	{
		return new(typeof(ICommandService<,>).MakeGenericType(commandType, resultType));
	}

	public static CommandExecutor MakeFastCommandExecutor(Type serviceType)
	{
		var methodInfo = serviceType.GetMethod("Execute")!;

		var returnType = methodInfo.ReturnType;
		var parameters = methodInfo.GetParameters().ToArray();

		var commandType = parameters[0].ParameterType;
		var canellationType = parameters[1].ParameterType;

		var delegateType = typeof(Func<,,,>).MakeGenericType(serviceType, commandType, canellationType, returnType);
		var executeDelegate = methodInfo!.CreateDelegate(delegateType);

		return CreateExecuteWrapperDelegate(executeDelegate, serviceType, commandType, returnType);
	}

	private static CommandExecutor CreateExecuteWrapperDelegate(
		Delegate executeDelegate, Type serviceType, Type commandType, Type returnType)
	{
		var wrapperDelegateMethod = _callCommandServiceExecuteOpenGenericMethod.MakeGenericMethod(
			serviceType, commandType, returnType);

		var wrapperDelegate = wrapperDelegateMethod.CreateDelegate(typeof(CommandExecutor), executeDelegate);

		return (CommandExecutor)wrapperDelegate;
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