namespace DotnetCoreTemplate.Application.Shared.Models;

public class Result : ResultBase
{
	private Result(bool succeeded, string[] errors) : base(succeeded, errors)
	{
	}

	public static Result Succeed()
	{
		return new Result(true, Array.Empty<string>());
	}

	public static Result Fail(string[] errors)
	{
		return new Result(false, errors);
	}
}

public class Result<TValue> : ResultBase where TValue : notnull
{
	private readonly TValue? _value;

	private Result(bool succeeded, string[] errors) : base(succeeded, errors)
	{
		Succeeded = succeeded;
		Errors = errors;
	}

	private Result(bool succeeded, TValue? value) : base(succeeded, Array.Empty<string>())
	{
		Succeeded = succeeded;
		_value = value;
	}

	public TValue Value => _value ?? throw new InvalidOperationException("Invalid sign in.");

	public static Result<TValue> Succeed(TValue value)
	{
		return new Result<TValue>(true, value);
	}

	public static Result<TValue> Fail(string[] errors)
	{
		return new Result<TValue>(false, errors);
	}
}