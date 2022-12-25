using DotnetCoreTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Configuration;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
	public void Configure(EntityTypeBuilder<Role> builder)
	{
		builder.HasMany(x => x.Users)
			.WithOne(x => x.Role);

		builder.ToView("Roles");
	}
}