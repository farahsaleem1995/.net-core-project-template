using DotnetCoreTemplate.Application.Shared.Specification.Expressions;
using DotnetCoreTemplate.Application.Shared.Specification.Interfaces;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specification;

public abstract class ProjectSpecificationBase<TEntity, TResult>
	: SpecificationBase<TEntity>, IProjectSpecification<TEntity, TResult>
	where TEntity : class
{
	private const string EmptyProjectionErrorMsg = "Projection from type '{0}' to type '{1}' was not configured.";

	private ProjectExpression<TEntity, TResult>? _projectExpression;

	public ProjectExpression<TEntity, TResult> ProjectExpression
	{
		get => _projectExpression ?? throw new InvalidOperationException(
			string.Format(EmptyProjectionErrorMsg, typeof(TEntity), typeof(TResult)));
		private set => _projectExpression = value;
	}

	public void Project(Expression<Func<TEntity, TResult>> expression)
	{
		ProjectExpression = new ProjectExpression<TEntity, TResult>(expression);
	}
}