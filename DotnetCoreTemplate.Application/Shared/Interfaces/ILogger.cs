namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface ILogger<T>
{
	void LogException(Exception exception);

	void LogMessage(string message);
}