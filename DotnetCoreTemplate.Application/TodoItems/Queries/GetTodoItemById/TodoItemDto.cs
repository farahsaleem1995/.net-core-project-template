using DotnetCoreTemplate.Domain.Enums;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;

public record TodoItemDto(int Id, string Title, string Description, TodoItemStatus Status);