namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IDomainLogger<T>
{
	void LogException(Exception exception);
}