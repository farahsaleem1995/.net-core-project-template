using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.Users.GetUserById;

[Security(SecurityRole.Administrator)]
public record GetUserByIdQuery(string Id) : IQuery<UserDto>;