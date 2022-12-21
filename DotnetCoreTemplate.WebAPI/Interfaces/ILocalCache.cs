﻿namespace DotnetCoreTemplate.WebAPI.Interfaces;

public interface ILocalCache<TKey, TValue> where TKey : notnull
{
	TValue Get(TKey key, Func<TKey, TValue> fallback);
}