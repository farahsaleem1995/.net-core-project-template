using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IDomainValidator<T>
{
	Task<ValidationResult> Validate(T instance, CancellationToken cancellation);
}