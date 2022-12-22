using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItems;

public class TodoItemsProjectSpecification : ProjectSpecificationBase<TodoItem, TodoItemsDto>
{
	public TodoItemsProjectSpecification(int pageNumber, byte pageSize)
	{
		Paginate(pageNumber, pageSize)
			.OrderBy(t => t.Id);

		Project(t => new TodoItemsDto(t.Id, t.Title, t.Description, t.Status));
	}
}