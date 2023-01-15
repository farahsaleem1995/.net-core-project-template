namespace DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

public interface ISpecificationEvaluator
{
	IQueryable<T> EvaluateQuery<T>(IQueryable<T> query, ISpecification<T> specification, bool onlyEvaluateFilter)
		where T : class;

	IQueryable<TOut> EvaluateQuery<T, TOut>(IQueryable<T> query, ISpecification<T, TOut> specification,
		bool onlyEvaluateFilter) where T : class;
}

public interface IEvaluator
{
	bool IsCriteriaEvaluator { get; }

	IQueryable<T> EvaluateQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class;
}