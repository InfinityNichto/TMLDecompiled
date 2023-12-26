using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Microsoft.Xna.Framework;
using ReLogic.OS;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.Exceptions;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria.ModLoader.IO;

internal static class WorldIO
{
	public static CustomModDataException customDataFail;

	internal static void Save(string path, bool isCloudSave)
	{
		path = Path.ChangeExtension(path, ".twld");
		if (FileUtilities.Exists(path, isCloudSave))
		{
			FileUtilities.Copy(path, path + ".bak", isCloudSave);
		}
		TagCompound tag = new TagCompound
		{
			["0header"] = SaveHeader(),
			["chests"] = SaveChestInventory(),
			["tiles"] = TileIO.SaveBasics(),
			["containers"] = TileIO.SaveContainers(),
			["npcs"] = SaveNPCs(),
			["tileEntities"] = TileIO.SaveTileEntities(),
			["killCounts"] = SaveNPCKillCounts(),
			["bestiaryKills"] = SaveNPCBestiaryKills(),
			["bestiarySights"] = SaveNPCBestiarySights(),
			["bestiaryChats"] = SaveNPCBestiaryChats(),
			["anglerQuest"] = SaveAnglerQuest(),
			["townManager"] = SaveTownManager(),
			["modData"] = SaveModData(),
			["alteredVanillaFields"] = SaveAlteredVanillaFields()
		};
		FileUtilities.WriteTagCompound(path, isCloudSave, tag);
	}

	internal static void Load(string path, bool isCloudSave)
	{
		customDataFail = null;
		path = Path.ChangeExtension(path, ".twld");
		if (FileUtilities.Exists(path, isCloudSave))
		{
			byte[] buf = FileUtilities.ReadAllBytes(path, isCloudSave);
			if (buf[0] != 31 || buf[1] != 139)
			{
				throw new IOException(Path.GetFileName(path) + ":: File Corrupted during Last Save Step. Aborting... ERROR: Missing NBT Header");
			}
			TagCompound tag = TagIO.FromStream(buf.ToMemoryStream());
			TileIO.LoadBasics(tag.GetCompound("tiles"));
			TileIO.LoadContainers(tag.GetCompound("containers"));
			LoadNPCs(tag.GetList<TagCompound>("npcs"));
			try
			{
				TileIO.LoadTileEntities(tag.GetList<TagCompound>("tileEntities"));
			}
			catch (CustomModDataException ex)
			{
				customDataFail = ex;
				throw;
			}
			LoadChestInventory(tag.GetList<TagCompound>("chests"));
			LoadNPCKillCounts(tag.GetList<TagCompound>("killCounts"));
			LoadNPCBestiaryKills(tag.GetList<TagCompound>("bestiaryKills"));
			LoadNPCBestiarySights(tag.GetList<TagCompound>("bestiarySights"));
			LoadNPCBestiaryChats(tag.GetList<TagCompound>("bestiaryChats"));
			LoadAnglerQuest(tag.GetCompound("anglerQuest"));
			LoadTownManager(tag.GetList<TagCompound>("townManager"));
			try
			{
				LoadModData(tag.GetList<TagCompound>("modData"));
			}
			catch (CustomModDataException ex2)
			{
				customDataFail = ex2;
				throw;
			}
			LoadAlteredVanillaFields(tag.GetCompound("alteredVanillaFields"));
		}
	}

	internal static List<TagCompound> SaveChestInventory()
	{
		List<TagCompound> list = new List<TagCompound>();
		for (int i = 0; i < 8000; i++)
		{
			Chest chest = Main.chest[i];
			if (chest != null)
			{
				List<TagCompound> itemTagListModded = PlayerIO.SaveInventory(chest.item);
				if (itemTagListModded != null)
				{
					TagCompound tag = new TagCompound
					{
						["items"] = itemTagListModded,
						["x"] = chest.x,
						["y"] = chest.y
					};
					list.Add(tag);
				}
			}
		}
		return list;
	}

	internal static void LoadChestInventory(IList<TagCompound> list)
	{
		foreach (TagCompound tag in list)
		{
			int cID = Chest.FindChest(tag.GetInt("x"), tag.GetInt("y"));
			if (cID >= 0)
			{
				PlayerIO.LoadInventory(Main.chest[cID].item, tag.GetList<TagCompound>("items"));
			}
		}
	}

