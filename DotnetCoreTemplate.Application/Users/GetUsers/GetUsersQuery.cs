using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Users.GetUsers;

public record GetUsersQuery(int PageNumber = 1, byte PageSize = 10)
	: IQuery<PaginatedList<UsersDto>>;