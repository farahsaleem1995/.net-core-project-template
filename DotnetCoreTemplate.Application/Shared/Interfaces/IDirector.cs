namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IDirector
{
	Task<TResult> Execute<TResult>(IOperation<TResult> command, CancellationToken cancellation);
}