using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.DataStructures;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.Exceptions;

namespace Terraria.ModLoader.IO;

internal static class TileIO
{
	internal struct ContainerTables
	{
		internal IDictionary<int, int> headSlots;

		internal IDictionary<int, int> bodySlots;

		internal IDictionary<int, int> legSlots;

		internal static ContainerTables Create()
		{
			ContainerTables result = default(ContainerTables);
			result.headSlots = new Dictionary<int, int>();
			result.bodySlots = new Dictionary<int, int>();
			result.legSlots = new Dictionary<int, int>();
			return result;
		}
	}

	public abstract class IOImpl<TBlock, TEntry> where TBlock : ModBlockType where TEntry : ModEntry
	{
		public readonly string entriesKey;

		public readonly string dataKey;

		public TEntry[] entries;

		public PosData<ushort>[] unloadedEntryLookup;

		public List<ushort> unloadedTypes = new List<ushort>();

		protected abstract int LoadedBlockCount { get; }

		protected abstract IEnumerable<TBlock> LoadedBlocks { get; }

		protected IOImpl(string entriesKey, string dataKey)
		{
			this.entriesKey = entriesKey;
			this.dataKey = dataKey;
		}

		protected abstract TEntry ConvertBlockToEntry(TBlock block);

		private List<TEntry> CreateEntries()
		{
			List<TEntry> entries = Enumerable.Repeat<TEntry>(null, LoadedBlockCount).ToList();
			foreach (TBlock block in LoadedBlocks)
			{
				if (!unloadedTypes.Contains(block.Type))
				{
					entries[block.Type] = ConvertBlockToEntry(block);
				}
			}
			return entries;
		}

		public void LoadEntries(TagCompound tag, out TEntry[] savedEntryLookup)
		{
			IList<TEntry> savedEntryList = tag.GetList<TEntry>(entriesKey);
			List<TEntry> entries = CreateEntries();
			if (savedEntryList.Count == 0)
			{
				savedEntryLookup = null;
			}
			else
			{
				savedEntryLookup = new TEntry[savedEntryList.Max((TEntry e) => e.type) + 1];
				foreach (TEntry entry in savedEntryList)
				{
					if (ModContent.TryFind<TBlock>(entry.modName, entry.name, out var block))
					{
						savedEntryLookup[entry.type] = entries[block.Type];
						continue;
					}
					savedEntryLookup[entry.type] = entry;
					entry.type = (ushort)entries.Count;
					entry.loadedType = (canPurgeOldData ? entry.vanillaReplacementType : ModContent.Find<TBlock>(string.IsNullOrEmpty(entry.unloadedType) ? entry.DefaultUnloadedType : entry.unloadedType).Type);
					entries.Add(entry);
				}
			}
			this.entries = entries.ToArray();
		}

		protected abstract void ReadData(Tile tile, TEntry entry, BinaryReader reader);

		public void LoadData(TagCompound tag, TEntry[] savedEntryLookup)
		{
			if (!tag.ContainsKey(dataKey))
			{
				return;
			}
			using BinaryReader reader = new BinaryReader(tag.GetByteArray(dataKey).ToMemoryStream());
			PosData<ushort>.OrderedSparseLookupBuilder builder = new PosData<ushort>.OrderedSparseLookupBuilder();
			for (int x = 0; x < Main.maxTilesX; x++)
			{
				for (int y = 0; y < Main.maxTilesY; y++)
				{
					ushort saveType = reader.ReadUInt16();
					if (saveType != 0)
					{
						TEntry entry = savedEntryLookup[saveType];
						if (entry.IsUnloaded && !canPurgeOldData)
						{
							builder.Add(x, y, entry.type);
						}
						ReadData(Main.tile[x, y], entry, reader);
					}
				}
			}
			unloadedEntryLookup = builder.Build();
		}

		public void Save(TagCompound tag)
		{
			if (entries == null)
			{
				entries = CreateEntries().ToArray();
			}
			tag[dataKey] = SaveData(out var hasBlocks);
			tag[entriesKey] = SelectEntries(hasBlocks, entries).ToList();
		}

