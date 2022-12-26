using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDone;

[Security(SecurityRole.Individual)]
public record SetTodoItemAsDoneCommand(int Id) : ICommand;