using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.ModLoader;

public static class HairLoader
{
	internal static readonly IList<ModHair> hairs = new List<ModHair>();

	public static int Count => HairID.Count + hairs.Count;

	internal static int Register(ModHair hair)
	{
		hairs.Add(hair);
		return Count - 1;
	}

	public static ModHair GetHair(int type)
	{
		if (type < HairID.Count || type >= Count)
		{
			return null;
		}
		return hairs[type - HairID.Count];
	}

	public static void UpdateUnlocks(HairstyleUnlocksHelper unlocksHelper, bool inCharacterCreation)
	{
		foreach (ModHair hair in hairs)
		{
			if (inCharacterCreation ? hair.AvailableDuringCharacterCreation : hair.GetUnlockConditions().All((Condition x) => x.IsMet()))
			{
				unlocksHelper.AvailableHairstyles.Add(hair.Type);
			}
		}
	}

	internal static void ResizeArrays()
	{
		Array.Resize(ref TextureAssets.PlayerHair, Count);
		Array.Resize(ref TextureAssets.PlayerHairAlt, Count);
		Array.Resize(ref HairID.Sets.DrawBackHair, Count);
	}

	internal static void Unload()
	{
		hairs.Clear();
	}
}
