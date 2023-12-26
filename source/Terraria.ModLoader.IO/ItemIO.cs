using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ID;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.Exceptions;

namespace Terraria.ModLoader.IO;

public static class ItemIO
{
	internal static void WriteVanillaID(Item item, BinaryWriter writer)
	{
		writer.Write((item.ModItem == null) ? item.netID : 0);
	}

	internal static void WriteShortVanillaID(Item item, BinaryWriter writer)
	{
		WriteShortVanillaID(item.netID, writer);
	}

	internal static void WriteShortVanillaID(int id, BinaryWriter writer)
	{
		writer.Write((short)((id < ItemID.Count) ? id : 0));
	}

	internal static void WriteShortVanillaStack(Item item, BinaryWriter writer)
	{
		WriteShortVanillaStack(item.stack, writer);
	}

	internal static void WriteShortVanillaStack(int stack, BinaryWriter writer)
	{
		writer.Write((short)((stack > 32767) ? 32767 : stack));
	}

	internal static void WriteByteVanillaPrefix(Item item, BinaryWriter writer)
	{
		WriteByteVanillaPrefix(item.prefix, writer);
	}

	internal static void WriteByteVanillaPrefix(int prefix, BinaryWriter writer)
	{
		writer.Write((byte)((prefix < PrefixID.Count) ? ((uint)prefix) : 0u));
	}

	public static TagCompound Save(Item item)
	{
		return Save(item, SaveGlobals(item));
	}

	public static TagCompound Save(Item item, List<TagCompound> globalData)
	{
		TagCompound tag = new TagCompound();
		if (item.type <= 0)
		{
			return tag;
		}
		if (item.ModItem == null)
		{
			tag.Set("mod", "Terraria");
			tag.Set("id", item.netID);
		}
		else
		{
			tag.Set("mod", item.ModItem.Mod.Name);
			tag.Set("name", item.ModItem.Name);
			TagCompound saveData = new TagCompound();
			item.ModItem.SaveData(saveData);
			if (saveData.Count > 0)
			{
				tag.Set("data", saveData);
			}
		}
		ModPrefix modPrefix = PrefixLoader.GetPrefix(item.prefix);
		if (modPrefix != null)
		{
			tag.Set("modPrefixMod", modPrefix.Mod.Name);
			tag.Set("modPrefixName", modPrefix.Name);
		}
		else if (item.prefix != 0 && item.prefix < PrefixID.Count)
		{
			tag.Set("prefix", (byte)item.prefix);
		}
		if (item.stack > 1)
		{
			tag.Set("stack", item.stack);
		}
		if (item.favorited)
		{
			tag.Set("fav", true);
		}
		tag.Set("globalData", globalData);
		return tag;
	}

	public static void Load(Item item, TagCompound tag)
	{
		string modName = tag.GetString("mod");
		if (modName == "")
		{
			item.netDefaults(0);
			return;
		}
		ModItem modItem;
		if (modName == "Terraria")
		{
			item.netDefaults(tag.GetInt("id"));
		}
		else if (ModContent.TryFind<ModItem>(modName, tag.GetString("name"), out modItem))
		{
			item.SetDefaults(modItem.Type);
			item.ModItem.LoadData(tag.GetCompound("data"));
		}
		else
		{
			item.SetDefaults(ModContent.ItemType<UnloadedItem>());
			((UnloadedItem)item.ModItem).Setup(tag);
		}
		if (tag.ContainsKey("modPrefixMod") && tag.ContainsKey("modPrefixName"))
		{
			item.Prefix(ModContent.TryFind<ModPrefix>(tag.GetString("modPrefixMod"), tag.GetString("modPrefixName"), out var prefix) ? prefix.Type : 0);
		}
		else if (tag.ContainsKey("prefix"))
		{
			item.Prefix(tag.GetByte("prefix"));
		}
		item.stack = tag.Get<int?>("stack") ?? 1;
		item.favorited = tag.GetBool("fav");
		if (!(item.ModItem is UnloadedItem))
		{
			LoadGlobals(item, tag.GetList<TagCompound>("globalData"));
		}
	}

	public static Item Load(TagCompound tag)
	{
		Item item = new Item();
		Load(item, tag);
		return item;
	}

