using DotnetCoreTemplate.Application.Shared.Specification.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification.Interfaces;

public interface IIncludeSpecification<TEntity>
{
    IReadOnlyCollection<IncludeExpression<TEntity>> IncludeExpressions { get; }
}