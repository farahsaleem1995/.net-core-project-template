using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsToDo;
public record SetTodoItemAsToDoCommand(int Id) : ICommand;