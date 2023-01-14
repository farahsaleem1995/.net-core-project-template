using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.TodoItems.Events;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemHandler : IRequestHandler<CreateTodoItemCommand, int>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public CreateTodoItemHandler(
		IUnitOfWork unitOfWork,
		IRepository<TodoItem> todoItemsRepository)
	{
		_unitOfWork = unitOfWork;
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellation)
	{
		var todoItem = TodoItem.Create(request.Title, request.Description);

		await _todoItemsRepository.AddAsync(todoItem, cancellation);

		await _unitOfWork.SaveAsync(cancellation);

		todoItem.DomainEvents.Add(new TodoItemCreatedEvent(todoItem.Id));

		return todoItem.Id;
	}
}