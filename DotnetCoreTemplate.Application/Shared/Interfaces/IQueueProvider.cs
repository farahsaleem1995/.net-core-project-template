using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IQueueProvider
{
	Task Queue<TWork>(TWork work, DateTime? firingTime = null);
}