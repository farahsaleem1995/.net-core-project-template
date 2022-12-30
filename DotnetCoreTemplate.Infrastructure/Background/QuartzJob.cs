using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Extensions;
using Quartz;

namespace DotnetCoreTemplate.Infrastructure.Background;

public class QuartzJob<TWork> : IJob where TWork : IWork
{
	private readonly IWorker<TWork> _worker;

	public QuartzJob(IWorker<TWork> worker)
	{
		_worker = worker;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var work = GetWork(context);
		if (work == null)
		{
			return;
		}

		await _worker.Execute(work, context.CancellationToken);
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