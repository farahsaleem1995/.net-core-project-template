using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using System.Reflection;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class SecurityCommandServiceDecorator<TOperation, TResult> : IOperationService<TOperation, TResult>
	where TOperation : IOperation<TResult>
{
	private readonly IOperationService<TOperation, TResult> _decoratee;
	private readonly IUserContext _userContext;

	public SecurityCommandServiceDecorator(
		IOperationService<TOperation, TResult> decoratee,
		IUserContext userContext)
	{
		_decoratee = decoratee;
		_userContext = userContext;
	}

	public async Task<TResult> Execute(TOperation command, CancellationToken cancellation)
	{
		var securityAttribute = typeof(TOperation).GetCustomAttribute<SecurityAttribute>();

		if (securityAttribute != null)
		{
			CheckAuthorization();
			CheckRole(securityAttribute.Role);
		}

		return await _decoratee.Execute(command, cancellation);
	}

	private void CheckAuthorization()
	{
		if (_userContext.Authorized)
		{
			throw new SecurityException();
		}
	}

	private void CheckRole(UserRole requiredRole)
	{
		if (_userContext.IsInRole(requiredRole))
		{
			throw new SecurityException(SecurityError.AccessDenial);
		}
	}
}