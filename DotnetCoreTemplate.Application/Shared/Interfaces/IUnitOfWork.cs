namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IUnitOfWork
{
	Task SaveAsync(CancellationToken cancellation = default);

	Task DispatchAsync(CancellationToken cancellation = default);
}