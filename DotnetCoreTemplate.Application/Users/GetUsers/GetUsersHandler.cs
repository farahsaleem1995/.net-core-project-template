using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.Users.GetUsers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, PaginatedList<UsersDto>>
{
	private readonly IRepository<User> _usersRepository;

	public GetUsersHandler(IRepository<User> usersRepository)
	{
		_usersRepository = usersRepository;
	}

	public Task<PaginatedList<UsersDto>> Handle(GetUsersQuery request, CancellationToken cancellation)
	{
		return _usersRepository.PaginateAsync(
			new UsersProjectSpecification(request.PageNumber, request.PageSize), cancellation);
	}
}