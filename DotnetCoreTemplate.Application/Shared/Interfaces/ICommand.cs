using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface ICommand<TResult> : IRequest<TResult>
{
}

public interface ICommand : ICommand<Unit>
{
}