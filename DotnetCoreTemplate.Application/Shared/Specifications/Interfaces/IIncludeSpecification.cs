using DotnetCoreTemplate.Application.Shared.Specifications.Expressions;
using System.Linq.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

public interface IIncludeSpecification<T>
{
	IReadOnlyCollection<IncludeExpression<T>> IncludeExpressions { get; }
}