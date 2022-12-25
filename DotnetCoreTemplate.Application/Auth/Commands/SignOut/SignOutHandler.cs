using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Auth.Commands.SignOut;

public class SignOutHandler : IRequestHandler<SignOutCommand>
{
	private readonly IUserContext _userContext;
	private readonly ITokenProvider _tokenProvider;

	public SignOutHandler(
		IUserContext userContext,
		ITokenProvider tokenProvider)
	{
		_userContext = userContext;
		_tokenProvider = tokenProvider;
	}

	public async Task<Unit> Handle(SignOutCommand request, CancellationToken cancellation)
	{
		var accessToken = _userContext.AccessToken;

		await _tokenProvider.InvalidateTokenAsync(accessToken, cancellation);

		return Unit.Value;
	}
}