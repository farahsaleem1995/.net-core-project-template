using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Application.Shared.Specification.Enums;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFFSpecificationProjector : ISpecificationProjector
{
	public IQueryable<TResult> Project<TEntity, TResult>(
		IQueryable<TEntity> query, ProjectSpecificationBase<TEntity, TResult> specification)
		where TEntity : class
	{
		if (specification.ProjectExpression.Type == ProjectionType.Selection)
		{
			return query.Select(specification.ProjectExpression.Expression);
		}

		throw new InvalidOperationException($"Unsupported projection {specification.ProjectExpression.Type}");
	}
}