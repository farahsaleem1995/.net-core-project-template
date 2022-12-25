using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Auth.Commands.SignIn;

public class SignInHandler : IRequestHandler<SignInCommand, Token>
{
	private readonly IUserRetriever _userRetriever;
	private readonly IIdentityProvider _identityProvider;
	private readonly ITokenProvider _tokenProvider;

	public SignInHandler(
		IUserRetriever userRetriever,
		IIdentityProvider identityProvider,
		ITokenProvider tokenProvider)
	{
		_userRetriever = userRetriever;
		_identityProvider = identityProvider;
		_tokenProvider = tokenProvider;
	}

	public async Task<Token> Handle(SignInCommand request, CancellationToken cancellation)
	{
		var user = await RetrieveUser(request, cancellation);

		await SignIn(user, request.Password, cancellation);

		return await GenerateToken(user, cancellation);
	}

	private async Task<User> RetrieveUser(SignInCommand request, CancellationToken cancellation)
	{
		var user = await _userRetriever.GetUserByEmai(request.Email, cancellation);
		if (user == null)
		{
			throw new SecurityException("Password mismatch");
		}

		return user;
	}

	private async Task SignIn(User user, string password, CancellationToken cancellation)
	{
		var result = await _identityProvider.SignInAsync(user.Id, password, cancellation);
		if (!result.Succeeded)
		{
			throw new SecurityException(result.Errors.First());
		}
	}

	private async Task<Token> GenerateToken(User user, CancellationToken cancellation)
	{
		var result = await _tokenProvider.GenerateTokenAsync(user.Id, cancellation);
		if (!result.Succeeded)
		{
			throw new SecurityException(result.Errors.First());
		}

		return result.Value;
	}
}