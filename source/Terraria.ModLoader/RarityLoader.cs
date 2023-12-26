using System;
using System.Collections.Generic;
using Terraria.GameContent.UI;

namespace Terraria.ModLoader;

public static class RarityLoader
{
	internal static readonly List<ModRarity> rarities = new List<ModRarity>();

	public static int RarityCount => 12 + rarities.Count;

	internal static int Add(ModRarity rarity)
	{
		if (ModNet.AllowVanillaClients)
		{
			throw new Exception("Adding rarities breaks vanilla client compatibility");
		}
		rarities.Add(rarity);
		return RarityCount - 1;
	}

	internal static void FinishSetup()
	{
		ItemRarity.Initialize();
	}

	public static ModRarity GetRarity(int type)
	{
		if (type < 12 || type >= RarityCount)
		{
			return null;
		}
		return rarities[type - 12];
	}

	internal static void Unload()
	{
		rarities.Clear();
		ItemRarity.Initialize();
	}
}
