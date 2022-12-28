using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Extensions;
using DotnetCoreTemplate.Infrastructure.Interfaces;
using Quartz;

namespace DotnetCoreTemplate.Infrastructure.Background;

public class QuartzJob : IJob
{
	private readonly IWorkExecutor _executor;

	public QuartzJob(IWorkExecutor executor)
	{
		_executor = executor;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var workType = GetWorkType(context);
		if (workType == null)
		{
			return;
		}

		var work = GetWork(context, workType);
		if (work == null)
		{
			return;
		}

		await _executor.Execute(work, context.CancellationToken);
	}

	private static Type? GetWorkType(IJobExecutionContext context)
	{
		var workTypeName = context.MergedJobDataMap.GetString("WorkType");
		if (string.IsNullOrEmpty(workTypeName))
		{
			return null;
		}

		var workType = Type.GetType(workTypeName);
		if (workType == null)
		{
			return null;
		}

		return workType;
	}

	private static object? GetWork(IJobExecutionContext context, Type workType)
	{
		var serializedWork = context.MergedJobDataMap.GetString("SerializedWork");
		if (serializedWork == null)
		{
			return null;
		}

		return serializedWork.Deserialize(workType);
	}
}