namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IScheduleProvider
{
	Task Schedule<TWork>(TWork work, DateTime firingTime, CancellationToken cancellation = default);
}