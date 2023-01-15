using DotnetCoreTemplate.Application.Shared.Specifications.Enums;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specifications.Expressions;

public record OrderExpression<TEntity>(Expression<Func<TEntity, object>> Expression, OrderDirection Direction);