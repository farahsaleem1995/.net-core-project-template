using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class ExceptionLogRequestHandlerDecorator<TRequest, TResult> : IRequestHandler<TRequest, TResult>
	where TRequest : IRequest<TResult>
{
	private readonly IRequestHandler<TRequest, TResult> _decoratee;
	private readonly ILogger<TRequest> _logger;

	public ExceptionLogRequestHandlerDecorator(
		IRequestHandler<TRequest, TResult> decoratee,
		ILogger<TRequest> logger)
	{
		_decoratee = decoratee;
		_logger = logger;
	}

	public async Task<TResult> Handle(TRequest request, CancellationToken cancellation)
	{
		try
		{
			return await _decoratee.Handle(request, cancellation);
		}
		catch (Exception e)
		{
			LogException(e);

			throw;
		}
	}

	private void LogException(Exception e)
	{
		if (!typeof(DomainException).IsAssignableFrom(e.GetType()))
		{
			_logger.LogException(e);
		}
	}
}