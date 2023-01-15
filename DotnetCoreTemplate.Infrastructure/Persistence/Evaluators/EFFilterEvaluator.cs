using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Evaluators;

public class EFFilterEvaluator : IEvaluator
{
	public bool IsCriteriaEvaluator => true;

	public IQueryable<T> EvaluateQuery<T>(IQueryable<T> query, ISpecification<T> specification)
		where T : class
	{
		foreach (var filter in specification.FilterExpressions)
		{
			query = query.Where(filter.Expression);
		}

		return query;
	}
}