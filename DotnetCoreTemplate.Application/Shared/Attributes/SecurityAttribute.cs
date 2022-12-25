using DotnetCoreTemplate.Application.Shared.Enums;

namespace DotnetCoreTemplate.Application.Shared.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SecurityAttribute : Attribute
{
    public SecurityAttribute(SecurityRole role)
    {
        Role = role;
    }

    public SecurityRole Role { get; }
}