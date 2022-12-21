namespace DotnetCoreTemplate.Application.Shared.Exceptions;

public class ValidationException : DomainException
{
	private const string ErrorMsg = "One or more validation failures have occurred.";

	public ValidationException()
		: base(ErrorMsg)
	{
		Errors = new Dictionary<string, List<string>>();
	}

	public ValidationException(IDictionary<string, List<string>> errors)
		: this()
	{
		Errors = errors;
	}

	public IDictionary<string, List<string>> Errors { get; } = new Dictionary<string, List<string>>();
}