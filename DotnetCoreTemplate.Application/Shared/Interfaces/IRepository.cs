﻿using DotnetCoreTemplate.Application.Shared.Specification;

namespace DotnetCoreTemplate.Application.Shared.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellation = default);

    Task RemoveAsync(TEntity entity, CancellationToken cancellation = default);

    Task<TEntity?> FirstOrDefaultAsync(SpecificationBase<TEntity> specification,
        CancellationToken cancellation = default);

    Task<IEnumerable<TEntity>> ListAsync(SpecificationBase<TEntity> specification,
        CancellationToken cancellation = default);
}