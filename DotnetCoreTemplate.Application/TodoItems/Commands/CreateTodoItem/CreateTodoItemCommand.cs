using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand(string Title, string Description) : ICommand<int>;