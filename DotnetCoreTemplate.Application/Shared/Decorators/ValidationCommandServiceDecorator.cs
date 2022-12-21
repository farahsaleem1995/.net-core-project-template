using DotnetCoreTemplate.Application.Shared.Interfaces;
using ValidationException = DotnetCoreTemplate.Application.Shared.Exceptions.ValidationException;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class ValidationCommandServiceDecorator<TCommand, TResult> : ICommandService<TCommand, TResult>
	where TCommand : ICommand<TResult>
{
	private readonly ICommandService<TCommand, TResult> _decoratee;
	private readonly IDomainValidator<TCommand> _validator;

	public ValidationCommandServiceDecorator(
		ICommandService<TCommand, TResult> decoratee,
		IDomainValidator<TCommand> validator)
	{
		_decoratee = decoratee;
		_validator = validator;
	}

	public async Task<TResult> Execute(TCommand command, CancellationToken cancellation)
	{
		var validationResult = await _validator.Validate(command, cancellation);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}

		return await _decoratee.Execute(command, cancellation);
	}
}