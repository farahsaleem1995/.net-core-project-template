using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.Auth.Commands.RegisterUser;
public record RegisterUserCommand(string Email, string Password) : ICommand;