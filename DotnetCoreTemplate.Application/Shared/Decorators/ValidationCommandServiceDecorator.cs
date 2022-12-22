using DotnetCoreTemplate.Application.Shared.Interfaces;
using ValidationException = DotnetCoreTemplate.Application.Shared.Exceptions.ValidationException;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class ValidationCommandServiceDecorator<TOperation, TResult> : IOperationService<TOperation, TResult>
	where TOperation : ICommand<TResult>
{
	private readonly IOperationService<TOperation, TResult> _decoratee;
	private readonly IDomainValidator<TOperation> _validator;

	public ValidationCommandServiceDecorator(
		IOperationService<TOperation, TResult> decoratee,
		IDomainValidator<TOperation> validator)
	{
		_decoratee = decoratee;
		_validator = validator;
	}

	public async Task<TResult> Execute(TOperation command, CancellationToken cancellation)
	{
		var validationResult = await _validator.Validate(command, cancellation);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}

		return await _decoratee.Execute(command, cancellation);
	}
}