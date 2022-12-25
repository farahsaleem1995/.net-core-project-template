using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.Auth.Commands.SignIn;

public record SignInCommand(string Email, string Password) : ICommand<Token>;