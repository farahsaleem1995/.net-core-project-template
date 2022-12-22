namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IQueryService<TQuery, TResult>
	where TQuery : IQuery<TResult>
{
	Task<TResult> Execute(TQuery query, CancellationToken cancellation);
}