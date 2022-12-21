using DotnetCoreTemplate.Application.Shared.Specification.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification.Interfaces;

public interface IOrderSpecification<TEntity>
{
    IReadOnlyCollection<OrderExpression<TEntity>> OrderExpressions { get; }
}