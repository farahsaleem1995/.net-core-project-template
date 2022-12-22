using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.TodoItems.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItems;

public class GetTodoItemsService : IQueryService<GetTodoItemsQuery, PaginatedList<TodoItemsDto>>
{
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public GetTodoItemsService(
		IRepository<TodoItem> todoItemsRepository)
	{
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<PaginatedList<TodoItemsDto>> Execute(GetTodoItemsQuery query, CancellationToken cancellation)
	{
		return await _todoItemsRepository.PaginateAsync(
			new TodoItemsProjectSpecification(query.PageNumber, query.PageSize), cancellation);
	}
}