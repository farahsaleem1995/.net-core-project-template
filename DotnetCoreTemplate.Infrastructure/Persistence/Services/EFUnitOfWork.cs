using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public EFUnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync(CancellationToken cancellation = default)
    {
        await _dbContext.SaveChangesAsync(cancellation);
    }
}