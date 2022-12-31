using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class ContainerExtensions
{
	public static Container RegisterSettings<T>(this Container container, IConfiguration configuration, string key)
		where T : class
	{
		var settings = configuration.GetOrThrow<T>(key);

		container.RegisterInstance(settings);

		return container;
	}
}