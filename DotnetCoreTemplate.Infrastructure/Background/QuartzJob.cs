using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Infrastructure.Extensions;
using Quartz;

namespace DotnetCoreTemplate.Infrastructure.Background;

public class QuartzJob<TWork> : IJob where TWork : IWork
{
	private readonly IWorkHandler<TWork> _handler;

	public QuartzJob(IWorkHandler<TWork> handler)
	{
		_handler = handler;
	}

	public async Task Execute(IJobExecutionContext context)
	{
		var work = GetWork(context);
		if (work == null)
		{
			return;
		}

		await _handler.Handle(work, context.CancellationToken);
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