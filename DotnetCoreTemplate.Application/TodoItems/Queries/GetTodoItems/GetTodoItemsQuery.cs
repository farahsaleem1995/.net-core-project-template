using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItems;

public class GetTodoItemsQuery : IQuery<PaginatedList<TodoItemsDto>>
{
	public int PageNumber { get; set; } = 1;

	public byte PageSize { get; set; } = 10;
}