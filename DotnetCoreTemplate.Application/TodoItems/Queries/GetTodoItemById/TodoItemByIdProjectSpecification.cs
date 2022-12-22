using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;

public class TodoItemByIdProjectSpecification : ProjectSpecificationBase<TodoItem, TodoItemDto>
{
	public TodoItemByIdProjectSpecification(int todoItemId)
	{
		WithFilter(t => t.Id == todoItemId);

		Project(t => new TodoItemDto(t.Id, t.Title, t.Description, t.Status));
	}
}