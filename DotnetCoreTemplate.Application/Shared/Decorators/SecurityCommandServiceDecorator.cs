using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using System.Reflection;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class SecurityCommandServiceDecorator<TCommand, TResult> : ICommandService<TCommand, TResult>
	where TCommand : ICommand<TResult>
{
	private readonly ICommandService<TCommand, TResult> _decoratee;
	private readonly IUserContext _userContext;

	public SecurityCommandServiceDecorator(
		ICommandService<TCommand, TResult> decoratee,
		IUserContext userContext)
	{
		_decoratee = decoratee;
		_userContext = userContext;
	}

	public async Task<TResult> Execute(TCommand command, CancellationToken cancellation)
	{
		var securityAttribute = typeof(TCommand).GetCustomAttribute<SecurityAttribute>();

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