using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISpecificationEvaluator _evaluator;

    public EFRepository(ApplicationDbContext dbContext, ISpecificationEvaluator evaluator)
    {
        _dbContext = dbContext;
        _evaluator = evaluator;
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellation = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _dbContext.AddAsync(entity, cancellation);
    }

    public Task RemoveAsync(TEntity entity, CancellationToken cancellation = default)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbContext.Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(SpecificationBase<TEntity> specification,
        CancellationToken cancellation = default)
    {
        if (specification == null)
            throw new ArgumentNullException(nameof(specification));

        var query = _evaluator.Evaluate(_dbContext.Set<TEntity>(), specification);

        return await query.FirstOrDefaultAsync(cancellation);
    }

    public async Task<IEnumerable<TEntity>> ListAsync(SpecificationBase<TEntity> specification,
        CancellationToken cancellation = default)
    {
        if (specification == null)
            throw new ArgumentNullException(nameof(specification));

        var query = _evaluator.Evaluate(_dbContext.Set<TEntity>(), specification);

        return await query.ToListAsync(cancellation);
    }
}