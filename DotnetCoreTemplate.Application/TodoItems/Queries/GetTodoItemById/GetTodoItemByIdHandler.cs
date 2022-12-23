using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;

public class GetTodoItemByIdHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItemDto>
{
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public GetTodoItemByIdHandler(
		IRepository<TodoItem> todoItemsRepository)
	{
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<TodoItemDto> Handle(GetTodoItemByIdQuery request, CancellationToken cancellation)
	{
		var todoItem = await _todoItemsRepository.FirstOrDefaultAsync(
			new TodoItemByIdProjectSpecification(request.Id), cancellation);

		return todoItem ?? throw new NotFoundException(typeof(TodoItem), request.Id);
	}
}