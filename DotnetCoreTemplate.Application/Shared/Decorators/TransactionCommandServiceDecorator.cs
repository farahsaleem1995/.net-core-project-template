using DotnetCoreTemplate.Application.Shared.Interfaces;
using System.Transactions;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class TransactionCommandServiceDecorator<TCommand, TResult> : ICommandService<TCommand, TResult>
	where TCommand : ICommand<TResult>
{
	private readonly ICommandService<TCommand, TResult> _decoratee;

	public TransactionCommandServiceDecorator(ICommandService<TCommand, TResult> decoratee)
	{
		_decoratee = decoratee;
	}

	public async Task<TResult> Execute(TCommand command, CancellationToken cancellation)
	{
		using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

		var result = await _decoratee.Execute(command, cancellation);

		scope.Complete();

		return result;
	}
}