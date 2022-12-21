using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.TodoItems.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemService : ICommandService<UpdateTodoItemCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public UpdateTodoItemService(
		IUnitOfWork unitOfWork,
		IRepository<TodoItem> todoItemsRepository)
	{
		_unitOfWork = unitOfWork;
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<Unit> Execute(UpdateTodoItemCommand command, CancellationToken cancellation)
	{
		var todoItem = await GetTodoItem(command.Id, cancellation);

		todoItem.Update(command.Title, command.Description);

		await _unitOfWork.SaveAsync(cancellation);

		return Unit.Value;
	}

	private async Task<TodoItem> GetTodoItem(int todoItemId, CancellationToken cancellation)
	{
		var todoItem = await _todoItemsRepository.FirstOrDefaultAsync(new TodoItemByIdSpecification(todoItemId),
			cancellation);

		return todoItem ?? throw new NotFoundException(typeof(TodoItem), todoItemId);
	}
}