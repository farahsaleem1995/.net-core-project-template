using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDone;
public record SetTodoItemAsDoneCommand(int Id) : ICommand;