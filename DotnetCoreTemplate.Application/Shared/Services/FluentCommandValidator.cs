using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using FluentValidation;

namespace DotnetCoreTemplate.Application.Shared.Services;

public class FluentCommandValidator<T> : IDomainValidator<T>
{
	private readonly IEnumerable<IValidator<T>> _validators;

	public FluentCommandValidator(IEnumerable<IValidator<T>> validators)
	{
		_validators = validators;
	}

	public async Task<ValidationResult> Validate(T instance, CancellationToken cancellation)
	{
		var result = new ValidationResult();

		if (_validators.Any())
		{
			var context = new ValidationContext<T>(instance);
			var validationResult =
				await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellation)));

			var failures = validationResult.SelectMany(v => v.Errors).Where(f => f != null).ToList();

			foreach (var failure in failures)
			{
				result.AddError(failure.PropertyName, failure.ErrorMessage);
			}
		}

		return result;
	}
}