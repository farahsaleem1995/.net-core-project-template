using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Users.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand>
{
	private readonly IIdentityProvider _identityProvider;

	public CreateUserHandler(IIdentityProvider identityProvider)
	{
		_identityProvider = identityProvider;
	}

	public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellation)
	{
		var result = await _identityProvider.RegisterUsertAsync(
			request.Email, request.Password, request.Role, cancellation);

		if (!result.Succeeded)
		{
			throw new DomainException(result.Errors.First());
		}

		return Unit.Value;
	}
}