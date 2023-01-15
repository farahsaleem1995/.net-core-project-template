using DotnetCoreTemplate.Application.Shared.Specifications.Enums;
using DotnetCoreTemplate.Application.Shared.Specifications.Expressions;
using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specifications;

public abstract class SpecificationBase<T, TOut> : SpecificationBase<T>, ISpecification<T, TOut>
	where T : class
{
	private const string EmptyProjectionErrorMsg = "Projection from type '{0}' to type '{1}' was not configured.";

	private ProjectExpression<T, TOut>? _projectExpression;

	public ProjectExpression<T, TOut> ProjectExpression
	{
		get => _projectExpression ?? throw new InvalidOperationException(
			string.Format(EmptyProjectionErrorMsg, typeof(T), typeof(TOut)));
		private set => _projectExpression = value;
	}

	public void Project(Expression<Func<T, TOut>> expression)
	{
		ProjectExpression = new ProjectExpression<T, TOut>(expression);
	}
}

public abstract class SpecificationBase<T> : ISpecification<T>
	where T : class
{
	private readonly List<FilterExpression<T>> _filterExpressions = new();
	private readonly List<OrderExpression<T>> _orderExpressions = new();
	private readonly List<IncludeExpression<T>> _includeExpressions = new();

	public IReadOnlyCollection<FilterExpression<T>> FilterExpressions => _filterExpressions;

	public IReadOnlyCollection<OrderExpression<T>> OrderExpressions => _orderExpressions;

	public IReadOnlyCollection<IncludeExpression<T>> IncludeExpressions => _includeExpressions;

	public int PageNumber { get; private set; }

	public int PageSize { get; private set; }

	public void WithFilter(Expression<Func<T, bool>> expression)
	{
		_filterExpressions.Add(new FilterExpression<T>(expression));
	}

	public void OrderBy(Expression<Func<T, object>> expression)
	{
		_orderExpressions.Add(new OrderExpression<T>(expression, OrderDirection.Ascending));
	}

	public void OrderByDescending(Expression<Func<T, object>> expression)
	{
		_orderExpressions.Add(new OrderExpression<T>(expression, OrderDirection.Descending));
	}

	public void Include(Expression<Func<T, object>> expression)
	{
		_includeExpressions.Add(new IncludeExpression<T>(expression));
	}

	public void Page(int pageNumber)
	{
		PageNumber = pageNumber;
	}

	public void Size(int pageSize)
	{
		PageSize = pageSize;
	}
}