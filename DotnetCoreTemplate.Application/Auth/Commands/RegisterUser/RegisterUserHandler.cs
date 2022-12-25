using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Auth.Commands.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand>
{
	private readonly IIdentityProvider _identityProvider;

	public RegisterUserHandler(IIdentityProvider identityProvider)
	{
		_identityProvider = identityProvider;
	}

	public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellation)
	{
		var result = await _identityProvider.RegisterUsertAsync(
			request.Email, request.Password, SecurityRole.Individual, cancellation);

		if (!result.Succeeded)
		{
			throw new DomainException(result.Errors.First());
		}

		return Unit.Value;
	}
}