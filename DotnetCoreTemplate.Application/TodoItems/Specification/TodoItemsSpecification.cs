using DotnetCoreTemplate.Application.Shared.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Specification;

public class TodoItemsSpecification : SpecificationBase<TodoItem>
{
	public TodoItemsSpecification(int pageNumber, byte pageSize)
	{
		Paginate(pageNumber, pageSize);
	}
}