	internal static List<TagCompound> SaveNPCs()
	{
		List<TagCompound> list = new List<TagCompound>();
		TagCompound data = new TagCompound();
		for (int index = 0; index < Main.maxNPCs; index++)
		{
			NPC npc = Main.npc[index];
			if (!npc.active || !NPCLoader.SavesAndLoads(npc))
			{
				continue;
			}
			List<TagCompound> globalData = new List<TagCompound>();
			EntityGlobalsEnumerator<GlobalNPC> enumerator = NPCLoader.HookSaveData.Enumerate(npc).GetEnumerator();
			while (enumerator.MoveNext())
			{
				GlobalNPC g = enumerator.Current;
				if (g is UnloadedGlobalNPC unloadedGlobalNPC)
				{
					globalData.AddRange(unloadedGlobalNPC.data);
					continue;
				}
				g.SaveData(npc, data);
				if (data.Count != 0)
				{
					globalData.Add(new TagCompound
					{
						["mod"] = g.Mod.Name,
						["name"] = g.Name,
						["data"] = data
					});
					data = new TagCompound();
				}
			}
			TagCompound tag;
			if (NPCLoader.IsModNPC(npc))
			{
				npc.ModNPC.SaveData(data);
				tag = new TagCompound
				{
					["mod"] = npc.ModNPC.Mod.Name,
					["name"] = npc.ModNPC.Name
				};
				if (data.Count != 0)
				{
					tag["data"] = data;
					data = new TagCompound();
				}
				if (npc.townNPC)
				{
					tag["displayName"] = npc.GivenName;
					tag["homeless"] = npc.homeless;
					tag["homeTileX"] = npc.homeTileX;
					tag["homeTileY"] = npc.homeTileY;
					tag["isShimmered"] = NPC.ShimmeredTownNPCs[npc.type];
					tag["npcTownVariationIndex"] = npc.townNpcVariationIndex;
				}
			}
			else
			{
				if (globalData.Count == 0)
				{
					continue;
				}
				tag = new TagCompound
				{
					["mod"] = "Terraria",
					["name"] = NPCID.Search.GetName(npc.type)
				};
			}
			tag["x"] = npc.position.X;
			tag["y"] = npc.position.Y;
			tag["globalData"] = globalData;
			list.Add(tag);
		}
		return list;
	}

	internal static void LoadNPCs(IList<TagCompound> list)
	{
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		if (list == null)
		{
			return;
		}
		int nextFreeNPC = 0;
		foreach (TagCompound tag in list)
		{
			NPC npc = null;
			for (; nextFreeNPC < Main.maxNPCs && Main.npc[nextFreeNPC].active; nextFreeNPC++)
			{
			}
			if (tag.GetString("mod") == "Terraria")
			{
				int npcId = NPCID.Search.GetId(tag.GetString("name"));
				float x = tag.GetFloat("x");
				float y = tag.GetFloat("y");
				int index;
				for (index = 0; index < Main.maxNPCs; index++)
				{
					npc = Main.npc[index];
					if (npc.active && npc.type == npcId && npc.position.X == x && npc.position.Y == y)
					{
						break;
					}
				}
				if (index == Main.maxNPCs)
				{
					if (nextFreeNPC == Main.maxNPCs)
					{
						ModContent.GetInstance<UnloadedSystem>().unloadedNPCs.Add(tag);
						continue;
					}
					npc = Main.npc[nextFreeNPC];
					npc.SetDefaults(npcId);
					npc.position = new Vector2(x, y);
				}
			}
			else
			{
				if (!ModContent.TryFind<ModNPC>(tag.GetString("mod"), tag.GetString("name"), out var modNpc))
				{
					ModContent.GetInstance<UnloadedSystem>().unloadedNPCs.Add(tag);
					continue;
				}
				if (nextFreeNPC == Main.maxNPCs)
				{
					ModContent.GetInstance<UnloadedSystem>().unloadedNPCs.Add(tag);
					continue;
				}
				npc = Main.npc[nextFreeNPC];
				npc.SetDefaults(modNpc.Type);
				npc.position.X = tag.GetFloat("x");
				npc.position.Y = tag.GetFloat("y");
				if (npc.townNPC)
				{
					npc.GivenName = tag.GetString("displayName");
					npc.homeless = tag.GetBool("homeless");
					npc.homeTileX = tag.GetInt("homeTileX");
					npc.homeTileY = tag.GetInt("homeTileY");
					NPC.ShimmeredTownNPCs[modNpc.Type] = tag.GetBool("isShimmered");
					npc.townNpcVariationIndex = tag.GetInt("npcTownVariationIndex");
				}
				if (tag.ContainsKey("data"))
				{
					npc.ModNPC.LoadData((TagCompound)tag["data"]);
				}
			}
			LoadGlobals(npc, tag.GetList<TagCompound>("globalData"));
		}
	}

