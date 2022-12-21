using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Application.Shared.Specification.Enums;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFOrderSpecificationEvaluator : ISpecificationEvaluator
{
    public IQueryable<TEntity> Evaluate<TEntity>(IQueryable<TEntity> query, SpecificationBase<TEntity> specification)
        where TEntity : class
    {
        if (specification.OrderExpressions.Count > 0)
        {
            foreach (var order in specification.OrderExpressions)
            {
                if (order.Direction == OrderDirection.Ascending)
                {
                    if (query is IOrderedQueryable<TEntity> orderedQuery)
                        query = orderedQuery.ThenBy(order.Expression);
                    else
                        query = query.OrderBy(order.Expression);
                }
                else if (order.Direction == OrderDirection.Descending)
                {
                    if (query is IOrderedQueryable<TEntity> orderedQuery)
                        query = orderedQuery.ThenByDescending(order.Expression);
                    else
                        query = query.OrderByDescending(order.Expression);
                }
            }
        }

        return query;
    }
}