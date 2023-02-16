namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Extensions;

public static class SimpleInjectorComposerExtensions
{
	public static void UseComposer(this IServiceProvider serviceProvider, SimpleInjectorComposer composer)
	{
		composer.IntegrateWithServiceProvider(serviceProvider);
	}

	public static void UseMiddleware<TMiddleware>(this IApplicationBuilder app, SimpleInjectorComposer composer)
	{
		composer.DecorateRequestPipeline<TMiddleware>(app);
	}
}