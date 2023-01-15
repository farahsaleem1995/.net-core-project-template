using DotnetCoreTemplate.Application.Shared.Specifications.Enums;
using DotnetCoreTemplate.Application.Shared.Specifications.Expressions;
using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Evaluators;

public class EFOrderEvaluator : IEvaluator
{
	public bool IsCriteriaEvaluator => false;

	public IQueryable<TEntity> EvaluateQuery<TEntity>(IQueryable<TEntity> query, ISpecification<TEntity> specification)
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
		return order.Direction switch
		{
			OrderDirection.Ascending => OrderAscending(query, order),
			OrderDirection.Descending => OrderDescending(query, order),
			_ => throw new NotImplementedException(),
		};
	}

	private static IQueryable<TEntity> OrderDescending<TEntity>(IQueryable<TEntity> query,
		OrderExpression<TEntity> order) where TEntity : class
	{
		if (IsOrdered(query))
			query = AsOrdered(query).ThenByDescending(order.Expression);
		else
			query = query.OrderByDescending(order.Expression);

		return query;
	}

	private static IQueryable<TEntity> OrderAscending<TEntity>(IQueryable<TEntity> query,
		OrderExpression<TEntity> order) where TEntity : class
	{
		if (IsOrdered(query))
			query = AsOrdered(query).ThenBy(order.Expression);
		else
			query = query.OrderBy(order.Expression);

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