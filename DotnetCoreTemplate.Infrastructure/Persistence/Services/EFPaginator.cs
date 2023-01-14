using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.Shared.Specification.Interfaces;
using DotnetCoreTemplate.Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotnetCoreTemplate.Infrastructure.Persistence.Services;

public class EFPaginator : IPaginator
{
	public async Task<PaginatedList<T>> Paginate<T>(
		IQueryable<T> query, IPaginationSpecification specification, CancellationToken cancellation)
	{
		if (specification.ApplyPagination)
		{
			var skip = specification.PageSize * (specification.PageNumber - 1);
			var take = specification.PageSize;

			var count = await query.CountAsync(cancellation);
			var list = await query.Skip(skip).Take(take).ToListAsync(cancellation);

			return new PaginatedList<T>(list, count, specification.PageNumber, specification.PageSize);
		}

		throw new InvalidOperationException($"Specification of type ${specification} has no pagination criteria.");
	}
}