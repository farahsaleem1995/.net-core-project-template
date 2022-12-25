using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Seeders;

public static class RoleSeeder
{
	public static ModelBuilder SeedRoles(this ModelBuilder builder, params SecurityRole[] roles)
	{
		foreach (var role in roles)
		{
			builder.Entity<ApplicationRole>()
				.HasData(new ApplicationRole
				{
					Id = role.ToString(),
					Name = role.ToString(),
					NormalizedName = role.ToString().ToUpper(),
				});
		}

		return builder;
	}
}