namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface ISender
{
	Task<TResult> Send<TResult>(IRequest<TResult> request, CancellationToken cancellation);
}