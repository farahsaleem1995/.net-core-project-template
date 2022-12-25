using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Auth.Commands.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, Token>
{
	private readonly ITokenProvider _tokenProvider;

	public RefreshTokenHandler(ITokenProvider tokenProvider)
	{
		_tokenProvider = tokenProvider;
	}

	public async Task<Token> Handle(RefreshTokenCommand request, CancellationToken cancellation)
	{
		var result = await _tokenProvider.RefreshTokenAsync(
			new Token(request.AccessToken, request.RefreshToken), cancellation);

		if (!result.Succeeded)
		{
			throw new SecurityException(result.Errors.First());
		}

		return result.Value;
	}
}