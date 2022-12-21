using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using DotnetCoreTemplate.WebAPI.Utils;

namespace DotnetCoreTemplate.WebAPI.Extensions;

public static class MvcOptionsExtensions
{
	public static void UseGeneralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
	{
		opts.Conventions.Add(new RoutePrefixConvention(routeAttribute));
	}

	public static void UseGeneralRoutePrefix(this MvcOptions opts, string prefix)
	{
		opts.UseGeneralRoutePrefix(new RouteAttribute(prefix));
	}
}