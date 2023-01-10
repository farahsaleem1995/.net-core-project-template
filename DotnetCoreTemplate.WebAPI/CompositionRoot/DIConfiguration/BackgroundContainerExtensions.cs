using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Background;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Host;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using Quartz;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class BackgroundContainerExtensions
{
	public static Container RegisterBackgroundServices(this Container container)
	{
		container.Register<IWorkScheduler, QuartzWorkScheduler>(Lifestyle.Singleton);

		container.Register<IWorkQueue, DefaultWorkQueue>(Lifestyle.Singleton);
		container.RegisterInstance(new DefaultWorkQueue.QueueSettings(100));

		container.RegisterQuartzJob()
			.RegisterHostedServices();

		return container;
	}

	private static Container RegisterQuartzJob(this Container container)
	{
		var jobTypesToRegisterOptions = new TypesToRegisterOptions
		{
			IncludeGenericTypeDefinitions = true,
			IncludeComposites = false
		};

		var assemblies = new[] { typeof(QuartzJob<>).Assembly };
		var types = container.GetTypesToRegister<IJob>(assemblies, jobTypesToRegisterOptions);
		foreach (var type in types)
		{
			container.Register(type);
		};

		return container;
	}

	private static Container RegisterHostedServices(this Container container)
	{
		container.RegisterInstance(
			new DefaultHostedService<WorkQueueProcessor>.Settings(typeof(IProcessor).Assembly));

		return container;
	}
}