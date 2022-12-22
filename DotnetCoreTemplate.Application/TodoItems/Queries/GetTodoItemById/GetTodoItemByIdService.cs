using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.TodoItems.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;

public class GetTodoItemByIdService : IQueryService<GetTodoItemByIdQuery, TodoItemDto>
{
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public GetTodoItemByIdService(
		IRepository<TodoItem> todoItemsRepository)
	{
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<TodoItemDto> Execute(GetTodoItemByIdQuery query, CancellationToken cancellation)
	{
		var todoItem = await _todoItemsRepository.FirstOrDefaultAsync(
			new TodoItemByIdSpecification(query.Id), cancellation);

		if (todoItem == null)
			throw new NotFoundException(typeof(TodoItem), query.Id);

		return new TodoItemDto(todoItem.Id, todoItem.Title, todoItem.Description, todoItem.Status);
	}
}