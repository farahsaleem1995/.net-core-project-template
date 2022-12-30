using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Background;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Host;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using Quartz;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class BackgroundContainerExtensions
{
	public static Container RegisterBackgroundServices(this Container container)
	{
		container.RegisterWorkers();

		container.RegisterQuartzJob();

		container.RegisterWorkQueue();

		container.RegisterHostedServices();

		return container;
	}

	private static void RegisterWorkers(this Container container)
	{
		container.Register<IWorkScheduler, QuartzWorkScheduler>(Lifestyle.Singleton);

		container.Register(typeof(IWorker<>), typeof(IWorker<>).Assembly);
	}

	private static void RegisterQuartzJob(this Container container)
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
	}

	private static void RegisterWorkQueue(this Container container)
	{
		container.Register<IWorkQueue, DefaultWorkQueue>(Lifestyle.Singleton);

		container.RegisterInstance(new DefaultWorkQueue.QueueSettings(100));
	}

	private static void RegisterHostedServices(this Container container)
	{
		container.RegisterInstance(
			new InifinteLoopHostedService<WorkQueueProcessor>
				.InfiniteLoopHostSettings(typeof(IHostProcessor).Assembly));
	}
}