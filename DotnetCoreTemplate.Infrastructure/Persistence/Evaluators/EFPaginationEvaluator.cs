using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Evaluators;

public class EFPaginationEvaluator : IEvaluator
{
	public bool IsCriteriaEvaluator => false;

	public IQueryable<T> EvaluateQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		var skip = specification.PageSize * (specification.PageNumber - 1);
		var take = specification.PageSize;

		return query.Skip(skip).Take(take);
	}
}