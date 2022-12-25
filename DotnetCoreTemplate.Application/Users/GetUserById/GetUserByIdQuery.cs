using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.Users.GetUserById;

public record GetUserByIdQuery(string Id) : IQuery<UserDto>;