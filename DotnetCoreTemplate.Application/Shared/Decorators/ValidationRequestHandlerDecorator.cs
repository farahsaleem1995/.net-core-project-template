using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class ValidationRequestHandlerDecorator<TRequest, TResult> : IRequestHandler<TRequest, TResult>
	where TRequest : ICommand<TResult>
{
	private readonly IRequestHandler<TRequest, TResult> _decoratee;
	private readonly IValidator<TRequest> _validator;

	public ValidationRequestHandlerDecorator(
		IRequestHandler<TRequest, TResult> decoratee,
		IValidator<TRequest> validator)
	{
		_decoratee = decoratee;
		_validator = validator;
	}

	public async Task<TResult> Handle(TRequest request, CancellationToken cancellation)
	{
		var validationResult = await _validator.Validate(request, cancellation);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}

		return await _decoratee.Handle(request, cancellation);
	}
}