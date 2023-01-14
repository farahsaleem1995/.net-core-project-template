namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IScheduler
{
	Task Schedule<TWork>(TWork work, DateTime firingTime, CancellationToken cancellation = default)
		 where TWork : IWork;
}