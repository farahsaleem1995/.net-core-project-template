using DotnetCoreTemplate.Application.Shared.Specification.Enums;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification.Expressions;

public record ProjectExpression<TEntity, TResult>
{
	public ProjectExpression(Expression<Func<TEntity, TResult>> expression)
	{
		Type = ProjectionType.Selection;
		Expression = expression;
	}

	public ProjectionType Type { get; private set; }

	public Expression<Func<TEntity, TResult>> Expression { get; }
}