using System.Text.Json;

namespace DotnetCoreTemplate.Infrastructure.Extensions;

public static class JsonSerializerExtension
{
	public static string Serialize<T>(this T instance)
	{
		return JsonSerializer.Serialize(instance);
	}

	public static T? Deserialize<T>(this string json)
	{
		return JsonSerializer.Deserialize<T>(json);
	}

	public static object? Deserialize(this string json, Type returnType)
	{
		return JsonSerializer.Deserialize(json, returnType);
	}

	public static object? Deserialize(this string json)
	{
		return json.Deserialize<object>();
	}
}