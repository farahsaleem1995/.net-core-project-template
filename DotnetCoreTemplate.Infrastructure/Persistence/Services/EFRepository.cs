using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IEvaluator _evaluator;
	private readonly IProjector _projector;
	private readonly IPaginator _paginator;

	public EFRepository(
		ApplicationDbContext dbContext,
		IEvaluator evaluator,
		IProjector projector,
		IPaginator paginator)
	{
		_dbContext = dbContext;
		_evaluator = evaluator;
		_projector = projector;
		_paginator = paginator;
	}

	public async Task AddAsync(TEntity entity, CancellationToken cancellation = default)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		await _dbContext.AddAsync(entity, cancellation);
	}

	public Task RemoveAsync(TEntity entity, CancellationToken cancellation = default)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		_dbContext.Remove(entity);

		return Task.CompletedTask;
	}

	public async Task<TEntity?> FirstOrDefaultAsync(SpecificationBase<TEntity> specification,
		CancellationToken cancellation = default)
	{
		if (specification == null)
			throw new ArgumentNullException(nameof(specification));

		var query = _evaluator.Evaluate(_dbContext.Set<TEntity>(), specification);

		return await query.FirstOrDefaultAsync(cancellation);
	}

	public async Task<TResult?> FirstOrDefaultAsync<TResult>(
		ProjectSpecificationBase<TEntity, TResult> specification, CancellationToken cancellation = default)
	{
		if (specification == null)
			throw new ArgumentNullException(nameof(specification));

		var query = _evaluator.Evaluate(_dbContext.Set<TEntity>(), specification);
		var resultQuery = _projector.Project(query, specification);

		return await resultQuery.FirstOrDefaultAsync(cancellation);
	}

	public async Task<PaginatedList<TEntity>> PaginateAsync(SpecificationBase<TEntity> specification,
		CancellationToken cancellation = default)
	{
		if (specification == null)
			throw new ArgumentNullException(nameof(specification));

		var query = _evaluator.Evaluate(_dbContext.Set<TEntity>(), specification);

		return await _paginator.Paginate(query, specification, cancellation);
	}

	public async Task<PaginatedList<TResult>> PaginateAsync<TResult>(
		ProjectSpecificationBase<TEntity, TResult> specification, CancellationToken cancellation = default)
	{
		if (specification == null)
			throw new ArgumentNullException(nameof(specification));

		var query = _evaluator.Evaluate(_dbContext.Set<TEntity>(), specification);
		var resultQuery = _projector.Project(query, specification);

		return await _paginator.Paginate(resultQuery, specification, cancellation);
	}
}