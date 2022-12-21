using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Adapters;

public class UnitCommandAdapter<TCommand> : ICommandService<TCommand>
	where TCommand : ICommand
{
	private readonly Container _container;

	public UnitCommandAdapter(Container container)
	{
		_container = container;
	}

	public async Task<Unit> Execute(TCommand command, CancellationToken cancellation)
	{
		var resolvedCommand = _container.GetInstance<ICommandService<TCommand, Unit>>();

		return await resolvedCommand.Execute(command, cancellation);
	}
}