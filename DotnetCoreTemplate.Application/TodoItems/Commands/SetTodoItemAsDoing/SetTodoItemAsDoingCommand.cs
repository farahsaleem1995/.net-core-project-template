using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.SetTodoItemAsDoing;

[Security(SecurityRole.Individual)]
public record SetTodoItemAsDoingCommand(int Id) : ICommand;