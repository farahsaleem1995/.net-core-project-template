using DotnetCoreTemplate.Application.Shared.Specification;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

public interface ISpecificationProjector
{
	IQueryable<TResult> Project<TEntity, TResult>(
		IQueryable<TEntity> query, ProjectSpecificationBase<TEntity, TResult> specification)
		where TEntity : class;
}