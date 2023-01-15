using DotnetCoreTemplate.Application.Shared.Specifications.Enums;
using DotnetCoreTemplate.Application.Shared.Specifications.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

public interface ISpecification<T, TOut> : ISpecification<T> where T : class
{
	ProjectExpression<T, TOut> ProjectExpression { get; }
}

public interface ISpecification<T> where T : class
{
	IReadOnlyCollection<FilterExpression<T>> FilterExpressions { get; }

	IReadOnlyCollection<IncludeExpression<T>> IncludeExpressions { get; }

	IReadOnlyCollection<OrderExpression<T>> OrderExpressions { get; }

	int PageNumber { get; }

	int PageSize { get; }

	TrackingBehaviour TrackingBehaviour { get; }

	QueryBehaviour QueryBehaviour { get; }
}