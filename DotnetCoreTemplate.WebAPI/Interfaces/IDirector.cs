using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.WebAPI.Interfaces;

public interface IDirector
{
	Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellation);

	Task<TResult> Execute<TResult>(IQuery<TResult> query, CancellationToken cancellation);
}