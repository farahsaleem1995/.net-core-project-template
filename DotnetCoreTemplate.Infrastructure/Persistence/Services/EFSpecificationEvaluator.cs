using AutoMapper.QueryableExtensions;
using DotnetCoreTemplate.Application.Shared.Specifications.Enums;
using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFSpecificationEvaluator : ISpecificationEvaluator
{
	private readonly IEnumerable<IEvaluator> _evaluators;

	public EFSpecificationEvaluator(IEnumerable<IEvaluator> evaluators)
	{
		_evaluators = evaluators;
	}

	public IQueryable<T> EvaluateQuery<T>(IQueryable<T> query, ISpecification<T> specification,
		bool onlyEvaluateFilter) where T : class
	{
		var evaluatos = GetEvaluators(onlyEvaluateFilter);

		foreach (var evaluator in evaluatos)
		{
			query = evaluator.EvaluateQuery(query, specification);
		}

		return query;
	}

	private IEnumerable<IEvaluator> GetEvaluators(bool onlyEvaluateFilter)
	{
		return onlyEvaluateFilter ? _evaluators.Where(e => e.IsCriteriaEvaluator) : _evaluators;
	}

	public IQueryable<TOut> EvaluateQuery<T, TOut>(IQueryable<T> query, ISpecification<T, TOut> specification,
		bool onlyEvaluateFilter) where T : class
	{
		query = EvaluateQuery<T>(query, specification, onlyEvaluateFilter);

		return Project(query, specification);
	}

	private static IQueryable<TOut> Project<T, TOut>(IQueryable<T> query, ISpecification<T, TOut> specification)
		where T : class
	{
		return specification.ProjectExpression.Type switch
		{
			ProjectionType.AutoMapper => Map<T, TOut>(query, specification),
			ProjectionType.Selection => Select(query, specification),
			_ => throw new NotImplementedException(),
		};
	}

	private static IQueryable<TOut> Map<T, TOut>(IQueryable<T> query, ISpecification<T, TOut> specification)
		where T : class
	{
		const string ErrorMsg = "Specification of type '{0}' does not specify a mapping configuration.";

		if (specification.ProjectExpression.Configuration == null)
		{
			throw new InvalidOperationException(string.Format(ErrorMsg, specification.GetType()));
		}

		return query.ProjectTo<TOut>(specification.ProjectExpression.Configuration);
	}

	private static IQueryable<TOut> Select<T, TOut>(IQueryable<T> query, ISpecification<T, TOut> specification)
		where T : class
	{
		const string ErrorMsg = "Specification of type '{0}' does not specify a select expression.";

		if (specification.ProjectExpression.Expression == null)
		{
			throw new InvalidOperationException(string.Format(ErrorMsg, specification.GetType()));
		}

		return query.Select(specification.ProjectExpression.Expression);
	}
}