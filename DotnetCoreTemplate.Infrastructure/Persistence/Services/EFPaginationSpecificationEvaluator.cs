using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFPaginationSpecificationEvaluator : ISpecificationEvaluator
{
    public IQueryable<TEntity> Evaluate<TEntity>(IQueryable<TEntity> query, SpecificationBase<TEntity> specification)
        where TEntity : class
    {
        var skip = specification.PageSize * (specification.PageNumber - 1);
        var take = specification.PageSize;

        return query.Skip(skip).Take(take);
    }
}