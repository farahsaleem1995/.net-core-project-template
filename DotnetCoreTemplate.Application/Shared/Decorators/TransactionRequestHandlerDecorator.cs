using DotnetCoreTemplate.Application.Shared.Interfaces;
using System.Transactions;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class TransactionRequestHandlerDecorator<TRequest, TResult> : IRequestHandler<TRequest, TResult>
	where TRequest : ICommand<TResult>
{
	private readonly IRequestHandler<TRequest, TResult> _decoratee;
	private readonly IUnitOfWork _unitOfWork;

	public TransactionRequestHandlerDecorator(
		IRequestHandler<TRequest, TResult> decoratee,
		IUnitOfWork unitOfWork)
	{
		_decoratee = decoratee;
		_unitOfWork = unitOfWork;
	}

	public async Task<TResult> Handle(TRequest request, CancellationToken cancellation)
	{
		using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

		var result = await _decoratee.Handle(request, cancellation);

		await _unitOfWork.DispatchAsync(cancellation);

		scope.Complete();

		return result;
	}
}