namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IWorkScheduler
{
	Task Schedule<TWork>(TWork work, DateTime firingTime, CancellationToken cancellation = default);
}