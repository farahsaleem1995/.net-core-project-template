using DotnetCoreTemplate.Application.Shared.Specification.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification.Interfaces;

public interface IFilterSpecification<TEntity>
{
    IReadOnlyCollection<FilterExpression<TEntity>> FilterExpressions { get; }
}