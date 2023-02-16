using DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Host;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using SimpleInjector;
using SimpleInjector.Integration.ServiceCollection;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot;

public class SimpleInjectorComposer
{
	private readonly Container _container;
	private readonly IServiceCollection _services;

	public SimpleInjectorComposer(IServiceCollection services)
	{
		_container = new Container();
		_services = services;
	}

	public void Compose(IConfiguration configuration)
	{
		_container.IntegrateWithServiceCollection(_services, configuration, SetupSimpleInjector)
			.RegisterDomainServices()
			.RegisterDataAccess()
			.RegisterIdentity(configuration)
			.RegisterUtilities()
			.RegisterBackgroundServices();
	}

	private void SetupSimpleInjector(SimpleInjectorAddOptions options)
	{
		options.AddAspNetCore()
			.AddControllerActivation()
			.AddViewComponentActivation()
			.AddPageModelActivation()
			.AddTagHelperActivation();

		options.AddLogging();
		options.AddLocalization();

		options.AddHostedService<DefaultHostedService<WorkQueueProcessor>>();
	}

	public void IntegrateWithServiceProvider(IServiceProvider serviceProvider)
	{
		serviceProvider.UseSimpleInjector(_container);

		_container.Verify();
	}

	public void DecorateRequestPipeline<TMiddleware>(IApplicationBuilder app)
	{
		app.UseMiddleware<TMiddleware>(_container);
	}
}