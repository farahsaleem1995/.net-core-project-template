namespace DotnetCoreTemplate.Application.Shared.Specifications.Interfaces;

public interface IPaginationSpecification
{
	int PageNumber { get; }

	int PageSize { get; }
}