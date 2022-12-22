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
		var todoItems = await _todoItemsRepository.PaginateAsync(
			new TodoItemsSpecification(query.PageNumber, query.PageSize), cancellation);

		return Project(todoItems);
	}

	private static PaginatedList<TodoItemsDto> Project(PaginatedList<TodoItem> todoItems)
	{
		var dtoItems = todoItems.Items
			.Select(t => new TodoItemsDto(t.Id, t.Title, t.Description, t.Status))
			.ToList();

		return new PaginatedList<TodoItemsDto>(
			dtoItems, todoItems.TotalItems, todoItems.PageNumber, todoItems.PageSize);
	}
}