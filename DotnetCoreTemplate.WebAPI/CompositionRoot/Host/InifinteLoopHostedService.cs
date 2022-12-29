using DotnetCoreTemplate.WebAPI.CompositionRoot.Interface;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Reflection;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Host;

public class InifinteLoopHostedService<TProcessor> : BackgroundService
	where TProcessor : class, IHostProcessor
{
	private readonly Container _container;
	private readonly Dictionary<Type, InstanceProducer> _processorProducers;

	public InifinteLoopHostedService(
		Container container,
		InfiniteLoopHostSettings settings)
	{
		_container = container;

		_processorProducers = _container.GetTypesToRegister(typeof(IHostProcessor), settings.Assemblies)
			.ToDictionary(type => type,
				type => Lifestyle.Transient.CreateProducer(typeof(IHostProcessor), type, container));
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		// Verify that TService can be resolved
		var producer = _processorProducers[typeof(TProcessor)];

		var processor = (IHostProcessor)producer.GetInstance();

		await DoWork(processor, stoppingToken);
	}

	private async Task DoWork(IHostProcessor processor, CancellationToken stoppingToken)
	{
		try
		{
			await TryDoWork(processor, stoppingToken);
		}
		catch
		{
			// Ignore
		}
	}

	private async Task TryDoWork(IHostProcessor processor, CancellationToken stoppingToken)
	{
		while (true)
		{
			using (AsyncScopedLifestyle.BeginScope(_container))
			{
				await processor.ProcessAsync(stoppingToken);
			}
		}
	}

	public class InfiniteLoopHostSettings
	{
		public InfiniteLoopHostSettings(params Assembly[] assemblies)
		{
			Assemblies = assemblies;
		}

		public Assembly[] Assemblies { get; }
	}
}