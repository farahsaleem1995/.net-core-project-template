using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsToDo;

[Security(SecurityRole.Individual)]
public record SetTodoItemAsToDoCommand(int Id) : ICommand;