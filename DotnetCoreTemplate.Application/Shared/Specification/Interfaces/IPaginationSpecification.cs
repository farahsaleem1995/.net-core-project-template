namespace DotnetCoreTemplate.Application.Shared.Specification.Interfaces;

public interface IPaginationSpecification
{
    public bool ApplyPagination { get; }

    public int PageNumber { get; }

    public byte PageSize { get; }
}