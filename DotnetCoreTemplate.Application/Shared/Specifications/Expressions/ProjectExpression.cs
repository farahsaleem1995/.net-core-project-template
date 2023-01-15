using AutoMapper;
using DotnetCoreTemplate.Application.Shared.Specifications.Enums;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specifications.Expressions;

public record ProjectExpression<TEntity, TResult>
{
	public ProjectExpression(IConfigurationProvider configuration)
	{
		Type = ProjectionType.AutoMapper;
		Configuration = configuration;
	}

	public ProjectExpression(Expression<Func<TEntity, TResult>> expression)
	{
		Type = ProjectionType.Selection;
		Expression = expression;
	}

	public ProjectionType Type { get; private set; }

	public IConfigurationProvider? Configuration { get; }

	public Expression<Func<TEntity, TResult>>? Expression { get; }
}