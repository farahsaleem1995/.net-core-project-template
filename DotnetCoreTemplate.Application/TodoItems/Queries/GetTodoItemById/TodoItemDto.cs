using DotnetCoreTemplate.Application.Shared.Mapping;
using DotnetCoreTemplate.Domain.Entities;
using DotnetCoreTemplate.Domain.Enums;

namespace DotnetCoreTemplate.Application.TodoItems.Queries.GetTodoItemById;

public record TodoItemDto : IMapFrom<TodoItem>
{
	public int Id { get; set; }

	public string Title { get; set; } = null!;

	public string Description { get; set; } = null!;

	public TodoItemStatus Status { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime? LastUpdatedDate { get; set; }
}