using DotnetCoreTemplate.Application.Shared.Enums;

namespace DotnetCoreTemplate.Application.Shared.Exceptions;

public class SecurityException : DomainException
{
	private const string ErrorMsg = "Attempt to perform unauthorized operation.";

	public SecurityException(SecurityError errorType = SecurityError.Unauthorized)
		: base(ErrorMsg)
	{
		ErrorType = errorType;
	}

	public SecurityException(string message, SecurityError errorType = SecurityError.Unauthorized)
		: base(message)
	{
		ErrorType = errorType;
	}

	public SecurityException(SecurityError errorType, Exception innerException)
		: base(ErrorMsg, innerException)
	{
		ErrorType = errorType;
	}

	public SecurityException(string message, SecurityError errorType, Exception innerException)
		: base(message, innerException)
	{
		ErrorType = errorType;
	}

	public SecurityError ErrorType { get; set; }
}