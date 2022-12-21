using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.UpdateTodoItem;

public record UpdateTodoItemCommand(int Id, string Title, string Description) : ICommand;