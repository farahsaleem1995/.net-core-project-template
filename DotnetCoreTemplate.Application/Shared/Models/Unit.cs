﻿namespace DotnetCoreTemplate.Application.Shared.Models;

public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    private static readonly Unit _value = new();

    public static Unit Value => _value;

    public static Task<Unit> Task { get; } = System.Threading.Tasks.Task.FromResult(_value);

    public int CompareTo(Unit other) => 0;

    public int CompareTo(object? obj) => 0;

    public bool Equals(Unit other) => true;

    public override bool Equals(object? obj) => obj is Unit;

    public override int GetHashCode() => 0;

    public static bool operator ==(Unit left, Unit right) => true;

    public static bool operator !=(Unit left, Unit right) => false;

    public static bool operator <(Unit left, Unit right) => false;

    public static bool operator <=(Unit left, Unit right) => false;

    public static bool operator >(Unit left, Unit right) => false;

    public static bool operator >=(Unit left, Unit right) => false;
}