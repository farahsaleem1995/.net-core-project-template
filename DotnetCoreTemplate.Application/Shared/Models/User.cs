using DotnetCoreTemplate.Application.Shared.Enums;

namespace DotnetCoreTemplate.Application.Shared.Models;

public record User(string Id, string Email, UserRole Role);