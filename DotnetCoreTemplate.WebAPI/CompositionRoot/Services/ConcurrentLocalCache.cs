﻿using DotnetCoreTemplate.Application.Shared.Interfaces;
using System.Collections.Concurrent;

namespace DotnetCoreTemplate.WebAPI.CompositionRoot.Services;

public class ConcurrentLocalCache<TKey, TValue> : ILocalCache<TKey, TValue>
	where TKey : notnull
{
	private readonly ConcurrentDictionary<TKey, TValue> _cache = new();

	public TValue Get(TKey key, Func<TKey, TValue> fallback)
	{
		return _cache.GetOrAdd(key, key => fallback(key));
	}
}