using AutoMapper;
using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;

public class GetTodoItemByIdHandler : IRequestHandler<GetTodoItemByIdQuery, TodoItemDto>
{
	private readonly IRepository<TodoItem> _todoItemsRepository;
	private readonly IMapper _mapper;

	public GetTodoItemByIdHandler(
		IRepository<TodoItem> todoItemsRepository,
		IMapper mapper)
	{
		_todoItemsRepository = todoItemsRepository;
		_mapper = mapper;
	}

	public async Task<TodoItemDto> Handle(GetTodoItemByIdQuery request, CancellationToken cancellation)
	{
		var todoItem = await _todoItemsRepository.FirstOrDefaultAsync(
			new TodoItemByIdProjectSpecification(request.Id, _mapper), cancellation);

		return todoItem ?? throw new NotFoundException(typeof(TodoItem), request.Id);
	}
}