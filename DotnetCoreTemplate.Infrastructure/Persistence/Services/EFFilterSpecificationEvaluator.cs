using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFFilterSpecificationEvaluator : ISpecificationEvaluator
{
    public IQueryable<TEntity> Evaluate<TEntity>(IQueryable<TEntity> query, SpecificationBase<TEntity> specification)
        where TEntity : class
    {
        if (specification.FilterExpressions.Count > 0)
        {
            foreach (var filter in specification.FilterExpressions)
                query = query.Where(filter.Expression);
        }

        return query;
    }
}