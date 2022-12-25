namespace DotnetCoreTemplate.Application.Shared.Models;

public class ResultBase
{
	protected ResultBase(bool succeeded, string[] errors)
	{
		Succeeded = succeeded;
		Errors = errors;
	}

	public bool Succeeded { get; protected set; }

	public string[] Errors { get; protected set; }
}