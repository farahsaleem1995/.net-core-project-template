using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Extensions;

public static class MethodInvokeExtensions
{
	public static TCallDelegate MakeFastMethodInvoker<TCallDelegate>(this Type declaringType, string methodName)
		where TCallDelegate : Delegate
	{
		return MethodInvokeHelper.MakeFastInvoker<TCallDelegate>(declaringType, methodName);
	}
}