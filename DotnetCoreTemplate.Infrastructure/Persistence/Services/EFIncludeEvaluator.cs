using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Application.Shared.Specification.Enums;
using DotnetCoreTemplate.Application.Shared.Specification.Expressions;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFIncludeEvaluator : IEvaluator
{
	private const string ErrorMsg = "'{0}' cannot be empty when '{1}' is set to '{2}'";

	public IQueryable<TEntity> Evaluate<TEntity>(IQueryable<TEntity> query, SpecificationBase<TEntity> specification)
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
		switch (include.Type)
		{
			case IncludeType.String:
				if (string.IsNullOrEmpty(include.StringExpression))
					throw new ArgumentNullException(
						string.Format(ErrorMsg, include.StringExpression, include.Type, IncludeType.String));

				query = query.Include(include.StringExpression);
				break;

			case IncludeType.LINQ:
				if (include.LinqExpression == null)
					throw new ArgumentNullException(
						string.Format(ErrorMsg, include.LinqExpression, include.Type, IncludeType.LINQ));

				query = query.Include(include.LinqExpression);
				break;

			default:
				throw new NotImplementedException();
		}

		return query;
	}
}