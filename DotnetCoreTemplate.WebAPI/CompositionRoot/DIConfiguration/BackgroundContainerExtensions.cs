using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Background;
using DotnetCoreTemplate.Infrastructure.Interfaces;
using DotnetCoreTemplate.Infrastructure.Services;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Services;
using Quartz;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class BackgroundContainerExtensions
{
	public static Container RegisterBackgroundServices(this Container container)
	{
		container.Register<IQueueProvider, QuartzQueueProvider>(Lifestyle.Singleton);

		container.Register<IWorkExecutor, WorkExecutor>();

		container.Register(typeof(IWorker<>), typeof(IWorker<>).Assembly);

		container.RegisterQuartzJob();

		return container;
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
}