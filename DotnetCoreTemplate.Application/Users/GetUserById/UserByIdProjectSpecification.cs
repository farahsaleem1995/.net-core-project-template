using DotnetCoreTemplate.Application.Shared.Specifications;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.Users.GetUserById;

public class UserByIdProjectSpecification : SpecificationBase<User, UserDto>
{
	public UserByIdProjectSpecification(string userId)
	{
		WithFilter(u => u.Id == userId);

		Project(u => new UserDto(u.Id, u.Email, u.Roles.Select(r => r.Role.Name).ToArray()));
	}
}