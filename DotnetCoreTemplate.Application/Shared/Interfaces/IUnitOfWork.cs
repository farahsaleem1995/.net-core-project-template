namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IUnitOfWork
{
    Task SaveAsync(CancellationToken cancellation = default);
}