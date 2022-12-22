using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.TodoItems.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItems;

public class GetTodoItemsService : IQueryService<GetTodoItemsQuery, IEnumerable<TodoItemsDto>>
{
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public GetTodoItemsService(
		IRepository<TodoItem> todoItemsRepository)
	{
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<IEnumerable<TodoItemsDto>> Execute(GetTodoItemsQuery query, CancellationToken cancellation)
	{
		var todoItems = await _todoItemsRepository.ListAsync(
			new TodoItemsSpecification(query.PageNumber, query.PageSize), cancellation);

		return todoItems.Select(t => new TodoItemsDto(t.Title, t.Description, t.Status));
	}
}