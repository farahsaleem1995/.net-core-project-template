using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.Users.CreateUser;

public record CreateUserCommand(string Email, string Password, SecurityRole Role) : ICommand;