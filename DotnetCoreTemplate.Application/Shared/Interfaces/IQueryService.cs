namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IQueryService<TQuery, TResult> : IOperationService<TQuery, TResult>
	where TQuery : IQuery<TResult>
{
}