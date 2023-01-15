using DotnetCoreTemplate.Application.Shared.Specifications.Enums;
using DotnetCoreTemplate.Application.Shared.Specifications.Expressions;
using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Evaluators;

public class EFIncludeEvaluator : IEvaluator
{
	private const string MissingExpressionErrorMsg = "'{0}' cannot be empty when include type is set to '{1}'";

	public bool IsCriteriaEvaluator => false;

	public IQueryable<TEntity> EvaluateQuery<TEntity>(IQueryable<TEntity> query, ISpecification<TEntity> specification)
		where TEntity : class
	{
		foreach (var include in specification.IncludeExpressions)
		{
			query = Include(query, include);
		}

		return query;
	}

	private static IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> query, IncludeExpression<TEntity> include)
		where TEntity : class
	{
		return include.Type switch
		{
			IncludeType.String => IncludeString(query, include),
			IncludeType.LINQ => IncludeLinq(query, include),
			_ => throw new NotImplementedException(),
		};
	}

	private static IQueryable<TEntity> IncludeLinq<TEntity>(IQueryable<TEntity> query,
		IncludeExpression<TEntity> include) where TEntity : class
	{
		if (include.LinqExpression == null)
			throw new ArgumentNullException(
				string.Format(MissingExpressionErrorMsg, include.LinqExpression, IncludeType.LINQ));

		return query.Include(include.LinqExpression);
	}

	private static IQueryable<TEntity> IncludeString<TEntity>(IQueryable<TEntity> query,
		IncludeExpression<TEntity> include) where TEntity : class
	{
		if (string.IsNullOrEmpty(include.StringExpression))
			throw new ArgumentNullException(
				string.Format(MissingExpressionErrorMsg, include.StringExpression, IncludeType.String));

		return query.Include(include.StringExpression);
	}
}