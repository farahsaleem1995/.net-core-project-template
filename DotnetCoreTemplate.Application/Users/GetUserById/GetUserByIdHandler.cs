using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.Users.GetUserById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
	private readonly IRepository<User> _usersRepository;

	public GetUserByIdHandler(IRepository<User> usersRepository)
	{
		_usersRepository = usersRepository;
	}

	public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellation)
	{
		var user = await _usersRepository.FirstOrDefaultAsync(
			new UserByIdProjectSpecification(request.Id), cancellation);

		return user ?? throw new NotFoundException(typeof(User), request.Id);
	}
}