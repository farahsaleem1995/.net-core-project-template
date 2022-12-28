using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Commands.CreateTodoItem;

//[Security(SecurityRole.Individual)]
public record CreateTodoItemCommand(string Title, string Description) : ICommand<int>;