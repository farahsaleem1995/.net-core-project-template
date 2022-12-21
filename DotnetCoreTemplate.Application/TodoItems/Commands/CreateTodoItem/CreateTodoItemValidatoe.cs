using FluentValidation;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.CreateTodoItem;

public class CreateTodoItemValidatoe : AbstractValidator<CreateTodoItemCommand>
{
	public CreateTodoItemValidatoe()
	{
		RuleFor(v => v.Title).NotEmpty().MaximumLength(256);
		RuleFor(v => v.Description).NotEmpty().MaximumLength(2048);
	}
}