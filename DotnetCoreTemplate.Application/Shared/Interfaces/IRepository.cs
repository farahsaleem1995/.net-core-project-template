using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
	Task AddAsync(TEntity entity, CancellationToken cancellation = default);

	Task RemoveAsync(TEntity entity, CancellationToken cancellation = default);

	Task<TEntity?> FirstOrDefaultAsync(ISpecification<TEntity> specification,
		CancellationToken cancellation = default);

	Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<TEntity, TResult> specification,
		CancellationToken cancellation = default);

	Task<PaginatedList<TEntity>> PaginateAsync(ISpecification<TEntity> specification,
		CancellationToken cancellation = default);

	Task<PaginatedList<TResult>> PaginateAsync<TResult>(ISpecification<TEntity, TResult> specification,
		CancellationToken cancellation = default);
}