using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Infrastructure.Background;
using DotnetCoreTemplate.Infrastructure.Extensions;
using Quartz;

namespace DotnetCoreTemplate.Infrastructure.Services;

public class QuartzQueueProvider : IQueueProvider
{
	private readonly ISchedulerFactory _schedulerFactory;

	public QuartzQueueProvider(ISchedulerFactory schedulerFactory)
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

		var name = typeof(TWork).FullName;
		var group = "Default";
		var dataMap = GetJobDataMap(work);

		var jobDetail = BuildJob(name, group, dataMap);
		var trigger = BuildTrigger(name, group, firingTime);

		await scheduler.ScheduleJob(jobDetail, trigger);
	}

	private static JobDataMap GetJobDataMap<TWork>(TWork work)
	{
		var dataMap = new JobDataMap();
		dataMap.Put("SerializedWork", work.Serialize());
		dataMap.Put("WorkType", $"{typeof(TWork)}, {typeof(TWork).Assembly}");

		return dataMap;
	}

	private static IJobDetail BuildJob(string name, string group, JobDataMap dataMap)
	{
		return JobBuilder.Create<QuartzJob>()
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