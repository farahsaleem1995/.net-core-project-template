using DotnetCoreTemplate.Application.Shared.Enums;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IUserContext
{
    bool Authorized { get; }

    string UserId { get; }

    bool IsInRole(UserRole role);
}