using DotnetCoreTemplate.Application.Shared.Interfaces;
using System.Transactions;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class TransactionCommandServiceDecorator<TOperation, TResult> : IOperationService<TOperation, TResult>
	where TOperation : ICommand<TResult>
{
	private readonly IOperationService<TOperation, TResult> _decoratee;

	public TransactionCommandServiceDecorator(IOperationService<TOperation, TResult> decoratee)
	{
		_decoratee = decoratee;
	}

	public async Task<TResult> Execute(TOperation command, CancellationToken cancellation)
	{
		using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

		var result = await _decoratee.Execute(command, cancellation);

		scope.Complete();

		return result;
	}
}