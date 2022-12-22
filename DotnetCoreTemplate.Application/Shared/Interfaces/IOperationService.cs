namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IOperationService<TOperation, TResult>
	where TOperation : IOperation<TResult>
{
	Task<TResult> Execute(TOperation query, CancellationToken cancellation);
}