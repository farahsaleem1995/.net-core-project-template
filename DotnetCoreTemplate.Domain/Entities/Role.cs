namespace DotnetCoreTemplate.Domain.Entities;

public class Role
{
	public Role(string id, string name)
	{
		Id = id;
		Name = name;
	}

	public string Id { get; private set; }

	public string Name { get; private set; }

	public IEnumerable<UserRole> Users { get; } = new List<UserRole>();
}