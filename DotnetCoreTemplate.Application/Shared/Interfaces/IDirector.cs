namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IDirector
{
	Task<TResult> Send<TResult>(IRequest<TResult> request, CancellationToken cancellation);
}