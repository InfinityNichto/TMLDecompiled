using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Map;

namespace Terraria.ModLoader;

internal static class MapLoader
{
	internal static bool initialized = false;

	internal static readonly IDictionary<ushort, IList<MapEntry>> tileEntries = new Dictionary<ushort, IList<MapEntry>>();

	internal static readonly IDictionary<ushort, IList<MapEntry>> wallEntries = new Dictionary<ushort, IList<MapEntry>>();

	internal static readonly IDictionary<ushort, Func<string, int, int, string>> nameFuncs = new Dictionary<ushort, Func<string, int, int, string>>();

	internal static readonly IDictionary<ushort, ushort> entryToTile = new Dictionary<ushort, ushort>();

	internal static readonly IDictionary<ushort, ushort> entryToWall = new Dictionary<ushort, ushort>();

	internal static int modTileOptions(ushort type)
	{
		return tileEntries[type].Count;
	}

	internal static int modWallOptions(ushort type)
	{
		return wallEntries[type].Count;
	}

	internal static void FinishSetup()
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		if (Main.dedServ)
		{
			return;
		}
		Array.Resize(ref MapHelper.tileLookup, TileLoader.TileCount);
		Array.Resize(ref MapHelper.wallLookup, WallLoader.WallCount);
		IList<Color> colors = new List<Color>();
		IList<LocalizedText> names = new List<LocalizedText>();
		foreach (ushort type2 in tileEntries.Keys)
		{
			MapHelper.tileLookup[type2] = (ushort)(MapHelper.modPosition + colors.Count);
			foreach (MapEntry entry2 in tileEntries[type2])
			{
				ushort mapType2 = (ushort)(MapHelper.modPosition + colors.Count);
				entryToTile[mapType2] = type2;
				nameFuncs[mapType2] = entry2.getName;
				colors.Add(entry2.color);
				if (entry2.name != null)
				{
					names.Add(entry2.name);
					continue;
				}
				throw new Exception("How did this happen?");
			}
		}
		foreach (ushort type in wallEntries.Keys)
		{
			MapHelper.wallLookup[type] = (ushort)(MapHelper.modPosition + colors.Count);
			foreach (MapEntry entry in wallEntries[type])
			{
				ushort mapType = (ushort)(MapHelper.modPosition + colors.Count);
				entryToWall[mapType] = type;
				nameFuncs[mapType] = entry.getName;
				colors.Add(entry.color);
				if (entry.name != null)
				{
					names.Add(entry.name);
					continue;
				}
				throw new Exception("How did this happen?");
			}
		}
		Array.Resize(ref MapHelper.colorLookup, MapHelper.modPosition + colors.Count);
		Lang._mapLegendCache.Resize(MapHelper.modPosition + names.Count);
		for (int i = 0; i < colors.Count; i++)
		{
			MapHelper.colorLookup[MapHelper.modPosition + i] = colors[i];
			Lang._mapLegendCache[MapHelper.modPosition + i] = names[i];
		}
		initialized = true;
	}

	internal static void UnloadModMap()
	{
		tileEntries.Clear();
		wallEntries.Clear();
		if (!Main.dedServ)
		{
			nameFuncs.Clear();
			entryToTile.Clear();
			entryToWall.Clear();
			Array.Resize(ref MapHelper.tileLookup, TileID.Count);
			Array.Resize(ref MapHelper.wallLookup, WallID.Count);
			Array.Resize(ref MapHelper.colorLookup, MapHelper.modPosition);
			Lang._mapLegendCache.Resize(MapHelper.modPosition);
			initialized = false;
		}
	}

	internal static void ModMapOption(ref ushort mapType, int i, int j)
	{
		if (entryToTile.ContainsKey(mapType))
		{
			ModTile tile = TileLoader.GetTile(entryToTile[mapType]);
			ushort option = tile.GetMapOption(i, j);
			if (option < 0 || option >= modTileOptions(tile.Type))
			{
				throw new ArgumentOutOfRangeException("Bad map option for tile " + tile.Name + " from mod " + tile.Mod.Name);
			}
			mapType += option;
		}
		else if (entryToWall.ContainsKey(mapType))
		{
			ModWall wall = WallLoader.GetWall(entryToWall[mapType]);
			ushort option2 = wall.GetMapOption(i, j);
			if (option2 < 0 || option2 >= modWallOptions(wall.Type))
			{
				throw new ArgumentOutOfRangeException("Bad map option for wall " + wall.Name + " from mod " + wall.Mod.Name);
			}
			mapType += option2;
		}
	}
}
