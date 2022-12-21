using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;
using DotnetCoreTemplate.WebAPI.Interfaces;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public record ServiceType(Type Type);
public record ExecutionDelegate(Delegate Delegate);

public class CommandDirector : ICommandDirector
{
	private readonly Container _container;
	private readonly ILocalCache<Type, ServiceType> _serviceTypes;
	private readonly ILocalCache<Type, ExecutionDelegate> _executionDelegates;

	public CommandDirector(
		Container container,
		ILocalCache<Type, ServiceType> serviceTypes,
		ILocalCache<Type, ExecutionDelegate> executionDelegates)
	{
		_container = container;
		_serviceTypes = serviceTypes;
		_executionDelegates = executionDelegates;
	}

	public async Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellation)
	{
		var commandType = command.GetType();

		var serviceType = GetServiceType<TResult>(commandType);

		var serviceObject = _container.GetInstance(serviceType);

		var executeDelegate = GetExecuteDelegate<TResult>(serviceType);

		return await executeDelegate(serviceObject, command, cancellation);
	}

	private Type GetServiceType<TResult>(Type commandType)
	{
		var serviceType = _serviceTypes.Get(commandType, type =>
			new(CommandServiceHelper.MakeGenericType<TResult>(commandType)));

		return serviceType.Type;
	}

	private Func<object, object, CancellationToken, Task<TResult>> GetExecuteDelegate<TResult>(Type serviceType)
	{
		var executionDelegate = _executionDelegates.Get(serviceType, type =>
			new(CommandServiceHelper.MakeFastExecutionDelegate<TResult>(type)));

		return (Func<object, object, CancellationToken, Task<TResult>>)executionDelegate.Delegate;
	}
}