using System.Collections.Generic;

namespace Terraria.ModLoader;

public static class ResourceDisplaySetLoader
{
	internal static readonly IList<ModResourceDisplaySet> moddedDisplaySets = new List<ModResourceDisplaySet>();

	public static int DisplaySetCount => moddedDisplaySets.Count;

	internal static int Add(ModResourceDisplaySet displaySet)
	{
		moddedDisplaySets.Add(displaySet);
		return DisplaySetCount - 1;
	}

	public static ModResourceDisplaySet GetDisplaySet(int type)
	{
		if (type < 0 || type >= DisplaySetCount)
		{
			return null;
		}
		return moddedDisplaySets[type];
	}

	internal static void Unload()
	{
		moddedDisplaySets.Clear();
		Main.ResourceSetsManager.ResetToVanilla();
	}
}
