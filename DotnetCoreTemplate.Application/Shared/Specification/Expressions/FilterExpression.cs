using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification.Expressions;

public record FilterExpression<TEntity>(Expression<Func<TEntity, bool>> Expression);