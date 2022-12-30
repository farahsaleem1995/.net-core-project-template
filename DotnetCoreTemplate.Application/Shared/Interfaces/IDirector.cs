using DotnetCoreTemplate.Domain.Shared;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IDirector
{
	Task<TResult> SendRequest<TResult>(IRequest<TResult> request, CancellationToken cancellation);

	Task DispatchEvent(DomainEvent domainEvent, CancellationToken cancellation);

	Task ExecuteWork(IWork work, CancellationToken cancellation);
}