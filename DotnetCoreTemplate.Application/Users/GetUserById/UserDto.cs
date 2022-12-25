namespace DotnetCoreTemplate.Application.Users.GetUserById;

public record UserDto(string Id, string Email, string[] Roles);