	private static void LoadGlobals(NPC npc, IList<TagCompound> list)
	{
		foreach (TagCompound tag in list)
		{
			if (ModContent.TryFind<GlobalNPC>(tag.GetString("mod"), tag.GetString("name"), out var globalNPCBase) && npc.TryGetGlobalNPC(globalNPCBase, out var globalNPC))
			{
				try
				{
					globalNPC.LoadData(npc, tag.GetCompound("data"));
				}
				catch (Exception inner)
				{
					throw new CustomModDataException(globalNPC.Mod, "Error in reading custom player data for " + tag.GetString("mod"), inner);
				}
			}
			else
			{
				npc.GetGlobalNPC<UnloadedGlobalNPC>().data.Add(tag);
			}
		}
	}

	internal static List<TagCompound> SaveNPCKillCounts()
	{
		List<TagCompound> list = new List<TagCompound>();
		for (int type = NPCID.Count; type < NPCLoader.NPCCount; type++)
		{
			int killCount = NPC.killCount[type];
			if (killCount > 0)
			{
				ModNPC modNPC = NPCLoader.GetNPC(type);
				list.Add(new TagCompound
				{
					["mod"] = modNPC.Mod.Name,
					["name"] = modNPC.Name,
					["count"] = killCount
				});
			}
		}
		return list;
	}

	internal static void LoadNPCKillCounts(IList<TagCompound> list)
	{
		foreach (TagCompound tag in list)
		{
			if (ModContent.TryFind<ModNPC>(tag.GetString("mod"), tag.GetString("name"), out var modNpc))
			{
				NPC.killCount[modNpc.Type] = tag.GetInt("count");
			}
			else
			{
				ModContent.GetInstance<UnloadedSystem>().unloadedKillCounts.Add(tag);
			}
		}
	}

	internal static List<TagCompound> SaveNPCBestiaryKills()
	{
		List<TagCompound> list = new List<TagCompound>();
		for (int type = NPCID.Count; type < NPCLoader.NPCCount; type++)
		{
			int killCount = Main.BestiaryTracker.Kills.GetKillCount(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[type]);
			if (killCount > 0)
			{
				ModNPC modNPC = NPCLoader.GetNPC(type);
				list.Add(new TagCompound
				{
					["mod"] = modNPC.Mod.Name,
					["name"] = modNPC.Name,
					["count"] = killCount
				});
			}
		}
		return list;
	}

	internal static void LoadNPCBestiaryKills(IList<TagCompound> list)
	{
		foreach (TagCompound tag in list)
		{
			if (ModContent.TryFind<ModNPC>(tag.GetString("mod"), tag.GetString("name"), out var modNpc))
			{
				string persistentId = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[modNpc.Type];
				Main.BestiaryTracker.Kills.SetKillCountDirectly(persistentId, tag.GetInt("count"));
			}
			else
			{
				ModContent.GetInstance<UnloadedSystem>().unloadedBestiaryKills.Add(tag);
			}
		}
	}

	internal static List<TagCompound> SaveNPCBestiarySights()
	{
		List<TagCompound> list = new List<TagCompound>();
		for (int type = NPCID.Count; type < NPCLoader.NPCCount; type++)
		{
			if (Main.BestiaryTracker.Sights.GetWasNearbyBefore(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[type]))
			{
				ModNPC modNPC = NPCLoader.GetNPC(type);
				list.Add(new TagCompound
				{
					["mod"] = modNPC.Mod.Name,
					["name"] = modNPC.Name
				});
			}
		}
		return list;
	}

	internal static void LoadNPCBestiarySights(IList<TagCompound> list)
	{
		foreach (TagCompound tag in list)
		{
			if (ModContent.TryFind<ModNPC>(tag.GetString("mod"), tag.GetString("name"), out var modNpc))
			{
				string persistentId = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[modNpc.Type];
				Main.BestiaryTracker.Sights.SetWasSeenDirectly(persistentId);
			}
			else
			{
				ModContent.GetInstance<UnloadedSystem>().unloadedBestiarySights.Add(tag);
			}
		}
	}

