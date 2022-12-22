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