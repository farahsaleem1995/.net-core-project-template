using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDoing;
using DotnetCoreTemplate.Application.TodoItems.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsToDo;

public class SetTodoItemAsToDoService : ICommandService<SetTodoItemAsToDoCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public SetTodoItemAsToDoService(
		IUnitOfWork unitOfWork,
		IRepository<TodoItem> todoItemsRepository)
	{
		_unitOfWork = unitOfWork;
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<Unit> Execute(SetTodoItemAsToDoCommand command, CancellationToken cancellation)
	{
		var todoItem = await _todoItemsRepository.FirstOrDefaultAsync(
			new TodoItemByIdSpecification(command.Id));

		if (todoItem == null)
			throw new NotFoundException(typeof(TodoItem), command.Id);

		todoItem.ToDo();

		await _unitOfWork.SaveAsync();

		return Unit.Value;
	}
}