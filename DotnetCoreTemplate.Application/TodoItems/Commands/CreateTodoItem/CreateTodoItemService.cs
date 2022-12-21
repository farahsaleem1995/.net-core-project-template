using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemService : ICommandService<CreateTodoItemCommand, int>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public CreateTodoItemService(
		IUnitOfWork unitOfWork,
		IRepository<TodoItem> todoItemsRepository)
	{
		_unitOfWork = unitOfWork;
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<int> Execute(CreateTodoItemCommand command, CancellationToken cancellation)
	{
		var todoItem = TodoItem.Create(command.Title, command.Description);

		await _todoItemsRepository.AddAsync(todoItem, cancellation);

		await _unitOfWork.SaveAsync(cancellation);

		return todoItem.Id;
	}
}