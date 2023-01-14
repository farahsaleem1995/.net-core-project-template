using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Application.Shared.Specification.Enums;
using DotnetCoreTemplate.Application.Shared.Specification.Expressions;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFOrderEvaluator : IEvaluator
{
	public IQueryable<TEntity> Evaluate<TEntity>(IQueryable<TEntity> query, SpecificationBase<TEntity> specification)
		where TEntity : class
	{
		foreach (var order in specification.OrderExpressions)
		{
			query = Order(query, order);
		}

		return query;
	}

	private static IQueryable<TEntity> Order<TEntity>(IQueryable<TEntity> query, OrderExpression<TEntity> order)
		where TEntity : class
	{
		if (order.Direction == OrderDirection.Ascending)
		{
			if (IsOrdered(query))
				query = AsOrdered(query).ThenBy(order.Expression);
			else
				query = query.OrderBy(order.Expression);
		}
		else if (order.Direction == OrderDirection.Descending)
		{
			if (IsOrdered(query))
				query = AsOrdered(query).ThenByDescending(order.Expression);
			else
				query = query.OrderByDescending(order.Expression);
		}

		return query;
	}

	public static bool IsOrdered<T>(IQueryable<T> query)
	{
		return query.Expression.Type == typeof(IOrderedQueryable<T>);
	}

	public static IOrderedQueryable<T> AsOrdered<T>(IQueryable<T> query)
	{
		return (IOrderedQueryable<T>)query;
	}
}