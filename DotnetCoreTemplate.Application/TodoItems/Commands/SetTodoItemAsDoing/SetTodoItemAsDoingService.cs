using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.TodoItems.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDoing;

public class SetTodoItemAsDoingService : ICommandService<SetTodoItemAsDoingCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public SetTodoItemAsDoingService(
		IUnitOfWork unitOfWork,
		IRepository<TodoItem> todoItemsRepository)
	{
		_unitOfWork = unitOfWork;
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<Unit> Execute(SetTodoItemAsDoingCommand command, CancellationToken cancellation)
	{
		var todoItem = await _todoItemsRepository.FirstOrDefaultAsync(
			new TodoItemByIdSpecification(command.Id));

		if (todoItem == null)
			throw new NotFoundException(typeof(TodoItem), command.Id);

		todoItem.Doing();

		await _unitOfWork.SaveAsync();

		return Unit.Value;
	}
}