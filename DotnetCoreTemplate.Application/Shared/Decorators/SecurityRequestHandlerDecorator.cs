using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using System.Reflection;

namespace DotnetCoreTemplate.Application.Shared.Decorators;

public class SecurityRequestHandlerDecorator<TRequest, TResult> : IRequestHandler<TRequest, TResult>
	where TRequest : IRequest<TResult>
{
	private readonly IRequestHandler<TRequest, TResult> _decoratee;
	private readonly IUserContext _userContext;

	public SecurityRequestHandlerDecorator(
		IRequestHandler<TRequest, TResult> decoratee,
		IUserContext userContext)
	{
		_decoratee = decoratee;
		_userContext = userContext;
	}

	public async Task<TResult> Handle(TRequest request, CancellationToken cancellation)
	{
		var securityAttribute = typeof(TRequest).GetCustomAttribute<SecurityAttribute>();

		if (securityAttribute != null)
		{
			CheckAuthorization();
			CheckRole(securityAttribute.Role);
		}

		return await _decoratee.Handle(request, cancellation);
	}

	private void CheckAuthorization()
	{
		if (!_userContext.Authorized)
		{
			throw new SecurityException();
		}
	}

	private void CheckRole(SecurityRole requiredRole)
	{
		if (!_userContext.IsInRole(requiredRole))
		{
			throw new SecurityException(SecurityError.AccessDenial);
		}
	}
}