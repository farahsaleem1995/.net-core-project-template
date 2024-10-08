﻿using DotnetCoreTemplate.Application.Shared.Specifications;
using DotnetCoreTemplate.Domain.Entities;

namespace DotnetCoreTemplate.Application.TodoItems.Specification;

public class TodoItemByIdSpecification : SpecificationBase<TodoItem>
{
	public TodoItemByIdSpecification(int todoItemId)
	{
		WithFilter(t => t.Id == todoItemId);
	}
}