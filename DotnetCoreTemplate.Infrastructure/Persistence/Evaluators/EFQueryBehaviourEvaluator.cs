using DotnetCoreTemplate.Application.Shared.Specifications.Enums;
using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Evaluators;

public class EFQueryBehaviourEvaluator : IEvaluator
{
	public bool IsCriteriaEvaluator => true;

	public IQueryable<T> EvaluateQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		return specification.QueryBehaviour switch
		{
			QueryBehaviour.Single => query.AsSingleQuery(),
			QueryBehaviour.Split => query.AsSplitQuery(),
			_ => throw new NotImplementedException(),
		};
	}
}