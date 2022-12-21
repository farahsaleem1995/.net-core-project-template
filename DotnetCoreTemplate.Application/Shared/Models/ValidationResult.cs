namespace DotnetCoreTemplate.Application.Shared.Models;

public class ValidationResult
{
	public bool IsValid => !Errors.Any();

	public IDictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();

	public void AddError(string property, string error)
	{
		if (!Errors.ContainsKey(property))
		{
			Errors.Add(property, new List<string>());
		}

		Errors[property].Add(error);
	}
}