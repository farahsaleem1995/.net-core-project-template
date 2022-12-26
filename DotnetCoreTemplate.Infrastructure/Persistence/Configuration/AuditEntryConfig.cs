using DotnetCoreTemplate.Domain.Entities;
using DotnetCoreTemplate.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Configuration;

public class AuditEntryConfig : IEntityTypeConfiguration<AuditEntry>
{
	public void Configure(EntityTypeBuilder<AuditEntry> builder)
	{
		builder.Property(x => x.Id)
			.HasDefaultValueSql("NEWID()");

		builder.Property(x => x.Data)
			.HasConversion(x => x.Serialize(), x => x.Deserialize());
	}
}