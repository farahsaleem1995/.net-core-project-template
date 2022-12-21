namespace DotnetCoreTemplate.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand(int Id, string Title, string Description);