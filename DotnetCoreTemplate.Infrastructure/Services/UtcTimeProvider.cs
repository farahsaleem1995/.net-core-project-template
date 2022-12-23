using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Services;

public class UtcTimeProvider : ITimeProvider
{
	public DateTime Now => DateTime.UtcNow;
}