		private IEnumerable<TEntry> SelectEntries(bool[] select, TEntry[] entries)
		{
			for (int i = 0; i < select.Length; i++)
			{
				if (select[i])
				{
					yield return entries[i];
				}
			}
		}

		protected abstract ushort GetModBlockType(Tile tile);

		protected abstract void WriteData(BinaryWriter writer, Tile tile, TEntry entry);

		public byte[] SaveData(out bool[] hasObj)
		{
			using MemoryStream ms = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(ms);
			PosData<ushort>.OrderedSparseLookupReader unloadedReader = new PosData<ushort>.OrderedSparseLookupReader(unloadedEntryLookup);
			hasObj = new bool[entries.Length];
			for (int x = 0; x < Main.maxTilesX; x++)
			{
				for (int y = 0; y < Main.maxTilesY; y++)
				{
					Tile tile = Main.tile[x, y];
					int type = GetModBlockType(tile);
					if (type == 0)
					{
						writer.Write((ushort)0);
						continue;
					}
					if (entries[type] == null)
					{
						type = unloadedReader.Get(x, y);
					}
					hasObj[type] = true;
					WriteData(writer, tile, entries[type]);
				}
			}
			return ms.ToArray();
		}

		public void Clear()
		{
			entries = null;
			unloadedEntryLookup = null;
		}
	}

	public class TileIOImpl : IOImpl<ModTile, TileEntry>
	{
		protected override int LoadedBlockCount => TileLoader.TileCount;

		protected override IEnumerable<ModTile> LoadedBlocks => TileLoader.tiles;

		public TileIOImpl()
			: base("tileMap", "tileData")
		{
		}

		protected override TileEntry ConvertBlockToEntry(ModTile tile)
		{
			return new TileEntry(tile);
		}

		protected override ushort GetModBlockType(Tile tile)
		{
			if (!tile.active() || tile.type < TileID.Count)
			{
				return 0;
			}
			return tile.type;
		}

		protected override void ReadData(Tile tile, TileEntry entry, BinaryReader reader)
		{
			tile.type = entry.loadedType;
			tile.color(reader.ReadByte());
			tile.active(active: true);
			if (entry.frameImportant)
			{
				tile.frameX = reader.ReadInt16();
				tile.frameY = reader.ReadInt16();
			}
		}

		protected override void WriteData(BinaryWriter writer, Tile tile, TileEntry entry)
		{
			writer.Write(entry.type);
			writer.Write(tile.color());
			if (entry.frameImportant)
			{
				writer.Write(tile.frameX);
				writer.Write(tile.frameY);
			}
		}
	}

	public class WallIOImpl : IOImpl<ModWall, WallEntry>
	{
		protected override int LoadedBlockCount => WallLoader.WallCount;

		protected override IEnumerable<ModWall> LoadedBlocks => WallLoader.walls;

		public WallIOImpl()
			: base("wallMap", "wallData")
		{
		}

		protected override WallEntry ConvertBlockToEntry(ModWall wall)
		{
			return new WallEntry(wall);
		}

		protected override ushort GetModBlockType(Tile tile)
		{
			if (tile.wall < WallID.Count)
			{
				return 0;
			}
			return tile.wall;
		}

		protected override void ReadData(Tile tile, WallEntry entry, BinaryReader reader)
		{
			tile.wall = entry.loadedType;
			tile.wallColor(reader.ReadByte());
		}

		protected override void WriteData(BinaryWriter writer, Tile tile, WallEntry entry)
		{
			writer.Write(entry.type);
			writer.Write(tile.wallColor());
		}
	}

	internal static class TileIOFlags
	{
		internal const byte None = 0;

		internal const byte ModTile = 1;

		internal const byte FrameXInt16 = 2;

		internal const byte FrameYInt16 = 4;

		internal const byte TileColor = 8;

		internal const byte ModWall = 16;

		internal const byte WallColor = 32;

		internal const byte NextTilesAreSame = 64;

		internal const byte NextModTile = 128;
	}

	internal static TileIOImpl Tiles = new TileIOImpl();

