using DotnetCoreTemplate.Application.Shared.Specification.Enums;
using DotnetCoreTemplate.Application.Shared.Specification.Expressions;
using DotnetCoreTemplate.Application.Shared.Specification.Interfaces;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification;

public class SpecificationBase<TEntity> :
	IFilterSpecification<TEntity>,
	IOrderSpecification<TEntity>,
	IIncludeSpecification<TEntity>,
	IPaginationSpecification
	where TEntity : class
{
	private readonly List<FilterExpression<TEntity>> _filterExpressions = new();
	private readonly List<OrderExpression<TEntity>> _orderExpressions = new();
	private readonly List<IncludeExpression<TEntity>> _includeExpressions = new();

	public IReadOnlyCollection<FilterExpression<TEntity>> FilterExpressions => _filterExpressions;

	public IReadOnlyCollection<OrderExpression<TEntity>> OrderExpressions => _orderExpressions;

	public IReadOnlyCollection<IncludeExpression<TEntity>> IncludeExpressions => _includeExpressions;

	public bool ApplyPagination { get; private set; }

	public int PageNumber { get; private set; }

	public byte PageSize { get; private set; }

	public SpecificationBase<TEntity> WithFilter(Expression<Func<TEntity, bool>> expression)
	{
		_filterExpressions.Add(new FilterExpression<TEntity>(expression));

		return this;
	}

	public SpecificationBase<TEntity> OrderBy(Expression<Func<TEntity, object>> expression)
	{
		_orderExpressions.Add(new OrderExpression<TEntity>(expression, OrderDirection.Ascending));

		return this;
	}

	public SpecificationBase<TEntity> OrderByDescending(Expression<Func<TEntity, object>> expression)
	{
		_orderExpressions.Add(new OrderExpression<TEntity>(expression, OrderDirection.Descending));

		return this;
	}

	public SpecificationBase<TEntity> Include(IncludeExpression<TEntity> includeExpression)
	{
		_includeExpressions.Add(includeExpression);

		return this;
	}

	public SpecificationBase<TEntity> Paginate(int pageNumber, byte pageSize)
	{
		ApplyPagination = true;
		PageNumber = pageNumber;
		PageSize = pageSize;

		return this;
	}
}