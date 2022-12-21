using DotnetCoreTemplate.Application.Shared.Specification.Enums;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification.Expressions;

public record OrderExpression<TEntity>(Expression<Func<TEntity, object>> Expression, OrderDirection Direction);