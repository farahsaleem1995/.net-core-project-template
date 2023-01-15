using DotnetCoreTemplate.Application.Shared.Specifications.Expressions;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

public interface IOrderSpecification<T>
{
	IReadOnlyCollection<OrderExpression<T>> OrderExpressions { get; }
}