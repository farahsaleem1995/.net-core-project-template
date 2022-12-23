using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IRequestHandler<TRequest, TResult>
	where TRequest : IRequest<TResult>
{
	Task<TResult> Handle(TRequest request, CancellationToken cancellation);
}

public interface IRequestHandler<TRequest> : IRequestHandler<TRequest, Unit>
	where TRequest : IRequest<Unit>
{
}