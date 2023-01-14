using DotnetCoreTemplate.Domain.Enums;
using DotnetCoreTemplate.Domain.Shared;

namespace DotnetCoreTemplate.Domain.Entities;

public class TodoItem : Auditable, IHasDomainEvents
{
	private TodoItem(string title, string description)
	{
		Title = title;
		Description = description;
		Status = TodoItemStatus.ToDo;
	}

	public int Id { get; private set; }

	public string Title { get; private set; }

	public string Description { get; private set; }

	public TodoItemStatus Status { get; private set; }

	public ICollection<DomainEvent> DomainEvents { get; private set; } = new List<DomainEvent>();

	public static TodoItem Create(string title, string description)
	{
		return new TodoItem(title, description);
	}

	public void Update(string title, string description)
	{
		Title = title;
		Description = description;
	}

	public void ToDo()
	{
		Status = TodoItemStatus.ToDo;
	}

	public void Doing()
	{
		Status = TodoItemStatus.Doing;
	}

	public void Done()
	{
		Status = TodoItemStatus.Done;
	}
}