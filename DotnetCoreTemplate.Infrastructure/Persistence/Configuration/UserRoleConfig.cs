using DotnetCoreTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Configuration;

public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
{
	public void Configure(EntityTypeBuilder<UserRole> builder)
	{
		builder.HasKey(x => new { x.UserId, x.RoleId });

		builder.HasOne(x => x.User)
			.WithMany(x => x.Roles);

		builder.HasOne(x => x.Role)
			.WithMany(x => x.Users);

		builder.ToView("UserRoles");
	}
}