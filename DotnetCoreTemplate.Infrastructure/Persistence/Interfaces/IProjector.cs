using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Application.Shared.Specification.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

public interface IProjector
{
	IQueryable<TResult> Project<TEntity, TResult>(
		IQueryable<TEntity> query, IProjectSpecification<TEntity, TResult> specification)
		where TEntity : class;
}