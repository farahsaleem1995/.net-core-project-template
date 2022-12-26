using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.UpdateTodoItem;

[Security(SecurityRole.Individual)]
public record UpdateTodoItemCommand(int Id, string Title, string Description) : ICommand;