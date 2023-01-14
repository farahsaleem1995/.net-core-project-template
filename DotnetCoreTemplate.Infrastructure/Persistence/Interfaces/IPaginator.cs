using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.Shared.Specification.Interfaces;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;

public interface IPaginator
{
	Task<PaginatedList<T>> Paginate<T>(
		IQueryable<T> query, IPaginationSpecification specification, CancellationToken cancellation);
}