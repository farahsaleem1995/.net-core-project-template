using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IRequest<TResult>
{
}

public interface IRequest : IRequest<Unit>
{
}