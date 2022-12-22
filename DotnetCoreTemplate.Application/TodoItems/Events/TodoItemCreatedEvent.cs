using DotnetCoreTemplate.Application.Shared.Interfaces;

namespace DotnetCoreTemplate.Application.TodoItems.Events;

public record TodoItemCreatedEvent(int Id) : IDomainEvent;