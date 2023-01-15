namespace DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

public interface IEvaluator
{
	bool IsCriteriaEvaluator { get; }

	IQueryable<T> EvaluateQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class;
}