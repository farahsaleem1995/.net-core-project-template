using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Composites;

public class CompositeSpecificationEvaluator : IEvaluator
{
	private readonly IEnumerable<IEvaluator> _evaluators;

	public CompositeSpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
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