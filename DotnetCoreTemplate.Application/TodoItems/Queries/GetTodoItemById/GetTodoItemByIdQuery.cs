using DotnetCoreTemplate.Application.Shared.Attributes;
using DotnetCoreTemplate.Application.Shared.Enums;
using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;

[Security(SecurityRole.Individual)]
public record GetTodoItemByIdQuery(int Id) : IQuery<TodoItemDto>;