using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;
using DotnetCoreTemplate.WebAPI.Interfaces;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public record CommandServiceType(Type Type);

public delegate object CommandExecutor(object commandService, object command, CancellationToken cancellation);

public class CommandDirector : ICommandDirector
{
	private readonly Container _container;
	private readonly ILocalCache<Type, CommandServiceType> _serviceTypes;
	private readonly ILocalCache<Type, CommandExecutor> _executionDelegates;

	public CommandDirector(
		Container container,
		ILocalCache<Type, CommandServiceType> serviceTypes,
		ILocalCache<Type, CommandExecutor> executionDelegates)
	{
		_container = container;
		_serviceTypes = serviceTypes;
		_executionDelegates = executionDelegates;
	}

	public async Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellation)
	{
		var commandType = command.GetType();

		var commandServiceType = GetCommandServiceType<TResult>(commandType);

		var commandService = _container.GetInstance(commandServiceType.Type);

		var executor = GetCommandExecutor(commandServiceType.Type);

		var result = executor(commandService, command, cancellation);

		return await (Task<TResult>)result;
	}

	private CommandServiceType GetCommandServiceType<TResult>(Type commandType)
	{
		return _serviceTypes.Get(commandType,
			type => CommandServiceHelper.MakeCommandServiceType(type, typeof(TResult)));
	}

	private CommandExecutor GetCommandExecutor(Type serviceType)
	{
		return _executionDelegates.Get(serviceType, CommandServiceHelper.MakeFastCommandExecutor);
	}
}