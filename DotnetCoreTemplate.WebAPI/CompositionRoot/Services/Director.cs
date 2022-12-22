using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.WebAPI.CompositionRoot.Helpers;
using DotnetCoreTemplate.WebAPI.Interfaces;
using SimpleInjector;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public record OperationServiceType(Type Type);

public delegate object OperationExecutor(object operationService, object operation, CancellationToken cancellation);

public class Director : IDirector
{
	private readonly Container _container;
	private readonly ILocalCache<Type, OperationServiceType> _serviceTypes;
	private readonly ILocalCache<Type, OperationExecutor> _executionDelegates;

	public Director(
		Container container,
		ILocalCache<Type, OperationServiceType> serviceTypes,
		ILocalCache<Type, OperationExecutor> executionDelegates)
	{
		_container = container;
		_serviceTypes = serviceTypes;
		_executionDelegates = executionDelegates;
	}

	public async Task<TResult> Execute<TResult>(ICommand<TResult> command, CancellationToken cancellation)
	{
		var commandType = command.GetType();
		var commandServiceType = GetCommandServiceType<TResult>(commandType);

		return await GetExecutionResult<TResult>(command, commandServiceType.Type, cancellation);
	}

	private OperationServiceType GetCommandServiceType<TResult>(Type commandType)
	{
		return _serviceTypes.Get(commandType,
			type => OperationServiceHelper.MakeCommandServiceType(type, typeof(TResult)));
	}

	public async Task<TResult> Execute<TResult>(IQuery<TResult> query, CancellationToken cancellation)
	{
		var queryType = query.GetType();
		var queryServiceType = GetQueryServiceType<TResult>(queryType);

		return await GetExecutionResult<TResult>(query, queryServiceType.Type, cancellation);
	}

	private OperationServiceType GetQueryServiceType<TResult>(Type queryType)
	{
		return _serviceTypes.Get(queryType,
			type => OperationServiceHelper.MakeQueryServiceType(type, typeof(TResult)));
	}

	private async Task<TResult> GetExecutionResult<TResult>(
		object operation, Type operationType, CancellationToken cancellation)
	{
		var opertionService = _container.GetInstance(operationType);

		var executor = GetOperationExecutor(operationType);

		var result = executor(opertionService, operation, cancellation);

		return await (Task<TResult>)result;
	}

	private OperationExecutor GetOperationExecutor(Type serviceType)
	{
		return _executionDelegates.Get(serviceType, OperationServiceHelper.MakeFastOperationExecutor);
	}
}