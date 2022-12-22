namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IDirector
{
	Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellation);

	Task<TResult> Execute<TResult>(IQuery<TResult> query, CancellationToken cancellation);
}