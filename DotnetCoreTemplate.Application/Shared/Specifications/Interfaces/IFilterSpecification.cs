using DotnetCoreTemplate.Application.Shared.Specifications.Expressions;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

public interface IFilterSpecification<T>
{
	IReadOnlyCollection<FilterExpression<T>> FilterExpressions { get; }
}