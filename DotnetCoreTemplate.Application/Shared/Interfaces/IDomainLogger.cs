namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IDomainLogger
{
    void LogException(Exception exception);
}