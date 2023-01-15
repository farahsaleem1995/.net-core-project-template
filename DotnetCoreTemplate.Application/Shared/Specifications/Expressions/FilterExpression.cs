using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specifications.Expressions;

public record FilterExpression<TEntity>(Expression<Func<TEntity, bool>> Expression);