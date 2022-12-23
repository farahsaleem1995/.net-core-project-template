using DotnetCoreTemplate.Application.Shared.Exceptions;
using DotnetCoreTemplate.Application.Shared.Interfaces;
using DotnetCoreTemplate.Application.Shared.Models;
using DotnetCoreTemplate.Application.TodoItems.Specification;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDone;

public class SetTodoItemAsDoneHandler : IRequestHandler<SetTodoItemAsDoneCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRepository<TodoItem> _todoItemsRepository;

	public SetTodoItemAsDoneHandler(
		IUnitOfWork unitOfWork,
		IRepository<TodoItem> todoItemsRepository)
	{
		_unitOfWork = unitOfWork;
		_todoItemsRepository = todoItemsRepository;
	}

	public async Task<Unit> Handle(SetTodoItemAsDoneCommand request, CancellationToken cancellation)
	{
		var todoItem = await _todoItemsRepository.FirstOrDefaultAsync(
			new TodoItemByIdSpecification(request.Id));

		if (todoItem == null)
			throw new NotFoundException(typeof(TodoItem), request.Id);

		todoItem.Done();

		await _unitOfWork.SaveAsync();

		return Unit.Value;
	}
}