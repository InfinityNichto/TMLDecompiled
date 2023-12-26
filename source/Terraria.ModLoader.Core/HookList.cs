using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Terraria.ModLoader.Core;

public class HookList<T> where T : class, IIndexed
{
	public readonly MethodInfo method;

	private int[] indices = Array.Empty<int>();

	private T[] defaultInstances = Array.Empty<T>();

	public HookList(MethodInfo method)
	{
		this.method = method;
	}

	public FilteredArrayEnumerator<T> Enumerate(T[] instances)
	{
		return new FilteredArrayEnumerator<T>(instances, indices);
	}

	public FilteredSpanEnumerator<T> Enumerate(ReadOnlySpan<T> instances)
	{
		return new FilteredSpanEnumerator<T>(instances, indices);
	}

	public FilteredSpanEnumerator<T> Enumerate(IEntityWithInstances<T> entity)
	{
		return Enumerate(entity.Instances);
	}

	public IEnumerable<T> EnumerateSlow(IReadOnlyList<T> instances)
	{
		int[] array = indices;
		foreach (int i in array)
		{
			yield return instances[i];
		}
	}

	public ReadOnlySpan<T> Enumerate()
	{
		return defaultInstances;
	}

	public void Update(IReadOnlyList<T> allDefaultInstances)
	{
		defaultInstances = allDefaultInstances.WhereMethodIsOverridden(method).ToArray();
		indices = ((IEnumerable<T>)defaultInstances).Select((Func<T, int>)((T g) => g.Index)).ToArray();
	}

	public static HookList<T> Create<F>(Expression<Func<T, F>> expr) where F : Delegate
	{
		return new HookList<T>(expr.ToMethodInfo());
	}
}
