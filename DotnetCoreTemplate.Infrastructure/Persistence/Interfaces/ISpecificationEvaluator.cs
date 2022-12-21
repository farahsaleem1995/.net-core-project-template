using DotnetCoreTemplate.Application.Shared.Specification;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

public interface ISpecificationEvaluator
{
    IQueryable<TEntity> Evaluate<TEntity>(IQueryable<TEntity> query, SpecificationBase<TEntity> specification)
        where TEntity : class;
}