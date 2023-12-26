using System;
using System.Collections.Generic;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader;

public static class GlobalList<TGlobal> where TGlobal : GlobalType<TGlobal>
{
	private static bool loadingFinished = false;

	private static List<TGlobal> _globals = new List<TGlobal>();

	/// <summary>
	/// All registered globals. Empty until all globals have loaded
	/// </summary>
	public static TGlobal[] Globals { get; private set; } = Array.Empty<TGlobal>();


	public static int SlotsPerEntity { get; private set; }

	public static int EntityTypeCount { get; private set; }

	internal static (short index, short perEntityIndex) Register(TGlobal global)
	{
		if (loadingFinished)
		{
			throw new Exception("Loading has finished. Cannot add more globals");
		}
		short item = (short)_globals.Count;
		short perEntityIndex = (short)(global.SlotPerEntity ? SlotsPerEntity++ : (-1));
		_globals.Add(global);
		return (index: item, perEntityIndex: perEntityIndex);
	}

	/// <summary>
	/// Call during <see cref="M:Terraria.ModLoader.ILoader.ResizeArrays" />. Which runs after all <see cref="M:Terraria.ModLoader.ILoadable.Load(Terraria.ModLoader.Mod)" /> calls, but before any <see cref="M:Terraria.ModLoader.ModType.SetupContent" /> calls
	/// </summary>
	public static void FinishLoading(int typeCount)
	{
		if (loadingFinished)
		{
			throw new Exception("FinishLoading already called");
		}
		loadingFinished = true;
		Globals = _globals.ToArray();
		EntityTypeCount = typeCount;
	}

	/// <summary>
	/// Call during unloading, to clear the globals list
	/// </summary>
	public static void Reset()
	{
		LoaderUtils.ResetStaticMembers(typeof(GlobalList<TGlobal>));
	}
}
