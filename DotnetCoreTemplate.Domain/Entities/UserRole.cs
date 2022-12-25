namespace DotnetCoreTemplate.Domain.Entities;

public class UserRole
{
	public UserRole(string userId, string roleId)
	{
		UserId = userId;
		RoleId = roleId;
	}

	public string UserId { get; private set; }

	public string RoleId { get; private set; }

	public User User { get; private set; } = null!;

	public Role Role { get; private set; } = null!;
}