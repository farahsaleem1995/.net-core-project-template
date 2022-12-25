namespace DotnetCoreTemplate.Infrastructure.Identity;

public class RefreshToken
{
	public RefreshToken(string jti, string token, string userId, DateTime expiredDate)
	{
		Jti = jti;
		Token = token;
		UserId = userId;
		ExpiredDate = expiredDate;
	}

	public string Jti { get; set; }

	public string Token { get; set; }

	public string UserId { get; set; }

	public DateTime ExpiredDate { get; set; }

	public bool Used { get; set; }

	public bool Invalidated { get; set; }
}