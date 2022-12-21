using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification.Expressions;

public record SelectExpression<TEntity, TResult>(Expression<Func<TEntity, TResult>> Expression);