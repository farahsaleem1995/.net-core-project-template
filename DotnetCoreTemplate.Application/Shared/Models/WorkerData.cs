using System.Collections;

namespace DotnetCoreTemplate.Application.Shared.Models;

public class WorkerData : IEnumerable<KeyValuePair<string, object>>
{
	private readonly Dictionary<string, object> _data = new();

	public void Put<T>(string key, T value) where T : notnull
	{
		if (_data.ContainsKey(key))
			_data[key] = value;

		_data.Add(key, value);
	}

	public T? Get<T>(string key) where T : notnull
	{
		var value = _data.GetValueOrDefault(key);

		try
		{
			var converted = Convert.ChangeType(value, typeof(T));

			return (T?)converted;
		}
		catch (Exception)
		{
			return default;
		}
	}

	public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
	{
		return _data.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}