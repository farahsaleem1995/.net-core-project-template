using DotnetCoreTemplate.Domain.Shared;

namespace DotnetCoreTemplate.Application.TodoItems.Events;

public class TodoItemCreatedEvent : DomainEvent
{
	public TodoItemCreatedEvent(int id)
	{
		Id = id;
	}

	public int Id { get; set; }
}