	internal static List<TagCompound> SaveNPCBestiaryChats()
	{
		List<TagCompound> list = new List<TagCompound>();
		for (int type = NPCID.Count; type < NPCLoader.NPCCount; type++)
		{
			if (Main.BestiaryTracker.Chats.GetWasChatWith(ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[type]))
			{
				ModNPC modNPC = NPCLoader.GetNPC(type);
				list.Add(new TagCompound
				{
					["mod"] = modNPC.Mod.Name,
					["name"] = modNPC.Name
				});
			}
		}
		return list;
	}

	internal static void LoadNPCBestiaryChats(IList<TagCompound> list)
	{
		foreach (TagCompound tag in list)
		{
			if (ModContent.TryFind<ModNPC>(tag.GetString("mod"), tag.GetString("name"), out var modNpc))
			{
				string persistentId = ContentSamples.NpcBestiaryCreditIdsByNpcNetIds[modNpc.Type];
				Main.BestiaryTracker.Chats.SetWasChatWithDirectly(persistentId);
			}
			else
			{
				ModContent.GetInstance<UnloadedSystem>().unloadedBestiaryChats.Add(tag);
			}
		}
	}

	internal static TagCompound SaveAnglerQuest()
	{
		if (Main.anglerQuest < ItemLoader.vanillaQuestFishCount)
		{
			return null;
		}
		ModItem modItem = ItemLoader.GetItem(Main.anglerQuestItemNetIDs[Main.anglerQuest]);
		return new TagCompound
		{
			["mod"] = modItem.Mod.Name,
			["itemName"] = modItem.Name
		};
	}

	internal static void LoadAnglerQuest(TagCompound tag)
	{
		if (!tag.ContainsKey("mod"))
		{
			return;
		}
		if (ModContent.TryFind<ModItem>(tag.GetString("mod"), tag.GetString("itemName"), out var modItem))
		{
			for (int i = 0; i < Main.anglerQuestItemNetIDs.Length; i++)
			{
				if (Main.anglerQuestItemNetIDs[i] == modItem.Type)
				{
					Main.anglerQuest = i;
					return;
				}
			}
		}
		Main.AnglerQuestSwap();
	}

