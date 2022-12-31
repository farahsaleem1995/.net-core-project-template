namespace DotnetCoreTemplate.WebAPI.CompositionRoot.DIConfiguration;

public static class ConfigurationExtensions
{
	public static T GetOrThrow<T>(this IConfiguration configuration, string key)
	{
		const string bindErrorMessage =
			"Unable to bind JSON settings to object of type '{0}', because JSON key '{1}' was not found.";

		var settings = configuration.GetSection(key).Get<T>();
		if (settings == null)
		{
			throw new InvalidOperationException(string.Format(bindErrorMessage, typeof(T), key));
		}

		return settings;
	}
}