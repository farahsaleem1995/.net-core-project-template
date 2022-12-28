using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Extensions;
using Quartz;

namespace DotnetCoreTemplate.Infrastructure.Background;

public class QueueProvider : IQueueProvider
{
	private readonly ISchedulerFactory _schedulerFactory;

	public QueueProvider(ISchedulerFactory schedulerFactory)
	{
		_schedulerFactory = schedulerFactory;
	}

	public async Task Queue<TWork>(TWork work, DateTime? firingTime = null)
	{
		if (firingTime == null)
		{
			throw new ArgumentNullException(nameof(firingTime));
		}

		await ScheduleJob(work, firingTime.Value);
	}

	private async Task ScheduleJob<TWork>(TWork work, DateTime firingTime)
	{
		var scheduler = await _schedulerFactory.GetScheduler();

		var name = $"{typeof(TWork).Name}_{Guid.NewGuid()}"; ;
		var group = "Default";
		var dataMap = GetJobDataMap(work);

		var jobDetail = BuildJob<TWork>(name, group, dataMap);
		var trigger = BuildTrigger(name, group, firingTime);

		await scheduler.ScheduleJob(jobDetail, trigger);
	}

	private static JobDataMap GetJobDataMap<TWork>(TWork work)
	{
		var dataMap = new JobDataMap();
		dataMap.Put("SerializedWork", work.Serialize());

		return dataMap;
	}

	private static IJobDetail BuildJob<TWork>(string name, string group, JobDataMap dataMap)
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