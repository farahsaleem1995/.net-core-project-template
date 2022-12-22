using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class ExceptionLogCommandServiceDecorator<TOperation, TResult> : IOperationService<TOperation, TResult>
	where TOperation : IOperation<TResult>
{
	private readonly IOperationService<TOperation, TResult> _decoratee;
	private readonly IDomainLogger<TOperation> _logger;

	public ExceptionLogCommandServiceDecorator(
		IOperationService<TOperation, TResult> decoratee,
		IDomainLogger<TOperation> logger)
	{
		_decoratee = decoratee;
		_logger = logger;
	}

	public async Task<TResult> Execute(TOperation command, CancellationToken cancellation)
	{
		try
		{
			return await _decoratee.Execute(command, cancellation);
		}
		catch (Exception e)
		{
			LogException(e);

			throw;
		}
	}

	private void LogException(Exception e)
	{
		if (e.GetType() != typeof(DomainException))
		{
			_logger.LogException(e);
		}
	}
}