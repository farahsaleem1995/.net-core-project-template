namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IQueryService<TCommand, TResult>
{
    Task<TResult> Execute(TCommand command, CancellationToken cancellation);
}