using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.WebAPI.Interfaces;

public interface ICommandDirector
{
	Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellation);
}