using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFSpecificationEvaluator : ISpecificationEvaluator
{
    private readonly IEnumerable<ISpecificationEvaluator> _evaluators;

    public EFSpecificationEvaluator(IEnumerable<ISpecificationEvaluator> evaluators)
    {
        _evaluators = evaluators;
    }

    public IQueryable<TEntity> Evaluate<TEntity>(IQueryable<TEntity> query, SpecificationBase<TEntity> specification)
        where TEntity : class
    {
        foreach (var evaluator in _evaluators)
        {
            query = evaluator.Evaluate(query, specification);
        }

        return query;
    }
}