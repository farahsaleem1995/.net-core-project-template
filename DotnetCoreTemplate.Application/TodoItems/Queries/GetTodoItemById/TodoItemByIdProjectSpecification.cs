using DotnetCoreTemplate.Application.Shared.Specifications;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;

public class TodoItemByIdProjectSpecification : SpecificationBase<TodoItem, TodoItemDto>
{
	public TodoItemByIdProjectSpecification(int todoItemId)
	{
		WithFilter(t => t.Id == todoItemId);

		Project(t => new TodoItemDto(
			t.Id, t.Title, t.Description, t.Status, t.CreatedDate, t.LastUpdatedDate));
	}
}