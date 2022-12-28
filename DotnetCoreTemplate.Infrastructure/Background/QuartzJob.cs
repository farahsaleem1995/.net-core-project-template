using DotnetCoreTemplate.Infrastructure.Extensions;
using Quartz;

namespace DotnetCoreTemplate.Infrastructure.Background;

public class QuartzJob<TWork> : IJob
{
	private readonly IWorkExecutor _executor;

	public QuartzJob(IWorkExecutor executor)
	{
		_executor = executor;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var work = GetWork(context);
		if (work == null)
		{
			return;
		}

		await _executor.Execute(work, context.CancellationToken);
	}

	private static TWork? GetWork(IJobExecutionContext context)
	{
		var serializedWork = context.MergedJobDataMap.GetString("SerializedWork");
		if (serializedWork == null)
		{
			return default;
		}

		return serializedWork.Deserialize<TWork>();
	}
}