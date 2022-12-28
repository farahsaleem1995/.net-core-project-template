using DotnetCoreTemplate.Infrastructure.Background;
using Quartz;
using Quartz.Spi;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Factories;

public class SimpleInjectorJobFactory : IJobFactory
{
	private readonly Container _container;
	private readonly Dictionary<Type, InstanceProducer> _jobProducers;

	public SimpleInjectorJobFactory(Container container)
	{
		_container = container;

		// By creating producers, jobs can be decorated.
		var assemblies = new[] { typeof(QuartzJob).Assembly };
		_jobProducers = container.GetTypesToRegister(typeof(IJob), assemblies).ToDictionary(
			type => type,
			type => Lifestyle.Transient.CreateProducer(typeof(IJob), type, container));
	}

	public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
	{
		var jobProducer = _jobProducers[bundle.JobDetail.JobType];

		return new AsyncScopedJobDecorator(
			_container,
			() => (IJob)jobProducer.GetInstance());
	}

	public void ReturnJob(IJob job)
	{
		// This will be handled automatically by Simple Injector
	}

	private sealed class AsyncScopedJobDecorator : IJob
	{
		private readonly Container _container;
		private readonly Func<IJob> _decoratee;

		public AsyncScopedJobDecorator(
			Container container, Func<IJob> decoratee)
		{
			_container = container;
			_decoratee = decoratee;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			using (AsyncScopedLifestyle.BeginScope(_container))
			{
				var job = _decoratee();
				await job.Execute(context);
			}
		}
	}
}