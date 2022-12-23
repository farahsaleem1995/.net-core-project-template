using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.WebAPI.Services;

public class AspNetLogger<T> : Application.Shared.Interfaces.ILogger<T>
{
	private readonly Microsoft.Extensions.Logging.ILogger<T> _logger;

	public AspNetLogger(Microsoft.Extensions.Logging.ILogger<T> logger)
	{
		_logger = logger;
	}

	public void LogException(Exception exception)
	{
		_logger.LogError(exception, "An error occurred while executing '{Type}'", typeof(T).Name);
	}

	public void LogMessage(string message)
	{
		var obj = new
		{
			Source = typeof(T).Name,
			Message = message
		};

		_logger.LogInformation("Domain Message: {MessageObj}", obj);
	}
}