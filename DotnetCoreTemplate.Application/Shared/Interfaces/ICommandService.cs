using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface ICommandService<TCommand, TResult>
	where TCommand : ICommand<TResult>
{
	Task<TResult> Execute(TCommand command, CancellationToken cancellation);
}

public interface ICommandService<TCommand> : ICommandService<TCommand, Unit>
	where TCommand : ICommand<Unit>
{
}