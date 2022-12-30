using SimpleInjector;
using SimpleInjector.Lifestyles;
using System.Reflection;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Host;

public class DefaultHostedService<TProcessor> : BackgroundService
	where TProcessor : class, IProcessor
{
	private readonly Container _container;
	private readonly Dictionary<Type, InstanceProducer> _processorProducers;

	public DefaultHostedService(Container container, Settings settings)
	{
		_container = container;

		_processorProducers = _container.GetTypesToRegister(typeof(IProcessor), settings.Assemblies)
			.ToDictionary(type => type,
				type => Lifestyle.Transient.CreateProducer(typeof(IProcessor), type, container));
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var producer = _processorProducers[typeof(TProcessor)];

		var processor = (IProcessor)producer.GetInstance();

		await DoWork(processor, stoppingToken);
	}

	private async Task DoWork(IProcessor processor, CancellationToken stoppingToken)
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

	private async Task TryDoWork(IProcessor processor, CancellationToken stoppingToken)
	{
		while (true)
		{
			using (AsyncScopedLifestyle.BeginScope(_container))
			{
				await processor.ProcessAsync(stoppingToken);
			}
		}
	}

	public class Settings
	{
		public Settings(params Assembly[] assemblies)
		{
			Assemblies = assemblies;
		}

		public Assembly[] Assemblies { get; }
	}
}