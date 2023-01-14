using AutoMapper;
using AutoMapper.QueryableExtensions;
using DotnetCoreTemplate.Application.Shared.Specification.Enums;
using DotnetCoreTemplate.Application.Shared.Specification.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFProjector : IProjector
{
	private readonly IMapper _mapper;

	public EFProjector(IMapper mapper)
	{
		_mapper = mapper;
	}

	public IQueryable<TResult> Project<TEntity, TResult>(
		IQueryable<TEntity> query, IProjectSpecification<TEntity, TResult> specification)
		where TEntity : class
	{
		return specification.ProjectExpression.Type switch
		{
			ProjectionType.Selection => Select(query, specification),
			ProjectionType.AutoMapper => query.ProjectTo<TResult>(_mapper.ConfigurationProvider),
			_ => throw new InvalidOperationException(
				$"Unsupported projection '{specification.ProjectExpression.Type}'."),
		};
	}

	public static IQueryable<TResult> Select<TEntity, TResult>(
		IQueryable<TEntity> query, IProjectSpecification<TEntity, TResult> specification)
		where TEntity : class
	{
		if (specification.ProjectExpression.Expression == null)
		{
			throw new InvalidOperationException(
				$"Projection expression cannot be null when type is set to '{ProjectionType.Selection}'.");
		}

		return query.Select(specification.ProjectExpression.Expression);
	}
}