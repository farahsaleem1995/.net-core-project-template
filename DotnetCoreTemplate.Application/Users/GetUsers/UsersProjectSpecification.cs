using DotnetCoreTemplate.Application.Shared.Specifications;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.Users.GetUsers;

public class UsersProjectSpecification : SpecificationBase<User, UsersDto>
{
	public UsersProjectSpecification(int pageNumber, byte pageSize)
	{
		OrderBy(u => u.Id);

		Page(pageNumber);

		Size(pageSize);

		Project(u => new UsersDto(u.Id, u.Email, u.Roles.Select(r => r.Role.Name).ToArray()));
	}
}