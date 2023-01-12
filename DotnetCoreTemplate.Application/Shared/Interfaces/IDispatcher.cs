using DotnetCoreTemplate.Domain.Shared;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IDispatcher
{
	Task Dispatch(DomainEvent domainEvent, CancellationToken cancellation);
}