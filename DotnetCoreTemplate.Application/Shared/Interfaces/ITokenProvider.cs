using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface ITokenProvider
{
	Task<Result<Token>> GenerateTokenAsync(string userId, CancellationToken cancellation = default);

	Task<Result<Token>> RefreshTokenAsync(Token token, CancellationToken cancellation = default);

	Task<Result> InvalidateTokenAsync(string token, CancellationToken cancellation = default);
}