namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IAuditTrailAppender
{
	public Task Append<T>(T operation, string userId, CancellationToken cancellation = default);
}