using DotnetCoreTemplate.Application.Shared.Specification.Enums;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification.Expressions;
public record IncludeExpression<TEntity>
{
    public IncludeExpression(string expression)
    {
        Type = IncludeType.String;
        StringExpression = expression;
    }

    public IncludeExpression(Expression<Func<TEntity, object>> expression)
    {
        Type = IncludeType.LINQ;
        LinqExpression = expression;
    }

    public IncludeType Type { get; }

    public string? StringExpression { get; }

    public Expression<Func<TEntity, object>>? LinqExpression { get; }
}