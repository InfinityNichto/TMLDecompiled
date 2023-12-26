using System;

namespace Terraria.ModLoader;

public ref struct RefReadOnlyArray<T>
{
	internal readonly T[] array;

	public int Length => array.Length;

	public T this[int index] => array[index];

	public RefReadOnlyArray(T[] array)
	{
		this.array = array;
	}

	public ReadOnlySpan<T>.Enumerator GetEnumerator()
	{
		return ((ReadOnlySpan<T>)array).GetEnumerator();
	}

	public static implicit operator RefReadOnlyArray<T>(T[] array)
	{
		return new RefReadOnlyArray<T>(array);
	}

	public static implicit operator ReadOnlySpan<T>(RefReadOnlyArray<T> readOnlyArray)
	{
		return readOnlyArray.array;
	}
}
