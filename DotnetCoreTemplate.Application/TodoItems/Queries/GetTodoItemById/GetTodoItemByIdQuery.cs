using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;

public record GetTodoItemByIdQuery(int Id) : IQuery<TodoItemDto>;