using System.Reflection;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

public static class MethodInvokeHelper
{
	private static readonly MethodInfo _callMethodInfo =
		typeof(MethodInvokeHelper).GetTypeInfo().GetDeclaredMethod(nameof(CallMethod))!;

	public static TCallDelegate MakeFastInvoker<TCallDelegate>(Type declaringType, string methodName)
		where TCallDelegate : Delegate
	{
		var methodInfo = declaringType.GetMethod(methodName)!;
		var parameters = methodInfo.GetParameters().ToArray();

		var firstParamType = parameters[0].ParameterType;
		var secondParamType = parameters[1].ParameterType;
		var returnType = methodInfo.ReturnType;

		var methodDelegateType = typeof(Func<,,,>).MakeGenericType(
			declaringType, firstParamType, secondParamType, returnType);

		var methodDelegate = methodInfo!.CreateDelegate(methodDelegateType);

		return CreateMethodWrapperDelegate<TCallDelegate>(
			methodDelegate, declaringType, firstParamType, secondParamType, returnType);
	}

	private static TCallDelegate CreateMethodWrapperDelegate<TCallDelegate>(
		Delegate executeDelegate, Type declaringType, Type firstParamType, Type secondParamType, Type returnType)
		where TCallDelegate : Delegate
	{
		var wrapperMethod = _callMethodInfo.MakeGenericMethod(
			declaringType, firstParamType, secondParamType, returnType);

		return wrapperMethod.CreateDelegate<TCallDelegate>(executeDelegate);
	}

	private static TReturn CallMethod<TDeclaring, TParam1, TParam2, TReturn>(
		Func<TDeclaring, TParam1, TParam2, TReturn> deleg,
		object declaringInstance,
		object firstArg,
		object secondArg)
	{
		return deleg((TDeclaring)declaringInstance, (TParam1)firstArg, (TParam2)secondArg)!;
	}
}