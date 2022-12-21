using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class ExceptionLogCommandServiceDecorator<TCommand, TResult> : ICommandService<TCommand, TResult>
{
    private readonly ICommandService<TCommand, TResult> _decoratee;
    private readonly IDomainLogger _logger;

    public ExceptionLogCommandServiceDecorator(
        ICommandService<TCommand, TResult> decoratee,
        IDomainLogger logger)
    {
        _decoratee = decoratee;
        _logger = logger;
    }

    public async Task<TResult> Execute(TCommand command, CancellationToken cancellation)
    {
        try
        {
            return await _decoratee.Execute(command, cancellation);
        }
        catch (Exception e)
        {
            LogException(e);

            throw;
        }
    }

    private void LogException(Exception e)
    {
        if (e.GetType() != typeof(DomainException))
        {
            _logger.LogException(e);
        }
    }
}