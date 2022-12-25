namespace DotnetCoreTemplate.Infrastructure.Identity;

public class TokenConfig
{
	public string Key { get; set; } = string.Empty;

	public TimeSpan AccessLifetime { get; set; }

	public TimeSpan RefreshLifetime { get; set; }

	public string Issuer { get; set; } = string.Empty;

	public bool ValidateIssuer { get; set; }

	public string Audience { get; set; } = string.Empty;

	public bool ValidateAudience { get; set; }
}