	internal static List<TagCompound> SaveTownManager()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		List<TagCompound> list = new List<TagCompound>();
		foreach (Tuple<int, Point> pair in WorldGen.TownManager._roomLocationPairs)
		{
			if (pair.Item1 >= NPCID.Count)
			{
				ModNPC npc = NPCLoader.GetNPC(pair.Item1);
				TagCompound tag = new TagCompound
				{
					["mod"] = npc.Mod.Name,
					["name"] = npc.Name,
					["x"] = pair.Item2.X,
					["y"] = pair.Item2.Y
				};
				list.Add(tag);
			}
		}
		return list;
	}

	internal static void LoadTownManager(IList<TagCompound> list)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		if (list == null)
		{
			return;
		}
		Point location = default(Point);
		foreach (TagCompound tag in list)
		{
			if (ModContent.TryFind<ModNPC>(tag.GetString("mod"), tag.GetString("name"), out var modNpc))
			{
				((Point)(ref location))._002Ector(tag.GetInt("x"), tag.GetInt("y"));
				WorldGen.TownManager._roomLocationPairs.Add(Tuple.Create<int, Point>(modNpc.Type, location));
				WorldGen.TownManager._hasRoom[modNpc.Type] = true;
			}
		}
	}

	internal static List<TagCompound> SaveModData()
	{
		List<TagCompound> list = new List<TagCompound>();
		TagCompound saveData = new TagCompound();
		foreach (ModSystem system in SystemLoader.Systems)
		{
			system.SaveWorldData(saveData);
			if (saveData.Count != 0)
			{
				list.Add(new TagCompound
				{
					["mod"] = system.Mod.Name,
					["name"] = system.Name,
					["data"] = saveData
				});
				saveData = new TagCompound();
			}
		}
		return list;
	}

	internal static void LoadModData(IList<TagCompound> list)
	{
		foreach (TagCompound tag in list)
		{
			if (ModContent.TryFind<ModSystem>(tag.GetString("mod"), tag.GetString("name"), out var system))
			{
				try
				{
					system.LoadWorldData(tag.GetCompound("data"));
				}
				catch (Exception e)
				{
					throw new CustomModDataException(system.Mod, "Error in reading custom world data for " + system.Mod.Name, e);
				}
			}
			else
			{
				ModContent.GetInstance<UnloadedSystem>().data.Add(tag);
			}
		}
	}

	internal static TagCompound SaveAlteredVanillaFields()
	{
		return new TagCompound
		{
			["timeCultists"] = CultistRitual.delay,
			["timeRain"] = Main.rainTime,
			["timeSandstorm"] = Sandstorm.TimeLeft
		};
	}

	internal static void LoadAlteredVanillaFields(TagCompound compound)
	{
		CultistRitual.delay = compound.GetDouble("timeCultists");
		Main.rainTime = compound.GetDouble("timeRain");
		Sandstorm.TimeLeft = compound.GetDouble("timeSandstorm");
	}

	public static void SendModData(BinaryWriter writer)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = SystemLoader.HookNetSend.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			ModSystem system = readOnlySpan[i];
			writer.SafeWrite(delegate(BinaryWriter w)
			{
				system.NetSend(w);
			});
		}
	}

	public static void ReceiveModData(BinaryReader reader)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = SystemLoader.HookNetReceive.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			ModSystem system = readOnlySpan[i];
			try
			{
				reader.SafeRead(delegate(BinaryReader r)
				{
					system.NetReceive(r);
				});
			}
			catch (IOException e)
			{
				Logging.tML.Error((object)e.ToString());
				Logging.tML.Error((object)$"Above IOException error caused by {system.Name} from the {system.Mod.Name} mod.");
			}
		}
	}

	public static void ValidateSigns()
	{
		for (int i = 0; i < Main.sign.Length; i++)
		{
			if (Main.sign[i] != null)
			{
				Tile tile = Main.tile[Main.sign[i].x, Main.sign[i].y];
				if (!tile.active() || !Main.tileSign[tile.type])
				{
					Main.sign[i] = null;
				}
			}
		}
	}

	internal static void MoveToCloud(string localPath, string cloudPath)
	{
		localPath = Path.ChangeExtension(localPath, ".twld");
		cloudPath = Path.ChangeExtension(cloudPath, ".twld");
		if (File.Exists(localPath))
		{
			FileUtilities.MoveToCloud(localPath, cloudPath);
		}
	}

	internal static void MoveToLocal(string cloudPath, string localPath)
	{
		cloudPath = Path.ChangeExtension(cloudPath, ".twld");
		localPath = Path.ChangeExtension(localPath, ".twld");
		if (FileUtilities.Exists(cloudPath, cloud: true))
		{
			FileUtilities.MoveToLocal(cloudPath, localPath);
		}
	}

	internal static void LoadBackup(string path, bool cloudSave)
	{
		path = Path.ChangeExtension(path, ".twld");
		if (FileUtilities.Exists(path + ".bak", cloudSave))
		{
			FileUtilities.Move(path + ".bak", path, cloudSave);
		}
	}

	internal static void LoadDedServBackup(string path, bool cloudSave)
	{
		path = Path.ChangeExtension(path, ".twld");
		if (FileUtilities.Exists(path, cloudSave))
		{
			FileUtilities.Copy(path, path + ".bad", cloudSave);
		}
		if (FileUtilities.Exists(path + ".bak", cloudSave))
		{
			FileUtilities.Copy(path + ".bak", path, cloudSave);
			FileUtilities.Delete(path + ".bak", cloudSave);
		}
	}

	internal static void RevertDedServBackup(string path, bool cloudSave)
	{
		path = Path.ChangeExtension(path, ".twld");
		if (FileUtilities.Exists(path, cloudSave))
		{
			FileUtilities.Copy(path, path + ".bak", cloudSave);
		}
		if (FileUtilities.Exists(path + ".bad", cloudSave))
		{
			FileUtilities.Copy(path + ".bad", path, cloudSave);
			FileUtilities.Delete(path + ".bad", cloudSave);
		}
	}

	internal static void EraseWorld(string path, bool cloudSave)
	{
		path = Path.ChangeExtension(path, ".twld");
		if (!cloudSave)
		{
			Platform.Get<IPathService>().MoveToRecycleBin(path);
			Platform.Get<IPathService>().MoveToRecycleBin(path + ".bak");
		}
		else if (SocialAPI.Cloud != null)
		{
			SocialAPI.Cloud.Delete(path);
		}
	}

	private static TagCompound SaveHeader()
	{
		return new TagCompound
		{
			["modHeaders"] = SaveModHeaders(),
			["usedMods"] = SaveUsedMods(),
			["usedModPack"] = SaveUsedModPack(),
			["generatedWithMods"] = SaveGeneratedWithMods()
		};
	}

	private static TagCompound SaveModHeaders()
	{
		TagCompound modHeaders = new TagCompound();
		TagCompound saveData = new TagCompound();
		foreach (ModSystem system in SystemLoader.Systems)
		{
			system.SaveWorldHeader(saveData);
			if (saveData.Count != 0)
			{
				modHeaders[system.FullName] = saveData;
				saveData = new TagCompound();
			}
		}
		foreach (KeyValuePair<string, TagCompound> entry in Main.ActiveWorldFileData.ModHeaders)
		{
			if (!ModContent.TryFind<ModSystem>(entry.Key, out var _))
			{
				modHeaders[entry.Key] = entry.Value;
			}
		}
		return modHeaders;
	}

	internal static void ReadWorldHeader(WorldFileData data)
	{
		string path = Path.ChangeExtension(data.Path, ".twld");
		bool isCloudSave = data.IsCloudSave;
		if (!FileUtilities.Exists(path, isCloudSave))
		{
			return;
		}
		try
		{
			using Stream stream = (isCloudSave ? ((Stream)new MemoryStream(SocialAPI.Cloud.Read(path))) : ((Stream)new FileStream(path, FileMode.Open)));
			using BinaryReader reader = new BigEndianReader(new GZipStream(stream, CompressionMode.Decompress));
			if (reader.ReadByte() != 10)
			{
				throw new IOException("Root tag not a TagCompound");
			}
			TagIO.ReadTagImpl(8, reader);
			if (reader.ReadByte() == 10 && !((string)TagIO.ReadTagImpl(8, reader) != "0header"))
			{
				LoadWorldHeader(data, (TagCompound)TagIO.ReadTagImpl(10, reader));
			}
		}
		catch (Exception ex)
		{
			Logging.tML.Warn((object)$"Error reading .twld header from: {path} (IsCloudSave={isCloudSave})", ex);
		}
	}

	private static void LoadWorldHeader(WorldFileData data, TagCompound tag)
	{
		LoadModHeaders(data, tag);
		LoadUsedMods(data, tag.GetList<string>("usedMods"));
		LoadUsedModPack(data, tag.GetString("usedModPack"));
		if (tag.ContainsKey("generatedWithMods"))
		{
			LoadGeneratedWithMods(data, tag.GetCompound("generatedWithMods"));
		}
	}

	private static void LoadModHeaders(WorldFileData data, TagCompound tag)
	{
		data.ModHeaders = new Dictionary<string, TagCompound>();
		foreach (KeyValuePair<string, object> entry in tag.GetCompound("modHeaders"))
		{
			string fullname = entry.Key;
			if (ModContent.TryFind<ModSystem>(fullname, out var system))
			{
				fullname = system.FullName;
			}
			data.ModHeaders[fullname] = (TagCompound)entry.Value;
		}
	}

	internal static void LoadUsedMods(WorldFileData data, IList<string> usedMods)
	{
		data.usedMods = usedMods;
	}

	internal static List<string> SaveUsedMods()
	{
		return ModLoader.Mods.Select((Mod m) => m.Name).Except(new string[1] { "ModLoader" }).ToList();
	}

	internal static void LoadUsedModPack(WorldFileData data, string modpack)
	{
		data.modPack = (string.IsNullOrEmpty(modpack) ? null : modpack);
	}

	internal static string SaveUsedModPack()
	{
		return Path.GetFileNameWithoutExtension(ModOrganizer.ModPackActive);
	}

	internal static void LoadGeneratedWithMods(WorldFileData data, TagCompound tag)
	{
		data.modVersionsDuringWorldGen = new Dictionary<string, Version>();
		foreach (KeyValuePair<string, object> item in tag)
		{
			data.modVersionsDuringWorldGen[item.Key] = new Version((string)item.Value);
		}
	}

	internal static TagCompound SaveGeneratedWithMods()
	{
		if (Main.ActiveWorldFileData.modVersionsDuringWorldGen == null)
		{
			return null;
		}
		TagCompound tag = new TagCompound();
		foreach (KeyValuePair<string, Version> item in Main.ActiveWorldFileData.modVersionsDuringWorldGen)
		{
			tag[item.Key] = item.Value;
		}
		return tag;
	}
}
