namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IWorkHandler<TWork> where TWork : IWork
{
	Task Handle(TWork work, CancellationToken cancellation);
}