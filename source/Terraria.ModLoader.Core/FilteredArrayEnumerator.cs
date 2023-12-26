using System.Runtime.CompilerServices;

namespace Terraria.ModLoader.Core;

public ref struct FilteredArrayEnumerator<T>
{
	private readonly T[] arr;

	private readonly int[] inds;

	private T current;

	private int i;

	public T Current => current;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public FilteredArrayEnumerator(T[] arr, int[] inds)
	{
		this.arr = arr;
		this.inds = inds;
		current = default(T);
		i = 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool MoveNext()
	{
		if (i >= inds.Length)
		{
			return false;
		}
		current = arr[inds[i++]];
		return true;
	}

	public FilteredArrayEnumerator<T> GetEnumerator()
	{
		return this;
	}
}
