using DotnetCoreTemplate.Application.Shared.Specifications;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItems;

public class TodoItemsProjectSpecification : SpecificationBase<TodoItem, TodoItemsDto>
{
	public TodoItemsProjectSpecification(int pageNumber, byte pageSize)
	{
		OrderByDescending(t => t.CreatedDate);

		Page(pageNumber);

		Size(pageSize);

		Project(t => new TodoItemsDto(t.Id, t.Title, t.Description, t.Status));
	}
}