using DotnetCoreTemplate.Application.Shared.Specifications.Enums;
using DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Evaluators;

public class EFTrackingBehaviourEvaluator : IEvaluator
{
	public bool IsCriteriaEvaluator => true;

	public IQueryable<T> EvaluateQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
	{
		return specification.TrackingBehaviour switch
		{
			TrackingBehaviour.Track => query.AsTracking(),
			TrackingBehaviour.NoTrack => query.AsNoTracking(),
			_ => throw new NotImplementedException(),
		};
	}
}