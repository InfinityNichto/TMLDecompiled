using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Default;

/// <summary>
/// Handles conversion of legacy 1.3 world unloaded tile TML TagCompound data to the newer 1.4+ systems.
/// </summary>
[LegacyName(new string[] { "MysteryTilesWorld", "UnloadedTilesSystem" })]
internal class LegacyUnloadedTilesSystem : ModSystem
{
	private struct TileFrame
	{
		private short frameX;

		private short frameY;

		public short FrameX => frameX;

		public short FrameY => frameY;

		public int FrameID
		{
			get
			{
				return frameY * 32768 + frameX;
			}
			set
			{
				frameX = (short)(value % 32768);
				frameY = (short)(value / 32768);
			}
		}

		public TileFrame(int value)
		{
			frameX = 0;
			frameY = 0;
			FrameID = value;
		}

		public TileFrame(short frameX, short frameY)
		{
			this.frameX = frameX;
			this.frameY = frameY;
		}
	}

	private struct TileInfo
	{
		public static readonly TileInfo Invalid = new TileInfo("UnknownMod", "UnknownTile");

		public readonly string modName;

		public readonly string name;

		public readonly bool frameImportant;

		public readonly short frameX;

		public readonly short frameY;

		public TileInfo(string modName, string name)
		{
			this.modName = modName;
			this.name = name;
			frameImportant = false;
			frameX = -1;
			frameY = -1;
		}

		public TileInfo(string modName, string name, short frameX, short frameY)
		{
			this.modName = modName;
			this.name = name;
			this.frameX = frameX;
			this.frameY = frameY;
			frameImportant = true;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is TileInfo other))
			{
				return false;
			}
			if (modName != other.modName || name != other.name || frameImportant != other.frameImportant)
			{
				return false;
			}
			if (frameImportant)
			{
				if (frameX == other.frameX)
				{
					return frameY == other.frameY;
				}
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int hash = name.GetHashCode() + modName.GetHashCode();
			if (frameImportant)
			{
				hash += frameX + frameY;
			}
			return hash;
		}

		public TagCompound Save()
		{
			TagCompound tag = new TagCompound
			{
				["mod"] = modName,
				["name"] = name
			};
			if (frameImportant)
			{
				tag.Set("frameX", frameX);
				tag.Set("frameY", frameY);
			}
			return tag;
		}
	}

	private static readonly List<TileInfo> infos = new List<TileInfo>();

	private static readonly Dictionary<int, ushort> converted = new Dictionary<int, ushort>();

	public override void ClearWorld()
	{
		infos.Clear();
		converted.Clear();
	}

	public override void SaveWorldData(TagCompound tag)
	{
	}

	public override void LoadWorldData(TagCompound tag)
	{
		foreach (TagCompound infoTag in tag.GetList<TagCompound>("list"))
		{
			if (!infoTag.ContainsKey("mod"))
			{
				infos.Add(TileInfo.Invalid);
				continue;
			}
			string modName = infoTag.GetString("mod");
			string name = infoTag.GetString("name");
			TileInfo info = (infoTag.ContainsKey("frameX") ? new TileInfo(modName, name, infoTag.GetShort("frameX"), infoTag.GetShort("frameY")) : new TileInfo(modName, name));
			infos.Add(info);
		}
		if (infos.Count > 0)
		{
			ConvertTiles();
		}
	}

	internal void ConvertTiles()
	{
		List<TileEntry> legacyEntries = TileIO.Tiles.entries.ToList();
		PosData<ushort>.OrderedSparseLookupBuilder builder = new PosData<ushort>.OrderedSparseLookupBuilder();
		PosData<ushort>.OrderedSparseLookupReader unloadedReader = new PosData<ushort>.OrderedSparseLookupReader(TileIO.Tiles.unloadedEntryLookup);
		for (int x = 0; x < Main.maxTilesX; x++)
		{
			for (int y = 0; y < Main.maxTilesY; y++)
			{
				Tile tile = Main.tile[x, y];
				if (!tile.active() || tile.type < TileID.Count)
				{
					continue;
				}
				ushort type = tile.type;
				if (TileIO.Tiles.entries[type] == null)
				{
					type = unloadedReader.Get(x, y);
					if (legacyEntries[type].modName.Equals("ModLoader"))
					{
						ConvertTile(tile, legacyEntries, out type);
					}
					builder.Add(x, y, type);
				}
			}
		}
		TileIO.Tiles.entries = legacyEntries.ToArray();
		TileIO.Tiles.unloadedEntryLookup = builder.Build();
	}

	internal void ConvertTile(Tile tile, List<TileEntry> entries, out ushort type)
	{
		int frameID = new TileFrame(tile.frameX, tile.frameY).FrameID;
		if (!converted.TryGetValue(frameID, out type))
		{
			TileInfo info = infos[frameID];
			TileEntry entry = new TileEntry(TileLoader.GetTile(tile.type))
			{
				name = info.name,
				modName = info.modName,
				frameImportant = (info.frameX > -1),
				type = (type = (ushort)entries.Count)
			};
			entries.Add(entry);
			converted.Add(frameID, type);
		}
	}
}
