namespace DotnetCoreTemplate.Domain.Entities;

public class User
{
	public User(string id, string email)
	{
		Id = id;
		Email = email;
	}

	public string Id { get; private set; }

	public string Email { get; private set; }

	public IEnumerable<UserRole> Roles { get; set; } = new List<UserRole>();
}