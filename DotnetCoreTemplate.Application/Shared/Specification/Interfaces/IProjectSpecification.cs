using DotnetCoreTemplate.Application.Shared.Specification.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification.Interfaces;

public interface IProjectSpecification<TEntity, TResult>
{
	ProjectExpression<TEntity, TResult> ProjectExpression { get; }
}