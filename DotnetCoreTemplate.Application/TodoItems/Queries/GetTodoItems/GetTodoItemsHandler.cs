using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItems;

public class GetTodoItemsHandler : IRequestHandler<GetTodoItemsQuery, PaginatedList<TodoItemsDto>>
{
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public GetTodoItemsHandler(
		IRepository<TodoItem> todoItemsRepository)
	{
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<PaginatedList<TodoItemsDto>> Handle(GetTodoItemsQuery request, CancellationToken cancellation)
	{
		return await _todoItemsRepository.PaginateAsync(
			new TodoItemsProjectSpecification(request.PageNumber, request.PageSize), cancellation);
	}
}