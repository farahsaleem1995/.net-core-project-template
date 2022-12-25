using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Adapters;

public class UnitCommandAdapter<TRequest> : IRequestHandler<TRequest>
	where TRequest : ICommand
{
	private readonly Container _container;

	public UnitCommandAdapter(Container container)
	{
		_container = container;
	}

	public async Task<Unit> Handle(TRequest request, CancellationToken cancellation)
	{
		var handler = _container.GetInstance<IRequestHandler<TRequest, Unit>>();

		return await handler.Handle(request, cancellation);
	}
}