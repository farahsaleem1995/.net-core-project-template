using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFRepository<T> : IRepository<T> where T : class
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ISpecificationEvaluator _evaluator;

	public EFRepository(ApplicationDbContext dbContext, ISpecificationEvaluator evaluator)
	{
		_dbContext = dbContext;
		_evaluator = evaluator;
	}

	public async Task AddAsync(T entity, CancellationToken cancellation = default)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		await _dbContext.AddAsync(entity, cancellation);
	}

	public Task RemoveAsync(T entity, CancellationToken cancellation = default)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		_dbContext.Remove(entity);

		return Task.CompletedTask;
	}

	public async Task<T?> FirstOrDefaultAsync(ISpecification<T> specification,
		CancellationToken cancellation = default)
	{
		if (specification == null)
			throw new ArgumentNullException(nameof(specification));

		var query = _evaluator.EvaluateQuery(_dbContext.Set<T>(), specification, true);

		return await query.FirstOrDefaultAsync(cancellation);
	}

	public async Task<TResult?> FirstOrDefaultAsync<TResult>(
		ISpecification<T, TResult> specification, CancellationToken cancellation = default)
	{
		if (specification == null)
			throw new ArgumentNullException(nameof(specification));

		var query = _evaluator.EvaluateQuery(_dbContext.Set<T>(), specification, true);

		return await query.FirstOrDefaultAsync(cancellation);
	}

	public async Task<PaginatedList<T>> PaginateAsync(ISpecification<T> specification,
		CancellationToken cancellation = default)
	{
		if (specification == null)
			throw new ArgumentNullException(nameof(specification));

		var dbSet = _dbContext.Set<T>();
		var listQuery = _evaluator.EvaluateQuery(dbSet, specification, false);
		var countQuery = _evaluator.EvaluateQuery(dbSet, specification, true);

		return await AsPaginatedList(specification, listQuery, countQuery, cancellation);
	}

	public async Task<PaginatedList<TResult>> PaginateAsync<TResult>(
		ISpecification<T, TResult> specification, CancellationToken cancellation = default)
	{
		if (specification == null)
			throw new ArgumentNullException(nameof(specification));

		var dbSet = _dbContext.Set<T>();
		var listQuery = _evaluator.EvaluateQuery(dbSet, specification, false);
		var countQuery = _evaluator.EvaluateQuery(dbSet, specification, true);

		return await AsPaginatedList(specification, listQuery, countQuery, cancellation);
	}

	private static async Task<PaginatedList<TITem>> AsPaginatedList<TITem>(ISpecification<T> specification,
		IQueryable<TITem> listQuery, IQueryable<TITem> countQuery, CancellationToken cancellation)
	{
		var list = await listQuery.ToListAsync(cancellation);
		var count = await countQuery.CountAsync(cancellation);

		return new PaginatedList<TITem>(list, count, specification.PageNumber, specification.PageSize);
	}
}