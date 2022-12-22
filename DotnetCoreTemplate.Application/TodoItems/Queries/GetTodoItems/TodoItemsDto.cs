using DotnetCoreTemplate.Domain.Enums;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItems;
public record TodoItemsDto(int Id, string Title, string Description, TodoItemStatus Status);