	internal static WallIOImpl Walls = new WallIOImpl();

	internal static bool canPurgeOldData => false;

	internal static void VanillaSaveFrames(Tile tile, ref short frameX)
	{
		if (tile.type == 128 || tile.type == 269)
		{
			int slot = tile.frameX / 100;
			int position = tile.frameY / 18;
			if (HasModArmor(slot, position))
			{
				frameX %= 100;
			}
		}
	}

	internal static TagCompound SaveContainers()
	{
		MemoryStream ms = new MemoryStream();
		BinaryWriter writer = new BinaryWriter(ms);
		byte[] flags = new byte[1];
		byte numFlags = 0;
		ISet<int> headSlots = new HashSet<int>();
		ISet<int> bodySlots = new HashSet<int>();
		ISet<int> legSlots = new HashSet<int>();
		for (int i = 0; i < Main.maxTilesX; i++)
		{
			for (int j = 0; j < Main.maxTilesY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (!tile.active() || (tile.type != 128 && tile.type != 269))
				{
					continue;
				}
				int slot4 = tile.frameX / 100;
				int position = tile.frameY / 18;
				if (HasModArmor(slot4, position))
				{
					switch (position)
					{
					case 0:
						headSlots.Add(slot4);
						break;
					case 1:
						bodySlots.Add(slot4);
						break;
					case 2:
						legSlots.Add(slot4);
						break;
					}
					flags[0] |= 1;
					numFlags = 1;
				}
			}
		}
		int tileEntity = 0;
		List<TagCompound> itemFrames = new List<TagCompound>();
		foreach (KeyValuePair<int, TileEntity> entity in TileEntity.ByID)
		{
			if (entity.Value is TEItemFrame itemFrame)
			{
				List<TagCompound> globalData = ItemIO.SaveGlobals(itemFrame.item);
				if (globalData != null || ItemLoader.NeedsModSaving(itemFrame.item))
				{
					itemFrames.Add(new TagCompound
					{
						["id"] = tileEntity,
						["item"] = ItemIO.Save(itemFrame.item, globalData)
					});
					numFlags = 1;
				}
			}
			if (!(entity.Value is ModTileEntity))
			{
				tileEntity++;
			}
		}
		if (numFlags == 0)
		{
			return null;
		}
		writer.Write(numFlags);
		writer.Write(flags, 0, numFlags);
		if ((flags[0] & 1) == 1)
		{
			writer.Write((ushort)headSlots.Count);
			foreach (int slot3 in headSlots)
			{
				writer.Write((ushort)slot3);
				ModItem item3 = ItemLoader.GetItem(EquipLoader.slotToId[EquipType.Head][slot3]);
				writer.Write(item3.Mod.Name);
				writer.Write(item3.Name);
			}
			writer.Write((ushort)bodySlots.Count);
			foreach (int slot2 in bodySlots)
			{
				writer.Write((ushort)slot2);
				ModItem item2 = ItemLoader.GetItem(EquipLoader.slotToId[EquipType.Body][slot2]);
				writer.Write(item2.Mod.Name);
				writer.Write(item2.Name);
			}
			writer.Write((ushort)legSlots.Count);
			foreach (int slot in legSlots)
			{
				writer.Write((ushort)slot);
				ModItem item = ItemLoader.GetItem(EquipLoader.slotToId[EquipType.Legs][slot]);
				writer.Write(item.Mod.Name);
				writer.Write(item.Name);
			}
			WriteContainerData(writer);
		}
		TagCompound tag = new TagCompound();
		tag.Set("data", ms.ToArray());
		if (itemFrames.Count > 0)
		{
			tag.Set("itemFrames", itemFrames);
		}
		return tag;
	}

	internal static void LoadContainers(TagCompound tag)
	{
		if (tag.ContainsKey("data"))
		{
			ReadContainers(new BinaryReader(tag.GetByteArray("data").ToMemoryStream()));
		}
		foreach (TagCompound frameTag in tag.GetList<TagCompound>("itemFrames"))
		{
			if (TileEntity.ByID.TryGetValue(frameTag.GetInt("id"), out var tileEntity) && tileEntity is TEItemFrame itemFrame)
			{
				ItemIO.Load(itemFrame.item, frameTag.GetCompound("item"));
			}
			else
			{
				Logging.tML.Warn((object)("Due to a bug in previous versions of tModLoader, the following ItemFrame data has been lost: " + frameTag.ToString()));
			}
		}
	}

