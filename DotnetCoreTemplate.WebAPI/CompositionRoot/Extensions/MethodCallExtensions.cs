using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Extensions;

public static class MethodCallExtensions
{
	public static TCallDelegate MakeFastMethodCaller<TCallDelegate>(this Type declaringType, string methodName)
		where TCallDelegate : Delegate
	{
		return MethodCallHelper.MakeFastCaller<TCallDelegate>(declaringType, methodName);
	}
}