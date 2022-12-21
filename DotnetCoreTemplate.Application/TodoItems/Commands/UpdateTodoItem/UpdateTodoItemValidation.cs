using FluentValidation;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.UpdateTodoItem;

public class UpdateTodoItemValidation : AbstractValidator<UpdateTodoItemCommand>
{
	public UpdateTodoItemValidation()
	{
		RuleFor(v => v.Title).NotEmpty().MaximumLength(256);
		RuleFor(v => v.Description).NotEmpty().MaximumLength(2048);
	}
}