using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.Shared.Specification;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
	Task AddAsync(TEntity entity, CancellationToken cancellation = default);

	Task RemoveAsync(TEntity entity, CancellationToken cancellation = default);

	Task<TEntity?> FirstOrDefaultAsync(SpecificationBase<TEntity> specification,
		CancellationToken cancellation = default);

	Task<TResult?> FirstOrDefaultAsync<TResult>(
		ProjectSpecificationBase<TEntity, TResult> specification, CancellationToken cancellation = default);

	Task<PaginatedList<TEntity>> PaginateAsync(SpecificationBase<TEntity> specification,
		CancellationToken cancellation = default);

	Task<PaginatedList<TResult>> PaginateAsync<TResult>(
		ProjectSpecificationBase<TEntity, TResult> specification, CancellationToken cancellation = default);
}