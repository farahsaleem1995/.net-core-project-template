using DotnetCoreTemplate.Application.Shared.Specifications.Expressions;

namespace DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

public interface ISpecification<T, TOut> : ISpecification<T>
	where T : class
{
	ProjectExpression<T, TOut> ProjectExpression { get; }
}

public interface ISpecification<T>
	: IIncludeSpecification<T>,
	IFilterSpecification<T>,
	IOrderSpecification<T>,
	IPaginationSpecification
	where T : class
{
}