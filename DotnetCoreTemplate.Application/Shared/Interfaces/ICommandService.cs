using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface ICommandService<TCommand, TResult> : IOperationService<TCommand, TResult>
	where TCommand : ICommand<TResult>
{
}

public interface ICommandService<TCommand> : ICommandService<TCommand, Unit>
	where TCommand : ICommand<Unit>
{
}