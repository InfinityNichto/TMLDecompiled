using System;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.ModLoader;

public static class DustLoader
{
	internal static readonly IList<ModDust> dusts = new List<ModDust>();

	internal static int DustCount { get; private set; } = DustID.Count;


	/// <summary>
	/// Gets the ModDust instance with the given type. Returns null if no ModDust with the given type exists.
	/// </summary>
	public static ModDust GetDust(int type)
	{
		if (type < DustID.Count || type >= DustCount)
		{
			return null;
		}
		return dusts[type - DustID.Count];
	}

	internal static int ReserveDustID()
	{
		return DustCount++;
	}

	internal static void ResizeArrays()
	{
		Array.Resize(ref ChildSafety.SafeDust, DustCount);
		for (int i = DustID.Count; i < DustCount; i++)
		{
			ChildSafety.SafeDust[i] = true;
		}
	}

	internal static void Unload()
	{
		dusts.Clear();
		DustCount = DustID.Count;
	}

	internal static void SetupDust(Dust dust)
	{
		ModDust modDust = GetDust(dust.type);
		if (modDust != null)
		{
			dust.frame.X = 0;
			dust.frame.Y %= 30;
			modDust.OnSpawn(dust);
		}
	}

	internal static void SetupUpdateType(Dust dust)
	{
		ModDust modDust = GetDust(dust.type);
		if (modDust != null && modDust.UpdateType >= 0)
		{
			dust.realType = dust.type;
			dust.type = modDust.UpdateType;
		}
	}

	internal static void TakeDownUpdateType(Dust dust)
	{
		if (dust.realType >= 0)
		{
			dust.type = dust.realType;
			dust.realType = -1;
		}
	}
}
