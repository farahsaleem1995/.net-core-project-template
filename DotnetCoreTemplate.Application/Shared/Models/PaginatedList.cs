namespace DotnetCoreTemplate.Application.Shared.Models;

public record PaginatedList<T>(List<T> Items, int TotalItems, int PageNumber, byte PageSize)
{
	public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

	public bool HasNext => PageNumber < TotalPages;

	public bool HasPrev => PageNumber > 1;
}