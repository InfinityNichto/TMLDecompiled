using System.Collections.Generic;

namespace Terraria.ModLoader;

/// <summary> Serves as a highest-level base for loaders. </summary>
public abstract class Loader : ILoader
{
	public int VanillaCount { get; set; }

	internal int TotalCount { get; set; }

	/// <summary>
	/// Initializes the loader based on the vanilla count of the ModType.
	/// </summary>
	internal void Initialize(int vanillaCount)
	{
		VanillaCount = vanillaCount;
		TotalCount = vanillaCount;
	}

	protected int Reserve()
	{
		return TotalCount++;
	}

	internal virtual void ResizeArrays()
	{
	}

	internal virtual void Unload()
	{
		TotalCount = VanillaCount;
	}

	void ILoader.ResizeArrays()
	{
		ResizeArrays();
	}

	void ILoader.Unload()
	{
		Unload();
	}
}
/// <summary> Serves as a highest-level base for loaders of mod types. </summary>
public abstract class Loader<T> : Loader where T : ModType
{
	internal List<T> list = new List<T>();

	public int Register(T obj)
	{
		int result = Reserve();
		ModTypeLookup<T>.Register(obj);
		list.Add(obj);
		return result;
	}

	public T Get(int id)
	{
		if (id < base.VanillaCount || id >= base.TotalCount)
		{
			return null;
		}
		return list[id - base.VanillaCount];
	}

	internal override void Unload()
	{
		base.Unload();
		list.Clear();
	}
}