	internal static void ReadContainers(BinaryReader reader)
	{
		byte[] flags = new byte[1];
		reader.Read(flags, 0, reader.ReadByte());
		if ((flags[0] & 1) == 1)
		{
			ContainerTables tables = ContainerTables.Create();
			int count = reader.ReadUInt16();
			for (int k = 0; k < count; k++)
			{
				tables.headSlots[reader.ReadUInt16()] = (ModContent.TryFind<ModItem>(reader.ReadString(), reader.ReadString(), out var item3) ? item3.Item.headSlot : 0);
			}
			count = reader.ReadUInt16();
			for (int j = 0; j < count; j++)
			{
				tables.bodySlots[reader.ReadUInt16()] = (ModContent.TryFind<ModItem>(reader.ReadString(), reader.ReadString(), out var item2) ? item2.Item.bodySlot : 0);
			}
			count = reader.ReadUInt16();
			for (int i = 0; i < count; i++)
			{
				tables.legSlots[reader.ReadUInt16()] = (ModContent.TryFind<ModItem>(reader.ReadString(), reader.ReadString(), out var item) ? item.Item.legSlot : 0);
			}
			ReadContainerData(reader, tables);
		}
	}

	internal static void WriteContainerData(BinaryWriter writer)
	{
		for (int i = 0; i < Main.maxTilesX; i++)
		{
			for (int j = 0; j < Main.maxTilesY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.active() && (tile.type == 128 || tile.type == 269))
				{
					int slot = tile.frameX / 100;
					int frameX = tile.frameX % 100;
					int position = tile.frameY / 18;
					if (HasModArmor(slot, position) && frameX % 36 == 0)
					{
						writer.Write(i);
						writer.Write(j);
						writer.Write((byte)position);
						writer.Write((ushort)slot);
					}
				}
			}
		}
		writer.Write(-1);
	}

	internal static void ReadContainerData(BinaryReader reader, ContainerTables tables)
	{
		for (int i = reader.ReadInt32(); i > 0; i = reader.ReadInt32())
		{
			int j = reader.ReadInt32();
			int position = reader.ReadByte();
			int slot = reader.ReadUInt16();
			Tile left = Main.tile[i, j];
			Tile right = Main.tile[i + 1, j];
			if (left.active() && right.active() && (left.type == 128 || left.type == 269) && left.type == right.type && (left.frameX == 0 || left.frameX == 36) && right.frameX == left.frameX + 18 && left.frameY / 18 == position && left.frameY == right.frameY)
			{
				switch (position)
				{
				case 0:
					slot = tables.headSlots[slot];
					break;
				case 1:
					slot = tables.bodySlots[slot];
					break;
				case 2:
					slot = tables.legSlots[slot];
					break;
				}
				left.frameX += (short)(100 * slot);
			}
		}
	}

	private static bool HasModArmor(int slot, int position)
	{
		return position switch
		{
			0 => slot >= ArmorIDs.Head.Count, 
			1 => slot >= ArmorIDs.Body.Count, 
			2 => slot >= ArmorIDs.Legs.Count, 
			_ => false, 
		};
	}

	internal static void LoadBasics(TagCompound tag)
	{
		Tiles.LoadEntries(tag, out var tileEntriesLookup);
		Walls.LoadEntries(tag, out var wallEntriesLookup);
		if (!tag.ContainsKey("wallData"))
		{
			LoadLegacy(tag, tileEntriesLookup, wallEntriesLookup);
		}
		else
		{
			Tiles.LoadData(tag, tileEntriesLookup);
			Walls.LoadData(tag, wallEntriesLookup);
		}
		WorldIO.ValidateSigns();
	}

	internal static TagCompound SaveBasics()
	{
		TagCompound tag = new TagCompound();
		Tiles.Save(tag);
		Walls.Save(tag);
		return tag;
	}

	internal static void ResetUnloadedTypes()
	{
		Tiles.unloadedTypes.Clear();
		Walls.unloadedTypes.Clear();
	}

	internal static void ClearWorld()
	{
		Tiles.Clear();
		Walls.Clear();
	}

	internal static void LoadLegacy(TagCompound tag, TileEntry[] tileEntriesLookup, WallEntry[] wallEntriesLookup)
	{
		if (!tag.ContainsKey("data"))
		{
			return;
		}
		using BinaryReader reader = new BinaryReader(tag.GetByteArray("data").ToMemoryStream());
		ReadTileData(reader, tileEntriesLookup, wallEntriesLookup, out var tilePosMapList, out var wallPosMapList);
		Tiles.unloadedEntryLookup = tilePosMapList.ToArray();
		Walls.unloadedEntryLookup = wallPosMapList.ToArray();
	}

	internal static void ReadTileData(BinaryReader reader, TileEntry[] tileEntriesLookup, WallEntry[] wallEntriesLookup, out List<PosData<ushort>> wallPosMapList, out List<PosData<ushort>> tilePosMapList)
	{
		int i = 0;
		int j = 0;
		bool nextModTile = false;
		tilePosMapList = new List<PosData<ushort>>();
		wallPosMapList = new List<PosData<ushort>>();
		do
		{
			if (!nextModTile)
			{
				byte skip;
				for (skip = reader.ReadByte(); skip == byte.MaxValue; skip = reader.ReadByte())
				{
					for (byte l = 0; l < byte.MaxValue; l++)
					{
						if (!NextLocation(ref i, ref j))
						{
							return;
						}
					}
				}
				for (byte k = 0; k < skip; k++)
				{
					if (!NextLocation(ref i, ref j))
					{
						return;
					}
				}
			}
			else
			{
				nextModTile = false;
			}
			ReadModTile(ref i, ref j, reader, ref nextModTile, tilePosMapList, wallPosMapList, tileEntriesLookup, wallEntriesLookup);
		}
		while (NextLocation(ref i, ref j));
	}

	internal static void ReadModTile(ref int i, ref int j, BinaryReader reader, ref bool nextModTile, List<PosData<ushort>> wallPosMapList, List<PosData<ushort>> tilePosMapList, TileEntry[] tileEntriesLookup, WallEntry[] wallEntriesLookup)
	{
		byte flags = reader.ReadByte();
		Tile tile = Main.tile[i, j];
		if ((flags & 1) == 1)
		{
			tile.active(active: true);
			ushort saveType = reader.ReadUInt16();
			TileEntry tEntry = tileEntriesLookup[saveType];
			tile.type = tEntry.loadedType;
			if (tEntry.frameImportant)
			{
				if ((flags & 2) == 2)
				{
					tile.frameX = reader.ReadInt16();
				}
				else
				{
					tile.frameX = reader.ReadByte();
				}
				if ((flags & 4) == 4)
				{
					tile.frameY = reader.ReadInt16();
				}
				else
				{
					tile.frameY = reader.ReadByte();
				}
			}
			else
			{
				tile.frameX = -1;
				tile.frameY = -1;
			}
			if ((flags & 8) == 8)
			{
				tile.color(reader.ReadByte());
			}
			if (tEntry.IsUnloaded)
			{
				tilePosMapList.Add(new PosData<ushort>(PosData.CoordsToPos(i, j), tEntry.type));
			}
			WorldGen.tileCounts[tile.type] += ((!((double)j <= Main.worldSurface)) ? 1 : 5);
		}
		if ((flags & 0x10) == 16)
		{
			ushort saveType = reader.ReadUInt16();
			WallEntry wEntry = wallEntriesLookup[saveType];
			tile.wall = wEntry.loadedType;
			if (wEntry.IsUnloaded)
			{
				wallPosMapList.Add(new PosData<ushort>(PosData.CoordsToPos(i, j), wEntry.type));
			}
			if ((flags & 0x20) == 32)
			{
				tile.wallColor(reader.ReadByte());
			}
		}
		if ((flags & 0x40) == 64)
		{
			byte sameCount = reader.ReadByte();
			for (byte k = 0; k < sameCount; k++)
			{
				NextLocation(ref i, ref j);
				Main.tile[i, j].CopyFrom(tile);
				WorldGen.tileCounts[tile.type] += ((!((double)j <= Main.worldSurface)) ? 1 : 5);
			}
		}
		if ((flags & 0x80) == 128)
		{
			nextModTile = true;
		}
	}

	/// <summary>
	/// Increases the provided x and y coordinates to the next location in accordance with order-sensitive position IDs.
	/// Typically used in clustering duplicate data across multiple consecutive locations, such as in ModLoader.TileIO
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns> False if x and y cannot be increased further (end of the world)  </returns>
	private static bool NextLocation(ref int x, ref int y)
	{
		y++;
		if (y >= Main.maxTilesY)
		{
			y = 0;
			x++;
			if (x >= Main.maxTilesX)
			{
				return false;
			}
		}
		return true;
	}

	internal static List<TagCompound> SaveTileEntities()
	{
		List<TagCompound> list = new List<TagCompound>();
		TagCompound saveData = new TagCompound();
		foreach (KeyValuePair<int, TileEntity> item in TileEntity.ByID)
		{
			TileEntity tileEntity = item.Value;
			ModTileEntity modTileEntity = tileEntity as ModTileEntity;
			tileEntity.SaveData(saveData);
			TagCompound tag = new TagCompound
			{
				["mod"] = modTileEntity?.Mod.Name ?? "Terraria",
				["name"] = modTileEntity?.Name ?? tileEntity.GetType().Name,
				["X"] = tileEntity.Position.X,
				["Y"] = tileEntity.Position.Y
			};
			if (saveData.Count != 0)
			{
				tag["data"] = saveData;
				saveData = new TagCompound();
			}
			list.Add(tag);
		}
		return list;
	}

	internal static void LoadTileEntities(IList<TagCompound> list)
	{
		foreach (TagCompound tag in list)
		{
			string modName = tag.GetString("mod");
			string name = tag.GetString("name");
			Point16 point = new Point16(tag.GetShort("X"), tag.GetShort("Y"));
			ModTileEntity baseModTileEntity = null;
			bool foundTE = true;
			TileEntity tileEntity;
			if (modName != "Terraria")
			{
				if (!ModContent.TryFind<ModTileEntity>(modName, name, out baseModTileEntity))
				{
					foundTE = false;
					baseModTileEntity = ModContent.GetInstance<UnloadedTileEntity>();
				}
				tileEntity = ModTileEntity.ConstructFromBase(baseModTileEntity);
				tileEntity.type = (byte)baseModTileEntity.Type;
				tileEntity.Position = point;
				if (!foundTE)
				{
					(tileEntity as UnloadedTileEntity)?.SetData(tag);
				}
			}
			else if (!TileEntity.ByPosition.TryGetValue(point, out tileEntity))
			{
				continue;
			}
			if (tag.ContainsKey("data"))
			{
				try
				{
					if (foundTE)
					{
						tileEntity.LoadData(tag.GetCompound("data"));
					}
					if (tileEntity is UnloadedTileEntity unloadedTE && unloadedTE.TryRestore(out var restoredTE))
					{
						tileEntity = restoredTE;
					}
				}
				catch (Exception e)
				{
					throw new CustomModDataException((tileEntity as ModTileEntity)?.Mod, "Error in reading " + name + " tile entity data for " + modName, e);
				}
			}
			if (baseModTileEntity != null && tileEntity.IsTileValidForEntity(tileEntity.Position.X, tileEntity.Position.Y))
			{
				tileEntity.ID = TileEntity.AssignNewID();
				TileEntity.ByID[tileEntity.ID] = tileEntity;
				if (TileEntity.ByPosition.TryGetValue(tileEntity.Position, out var other))
				{
					TileEntity.ByID.Remove(other.ID);
				}
				TileEntity.ByPosition[tileEntity.Position] = tileEntity;
			}
		}
	}
}
