using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDoing;

public record SetTodoItemAsDoingCommand(int Id) : ICommand;