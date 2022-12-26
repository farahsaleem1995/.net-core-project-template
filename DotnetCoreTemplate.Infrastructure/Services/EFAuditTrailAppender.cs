using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Entities;
using DotnetCoreTemplate.Infrastructure.Persistence;

namespace DotnetCoreTemplate.Infrastructure.Services;

public class EFAuditTrailAppender : IAuditTrailAppender
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ITimeProvider _timeProvider;

	public EFAuditTrailAppender(
		ApplicationDbContext dbContext,
		ITimeProvider timeProvider)
	{
		_dbContext = dbContext;
		_timeProvider = timeProvider;
	}

	public async Task Append<T>(T operation, string userId, CancellationToken cancellation = default)
	{
		if (operation == null)
		{
			throw new ArgumentNullException(nameof(operation));
		}

		if (string.IsNullOrEmpty(userId))
		{
			throw new ArgumentNullException(nameof(userId));
		}

		await AppendEntry(operation, userId, cancellation);
	}

	private async Task AppendEntry(object operation, string userId, CancellationToken cancellation)
	{
		var operationName = operation.GetType().Name;

		var entry = new AuditEntry(userId, operationName, _timeProvider.Now)
		{
			Data = operation
		};

		await _dbContext.AuditEntries.AddAsync(entry, cancellation);

		await _dbContext.SaveChangesAsync(cancellation);
	}
}