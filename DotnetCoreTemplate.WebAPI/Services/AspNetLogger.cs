using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.WebAPI.Services;

public class AspNetLogger<T> : IDomainLogger<T>
{
	private readonly ILogger<T> _logger;

	public AspNetLogger(ILogger<T> logger)
	{
		_logger = logger;
	}

	public void LogException(Exception exception)
	{
		_logger.LogError(exception, "An error occurred while executing '{Type}'", typeof(T));
	}
}