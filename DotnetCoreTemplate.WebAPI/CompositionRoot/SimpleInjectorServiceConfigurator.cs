using DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot;

public class SimpleInjectorServiceConfigurator
{
	private readonly IServiceCollection _services;
	private readonly IConfiguration _configuration;
	private readonly Container _container;

	public SimpleInjectorServiceConfigurator(
		IServiceCollection services,
		IConfiguration configuration,
		Container container)
	{
		_services = services;
		_configuration = configuration;
		_container = container;
	}

	public void Configure()
	{
		_container.IntegrateWithServiceCollection(_services, _configuration)
			.RegisterApplication()
			.RegisterInfrastructure()
			.RegisterWebApi();
	}
}