	internal static List<TagCompound> SaveGlobals(Item item)
	{
		if (item.ModItem is UnloadedItem)
		{
			return null;
		}
		List<TagCompound> list = new List<TagCompound>();
		TagCompound saveData = new TagCompound();
		EntityGlobalsEnumerator<GlobalItem> enumerator = ItemLoader.HookSaveData.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			if (g is UnloadedGlobalItem unloadedGlobalItem)
			{
				list.AddRange(unloadedGlobalItem.data);
				continue;
			}
			g.SaveData(item, saveData);
			if (saveData.Count != 0)
			{
				list.Add(new TagCompound
				{
					["mod"] = g.Mod.Name,
					["name"] = g.Name,
					["data"] = saveData
				});
				saveData = new TagCompound();
			}
		}
		if (list.Count <= 0)
		{
			return null;
		}
		return list;
	}

	internal static void LoadGlobals(Item item, IList<TagCompound> list)
	{
		foreach (TagCompound tag in list)
		{
			if (ModContent.TryFind<GlobalItem>(tag.GetString("mod"), tag.GetString("name"), out var globalItemBase) && item.TryGetGlobalItem(globalItemBase, out var globalItem))
			{
				try
				{
					globalItem.LoadData(item, tag.GetCompound("data"));
				}
				catch (Exception e)
				{
					throw new CustomModDataException(globalItem.Mod, "Error in reading custom player data for " + globalItem.FullName, e);
				}
			}
			else
			{
				item.GetGlobalItem<UnloadedGlobalItem>().data.Add(tag);
			}
		}
	}

	public static void Send(Item item, BinaryWriter writer, bool writeStack = false, bool writeFavorite = false)
	{
		writer.Write7BitEncodedInt(item.netID);
		writer.Write7BitEncodedInt(item.prefix);
		if (writeStack)
		{
			writer.Write7BitEncodedInt(item.stack);
		}
		if (writeFavorite)
		{
			writer.Write(item.favorited);
		}
		SendModData(item, writer);
	}

	public static void Receive(Item item, BinaryReader reader, bool readStack = false, bool readFavorite = false)
	{
		item.netDefaults(reader.Read7BitEncodedInt());
		item.Prefix(ModNet.AllowVanillaClients ? reader.ReadByte() : reader.Read7BitEncodedInt());
		if (readStack)
		{
			item.stack = reader.Read7BitEncodedInt();
		}
		if (readFavorite)
		{
			item.favorited = reader.ReadBoolean();
		}
		ReceiveModData(item, reader);
	}

	public static Item Receive(BinaryReader reader, bool readStack = false, bool readFavorite = false)
	{
		Item item = new Item();
		Receive(item, reader, readStack, readFavorite);
		return item;
	}

	public static void SendModData(Item item, BinaryWriter writer)
	{
		if (item.IsAir)
		{
			return;
		}
		writer.SafeWrite(delegate(BinaryWriter w)
		{
			item.ModItem?.NetSend(w);
		});
		EntityGlobalsEnumerator<GlobalItem> enumerator = ItemLoader.HookNetSend.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			writer.SafeWrite(delegate(BinaryWriter w)
			{
				g.NetSend(item, w);
			});
		}
	}

	public static void ReceiveModData(Item item, BinaryReader reader)
	{
		if (item.IsAir)
		{
			return;
		}
		try
		{
			reader.SafeRead(delegate(BinaryReader r)
			{
				item.ModItem?.NetReceive(r);
			});
		}
		catch (IOException e2)
		{
			Logging.tML.Error((object)e2.ToString());
			Logging.tML.Error((object)$"Above IOException error caused by {item.ModItem.Name} from the {item.ModItem.Mod.Name} mod.");
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = ItemLoader.HookNetReceive.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			try
			{
				reader.SafeRead(delegate(BinaryReader r)
				{
					g.NetReceive(item, r);
				});
			}
			catch (IOException e)
			{
				Logging.tML.Error((object)e.ToString());
				Logging.tML.Error((object)$"Above IOException error caused by {g.Name} from the {g.Mod.Name} mod while reading {item.Name}.");
			}
		}
	}

	internal static byte[] LegacyModData(int type, BinaryReader reader, bool hasGlobalSaving = true)
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (BinaryWriter writer = new BinaryWriter(memoryStream))
		{
			if (type >= ItemID.Count)
			{
				ushort length2 = reader.ReadUInt16();
				writer.Write(length2);
				writer.Write(reader.ReadBytes(length2));
			}
			if (hasGlobalSaving)
			{
				ushort count = reader.ReadUInt16();
				writer.Write(count);
				for (int i = 0; i < count; i++)
				{
					writer.Write(reader.ReadString());
					writer.Write(reader.ReadString());
					ushort length = reader.ReadUInt16();
					writer.Write(length);
					writer.Write(reader.ReadBytes(length));
				}
			}
		}
		return memoryStream.ToArray();
	}

	public static string ToBase64(Item item)
	{
		MemoryStream ms = new MemoryStream();
		TagIO.ToStream(Save(item), ms);
		return Convert.ToBase64String(ms.ToArray());
	}

	public static Item FromBase64(string base64)
	{
		return Load(TagIO.FromStream(Convert.FromBase64String(base64).ToMemoryStream()));
	}
}
