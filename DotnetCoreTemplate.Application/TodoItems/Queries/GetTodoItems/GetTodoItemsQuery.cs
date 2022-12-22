using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItems;

public class GetTodoItemsQuery : IQuery<IEnumerable<TodoItemsDto>>
{
	public int PageNumber { get; set; } = 1;

	public byte PageSize { get; set; } = 10;
}