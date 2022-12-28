using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Adapters;
using System.Reflection;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

public class WorkerHelper
{
	private static readonly MethodInfo _callExecuteOpenGenericMethod =
		typeof(WorkerHelper).GetTypeInfo().GetDeclaredMethod(nameof(CallExecute))!;

	public static Type MaketWorkerType(Type workType)
	{
		return typeof(IWorker<>).MakeGenericType(workType);
	}

	public static FastWorkExecutor MakeFastExecutor(Type workerType)
	{
		var methodInfo = workerType.GetMethod("Execute")!;
		var parameters = methodInfo.GetParameters().ToArray();

		var workType = parameters[0].ParameterType;
		var canellationType = parameters[1].ParameterType;
		var returnType = methodInfo.ReturnType;

		var delegateType = typeof(Func<,,,>).MakeGenericType(workerType, workType, canellationType, returnType);
		var executeDelegate = methodInfo!.CreateDelegate(delegateType);

		return CreateHandleWrapperDelegate(executeDelegate, workerType, workType, returnType);
	}

	private static FastWorkExecutor CreateHandleWrapperDelegate(
		Delegate executeDelegate, Type workerType, Type workType, Type returnType)
	{
		var wrapperDelegateMethod = _callExecuteOpenGenericMethod.MakeGenericMethod(
			workerType, workType, returnType);

		var wrapperDelegate = wrapperDelegateMethod.CreateDelegate(typeof(FastWorkExecutor), executeDelegate);

		return (FastWorkExecutor)wrapperDelegate;
	}

	private static TResult CallExecute<TWorker, TWork, TResult>(
		Func<TWorker, TWork, CancellationToken, TResult> deleg,
		object handler,
		object request,
		CancellationToken cancellation)
	{
		return deleg((TWorker)handler, (TWork)request, cancellation)!;
	}
}