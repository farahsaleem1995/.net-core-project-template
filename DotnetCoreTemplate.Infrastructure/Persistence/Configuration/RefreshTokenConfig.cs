using DotnetCoreTemplate.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Configuration;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
{
	public void Configure(EntityTypeBuilder<RefreshToken> builder)
	{
		builder.HasKey(t => t.Token);

		builder.HasIndex(t => t.UserId);

		builder.HasIndex(t => t.Jti);
	}
}