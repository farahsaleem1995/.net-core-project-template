using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Extensions;
using Quartz;

namespace DotnetCoreTemplate.Infrastructure.Background;

public class QuartzWorkScheduler : IWorkScheduler
{
	private readonly ISchedulerFactory _schedulerFactory;

	public QuartzWorkScheduler(ISchedulerFactory schedulerFactory)
	{
		_schedulerFactory = schedulerFactory;
	}

	public async Task Schedule<TWork>(TWork work, DateTime firingTime, CancellationToken cancellation = default)
		 where TWork : IWork
	{
		if (work == null)
		{
			throw new ArgumentNullException(nameof(work));
		}

		await SchedulrJob(work, firingTime, cancellation);
	}

	private async Task SchedulrJob<TWork>(TWork work, DateTime firingTime, CancellationToken cancellation)
		where TWork : IWork
	{
		var scheduler = await _schedulerFactory.GetScheduler(cancellation);

		var name = $"{typeof(TWork).Name}_{Guid.NewGuid()}"; ;
		var group = "Default";
		var dataMap = GetJobDataMap(work);

		var jobDetail = BuildJob<TWork>(name, group, dataMap);
		var trigger = BuildTrigger(name, group, firingTime);

		await scheduler.ScheduleJob(jobDetail, trigger, cancellation);
	}

	private static JobDataMap GetJobDataMap<TWork>(TWork work)
	{
		var dataMap = new JobDataMap();
		dataMap.Put("SerializedWork", work.Serialize());

		return dataMap;
	}

	private static IJobDetail BuildJob<TWork>(string name, string group, JobDataMap dataMap)
		where TWork : IWork
	{
		return JobBuilder.Create<QuartzJob<TWork>>()
			.WithIdentity(name, group)
			.UsingJobData(dataMap)
			.Build();
	}

	private static ITrigger BuildTrigger(string name, string group, DateTime firingTime)
	{
		return TriggerBuilder.Create()
			.WithIdentity(name, group)
			.StartAt(firingTime)
			.WithSimpleSchedule(x => x.WithMisfireHandlingInstructionFireNow())
			.Build();
	}
}