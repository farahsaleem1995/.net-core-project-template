using DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Host;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;

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
		_container.IntegrateWithServiceCollection(_services, _configuration, SetupSimpleInjector)
			.RegisterDomainServices()
			.RegisterDataAccess()
			.RegisterIdentity()
			.RegisterUtilities()
			.RegisterBackgroundServices();
	}

	public void SetupSimpleInjector(SimpleInjectorAddOptions options)
	{
		options.AddAspNetCore()
			.AddControllerActivation()
			.AddViewComponentActivation()
			.AddPageModelActivation()
			.AddTagHelperActivation();

		options.AddLogging();
		options.AddLocalization();

		options.AddHostedService<InifinteLoopHostedService<WorkQueueProcessor>>();
	}
}