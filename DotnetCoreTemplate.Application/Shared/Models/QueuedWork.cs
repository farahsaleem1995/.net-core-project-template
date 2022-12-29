namespace DotnetCoreTemplate.Application.Shared.Models;

public record QueuedWork(object WorkInstance)
{
	public Type WorkType => WorkInstance.GetType();
}