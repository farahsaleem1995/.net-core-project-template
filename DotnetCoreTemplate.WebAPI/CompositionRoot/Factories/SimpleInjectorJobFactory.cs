using DotnetCoreTemplate.Infrastructure.Background;
using Quartz;
using Quartz.Spi;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Factories;

public class SimpleInjectorJobFactory : IJobFactory
{
	private readonly Container _container;

	public SimpleInjectorJobFactory(Container container)
	{
		_container = container;
	}

	public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
	{
		return new AsyncScopedJobDecorator(
			_container,
			() => (IJob)_container.GetInstance(bundle.JobDetail.JobType));
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