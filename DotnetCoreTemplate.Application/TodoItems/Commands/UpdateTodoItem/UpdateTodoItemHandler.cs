using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.TodoItems.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemHandler : IRequestHandler<UpdateTodoItemCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public UpdateTodoItemHandler(
		IUnitOfWork unitOfWork,
		IRepository<TodoItem> todoItemsRepository)
	{
		_unitOfWork = unitOfWork;
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<Unit> Handle(UpdateTodoItemCommand request, CancellationToken cancellation)
	{
		var todoItem = await GetTodoItem(request.Id, cancellation);

		todoItem.Update(request.Title, request.Description);

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