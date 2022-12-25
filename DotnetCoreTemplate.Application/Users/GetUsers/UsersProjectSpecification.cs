using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.Users.GetUsers;

public class UsersProjectSpecification : ProjectSpecificationBase<User, UsersDto>
{
	public UsersProjectSpecification(int pageNumber, byte pageSize)
	{
		OrderBy(u => u.Id)
			.Paginate(pageNumber, pageSize);

		Project(u => new UsersDto(u.Id, u.Email, u.Roles.Select(r => r.Role.Name).ToArray()));
	}
}