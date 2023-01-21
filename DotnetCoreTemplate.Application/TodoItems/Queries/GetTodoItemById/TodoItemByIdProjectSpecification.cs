using AutoMapper;
using DotnetCoreTemplate.Application.Shared.Specifications;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;

public class TodoItemByIdProjectSpecification : SpecificationBase<TodoItem, TodoItemDto>
{
	public TodoItemByIdProjectSpecification(int todoItemId, IMapper mapper)
	{
		WithFilter(t => t.Id == todoItemId);

		Project(mapper.ConfigurationProvider);
	}
}