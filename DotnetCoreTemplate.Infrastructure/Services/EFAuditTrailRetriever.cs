using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Entities;
using DotnetCoreTemplate.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Services;

public class EFAuditTrailRetriever : IAuditTrailRetriever
{
	private readonly ApplicationDbContext _dbContext;

	public EFAuditTrailRetriever(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<IReadOnlyList<AuditEntry>> Get(int limit)
	{
		return await _dbContext.AuditEntries
			.OrderByDescending(e => e.ExecutedDate)
			.Take(limit)
			.ToListAsync();
	}
}