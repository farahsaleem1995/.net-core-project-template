using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFFilterEvaluator : IEvaluator
{
	public IQueryable<TEntity> Evaluate<TEntity>(IQueryable<TEntity> query, SpecificationBase<TEntity> specification)
		where TEntity : class
	{
		foreach (var filter in specification.FilterExpressions)
			query = query.Where(filter.Expression);

		return query;
	}
}