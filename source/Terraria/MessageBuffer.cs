using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Golf;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.IO;
using Terraria.Net;
using Terraria.Testing;
using Terraria.UI;

namespace Terraria;

public class MessageBuffer
{
	public const int readBufferMax = 131070;

	public const int writeBufferMax = 131070;

	public bool broadcast;

	public byte[] readBuffer = new byte[131070];

	public byte[] writeBuffer = new byte[131070];

	public bool writeLocked;

	public int messageLength;

	public int totalData;

	public int whoAmI;

	public int spamCount;

	public int maxSpam;

	public bool checkBytes;

	public MemoryStream readerStream;

	public MemoryStream writerStream;

	public BinaryReader reader;

	public BinaryWriter writer;

	public PacketHistory History = new PacketHistory();

	private float[] _temporaryProjectileAI = new float[Projectile.maxAI];

	private float[] _temporaryNPCAI = new float[NPC.maxAI];

	public static event TileChangeReceivedEvent OnTileChangeReceived;

	public void Reset()
	{
		Array.Clear(readBuffer, 0, readBuffer.Length);
		Array.Clear(writeBuffer, 0, writeBuffer.Length);
		writeLocked = false;
		messageLength = 0;
		totalData = 0;
		spamCount = 0;
		broadcast = false;
		checkBytes = false;
		ResetReader();
		ResetWriter();
	}

	public void ResetReader()
	{
		if (readerStream != null)
		{
			readerStream.Close();
		}
		readerStream = new MemoryStream(readBuffer);
		reader = new BinaryReader(readerStream);
	}

	public void ResetWriter()
	{
		if (writerStream != null)
		{
			writerStream.Close();
		}
		writerStream = new MemoryStream(writeBuffer);
		writer = new BinaryWriter(writerStream);
	}

	private float[] ReUseTemporaryProjectileAI()
	{
		for (int i = 0; i < _temporaryProjectileAI.Length; i++)
		{
			_temporaryProjectileAI[i] = 0f;
		}
		return _temporaryProjectileAI;
	}

	private float[] ReUseTemporaryNPCAI()
	{
		for (int i = 0; i < _temporaryNPCAI.Length; i++)
		{
			_temporaryNPCAI[i] = 0f;
		}
		return _temporaryNPCAI;
	}

	public void GetData(int start, int length, out int messageType)
	{
		//IL_3562: Unknown result type (might be due to invalid IL or missing references)
		//IL_3567: Unknown result type (might be due to invalid IL or missing references)
		//IL_356f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3574: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d42: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_7e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_7eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_8098: Unknown result type (might be due to invalid IL or missing references)
		//IL_809d: Unknown result type (might be due to invalid IL or missing references)
		//IL_80a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_80ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_8217: Unknown result type (might be due to invalid IL or missing references)
		//IL_821c: Unknown result type (might be due to invalid IL or missing references)
		//IL_9cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_9cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_39bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_39c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_39c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_52d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_52d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6933: Unknown result type (might be due to invalid IL or missing references)
		//IL_6938: Unknown result type (might be due to invalid IL or missing references)
		//IL_73fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_7400: Unknown result type (might be due to invalid IL or missing references)
		//IL_7416: Unknown result type (might be due to invalid IL or missing references)
		//IL_741b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_7fb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_8248: Unknown result type (might be due to invalid IL or missing references)
		//IL_8253: Unknown result type (might be due to invalid IL or missing references)
		//IL_872a: Unknown result type (might be due to invalid IL or missing references)
		//IL_874c: Unknown result type (might be due to invalid IL or missing references)
		//IL_8768: Unknown result type (might be due to invalid IL or missing references)
		//IL_876d: Unknown result type (might be due to invalid IL or missing references)
		//IL_8795: Unknown result type (might be due to invalid IL or missing references)
		//IL_88e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_88f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_7456: Unknown result type (might be due to invalid IL or missing references)
		//IL_745b: Unknown result type (might be due to invalid IL or missing references)
		//IL_747b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7480: Unknown result type (might be due to invalid IL or missing references)
		//IL_95e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_983b: Unknown result type (might be due to invalid IL or missing references)
		//IL_9840: Unknown result type (might be due to invalid IL or missing references)
		//IL_9845: Unknown result type (might be due to invalid IL or missing references)
		//IL_9d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_9d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_9d29: Unknown result type (might be due to invalid IL or missing references)
		//IL_9d34: Unknown result type (might be due to invalid IL or missing references)
		//IL_9e4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_9e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_9e65: Unknown result type (might be due to invalid IL or missing references)
		//IL_9e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ede: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ef0: Unknown result type (might be due to invalid IL or missing references)
		//IL_7ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_80dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_80ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_80f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_80fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_8105: Unknown result type (might be due to invalid IL or missing references)
		//IL_810a: Unknown result type (might be due to invalid IL or missing references)
		//IL_8a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_7f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_43df: Unknown result type (might be due to invalid IL or missing references)
		//IL_43e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4406: Unknown result type (might be due to invalid IL or missing references)
		//IL_9926: Unknown result type (might be due to invalid IL or missing references)
		//IL_992c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c94: Unknown result type (might be due to invalid IL or missing references)
		//IL_61f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_537c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6282: Unknown result type (might be due to invalid IL or missing references)
		//IL_69eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_69f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_82e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f70: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_8301: Unknown result type (might be due to invalid IL or missing references)
		//IL_830c: Unknown result type (might be due to invalid IL or missing references)
		//IL_8311: Unknown result type (might be due to invalid IL or missing references)
		//IL_8316: Unknown result type (might be due to invalid IL or missing references)
		//IL_831c: Unknown result type (might be due to invalid IL or missing references)
		//IL_8322: Unknown result type (might be due to invalid IL or missing references)
		//IL_8327: Unknown result type (might be due to invalid IL or missing references)
		//IL_832d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2975: Unknown result type (might be due to invalid IL or missing references)
		//IL_297a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6349: Unknown result type (might be due to invalid IL or missing references)
		//IL_6319: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a96: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_299f: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2991: Unknown result type (might be due to invalid IL or missing references)
		//IL_2996: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b66: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b12: Unknown result type (might be due to invalid IL or missing references)
		//IL_64d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b31: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6469: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c20: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2053: Unknown result type (might be due to invalid IL or missing references)
		//IL_29bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3819: Unknown result type (might be due to invalid IL or missing references)
		//IL_381b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3822: Unknown result type (might be due to invalid IL or missing references)
		//IL_3824: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_37ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_36bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6be8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3716: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_83eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_83fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_8403: Unknown result type (might be due to invalid IL or missing references)
		//IL_8408: Unknown result type (might be due to invalid IL or missing references)
		//IL_840d: Unknown result type (might be due to invalid IL or missing references)
		//IL_8420: Unknown result type (might be due to invalid IL or missing references)
		//IL_8426: Unknown result type (might be due to invalid IL or missing references)
		//IL_842c: Unknown result type (might be due to invalid IL or missing references)
		//IL_8431: Unknown result type (might be due to invalid IL or missing references)
		//IL_8436: Unknown result type (might be due to invalid IL or missing references)
		//IL_843c: Unknown result type (might be due to invalid IL or missing references)
		//IL_846a: Unknown result type (might be due to invalid IL or missing references)
		//IL_8470: Unknown result type (might be due to invalid IL or missing references)
		//IL_84a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_84a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_84e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_84ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_84f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_84fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_8502: Unknown result type (might be due to invalid IL or missing references)
		//IL_65d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_224a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2258: Unknown result type (might be due to invalid IL or missing references)
		//IL_6624: Unknown result type (might be due to invalid IL or missing references)
		//IL_2102: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fc9: Unknown result type (might be due to invalid IL or missing references)
		if (whoAmI < 256)
		{
			Netplay.Clients[whoAmI].TimeOutTimer = 0;
		}
		else
		{
			Netplay.Connection.TimeOutTimer = 0;
		}
		byte b = 0;
		int num = 0;
		num = start + 1;
		int interactingPlayer = (messageType = readBuffer[start]);
		b = (byte)interactingPlayer;
		if (ModNet.DetailedLogging)
		{
			ModNet.Debug(whoAmI, $"GetData {MessageID.GetName(b)}({b}), {length + 2}");
		}
		Main.ActiveNetDiagnosticsUI.CountReadMessage(b, length);
		if (Main.netMode == 1 && Netplay.Connection.StatusMax > 0)
		{
			Netplay.Connection.StatusCount++;
		}
		if (Main.verboseNetplay)
		{
			for (int i = start; i < start + length; i++)
			{
			}
			for (int j = start; j < start + length; j++)
			{
				_ = readBuffer[j];
			}
		}
		if (Main.netMode == 2 && b != 38 && Netplay.Clients[whoAmI].State == -1)
		{
			NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[1].ToNetworkText());
			return;
		}
		if (Main.netMode == 2)
		{
			if (Netplay.Clients[whoAmI].State < 10 && b > 12 && b != 93 && b != 16 && b != 42 && b != 50 && b != 38 && b != 68 && b != 147 && b < 250)
			{
				NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
			}
			if (Netplay.Clients[whoAmI].State == 0 && b != 1)
			{
				NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
			}
		}
		if (reader == null)
		{
			ResetReader();
		}
		reader.BaseStream.Position = num;
		if (ModNet.HijackGetData(ref b, ref reader, whoAmI))
		{
			return;
		}
		switch (b)
		{
		case 1:
			if (Main.netMode != 2)
			{
				break;
			}
			if (Main.dedServ && Netplay.IsBanned(Netplay.Clients[whoAmI].Socket.GetRemoteAddress()))
			{
				NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[3].ToNetworkText());
			}
			else
			{
				if (Netplay.Clients[whoAmI].State != 0)
				{
					break;
				}
				if (ModNet.IsClientCompatible(reader.ReadString(), out ModNet.isModdedClient[whoAmI], out var kickMsg))
				{
					if (string.IsNullOrEmpty(Netplay.ServerPassword))
					{
						Netplay.Clients[whoAmI].State = 1;
						if (ModNet.isModdedClient[whoAmI])
						{
							ModNet.SyncMods(whoAmI);
						}
						else
						{
							NetMessage.TrySendData(3, whoAmI);
						}
					}
					else
					{
						Netplay.Clients[whoAmI].State = -1;
						NetMessage.TrySendData(37, whoAmI);
					}
				}
				else
				{
					NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromLiteral(Lang.mp[4].Value + "\n(" + kickMsg + ")"));
				}
			}
			break;
		case 2:
			if (Main.netMode == 1)
			{
				Netplay.Disconnect = true;
				Main.statusText = NetworkText.Deserialize(reader).ToString();
				Main.menuMode = 14;
			}
			break;
		case 3:
			if (Main.netMode == 1)
			{
				if (Netplay.Connection.State == 1)
				{
					Netplay.Connection.State = 2;
				}
				int num47 = reader.ReadByte();
				bool value5 = reader.ReadBoolean();
				Netplay.Connection.ServerSpecialFlags[2] = value5;
				if (num47 != Main.myPlayer)
				{
					Main.player[num47] = Main.ActivePlayerFileData.Player;
					Main.player[Main.myPlayer] = new Player();
				}
				Main.player[num47].whoAmI = num47;
				Main.myPlayer = num47;
				Player player3 = Main.player[num47];
				NetMessage.TrySendData(4, -1, -1, null, num47);
				NetMessage.TrySendData(68, -1, -1, null, num47);
				NetMessage.TrySendData(16, -1, -1, null, num47);
				NetMessage.TrySendData(42, -1, -1, null, num47);
				NetMessage.TrySendData(50, -1, -1, null, num47);
				NetMessage.TrySendData(147, -1, -1, null, num47, player3.CurrentLoadoutIndex);
				for (int num48 = 0; num48 < 59; num48++)
				{
					NetMessage.TrySendData(5, -1, -1, null, num47, PlayerItemSlotID.Inventory0 + num48, player3.inventory[num48].prefix);
				}
				TrySendingItemArray(num47, player3.armor, PlayerItemSlotID.Armor0);
				TrySendingItemArray(num47, player3.dye, PlayerItemSlotID.Dye0);
				TrySendingItemArray(num47, player3.miscEquips, PlayerItemSlotID.Misc0);
				TrySendingItemArray(num47, player3.miscDyes, PlayerItemSlotID.MiscDye0);
				TrySendingItemArray(num47, player3.bank.item, PlayerItemSlotID.Bank1_0);
				TrySendingItemArray(num47, player3.bank2.item, PlayerItemSlotID.Bank2_0);
				NetMessage.TrySendData(5, -1, -1, null, num47, PlayerItemSlotID.TrashItem, player3.trashItem.prefix);
				TrySendingItemArray(num47, player3.bank3.item, PlayerItemSlotID.Bank3_0);
				TrySendingItemArray(num47, player3.bank4.item, PlayerItemSlotID.Bank4_0);
				TrySendingItemArray(num47, player3.Loadouts[0].Armor, PlayerItemSlotID.Loadout1_Armor_0);
				TrySendingItemArray(num47, player3.Loadouts[0].Dye, PlayerItemSlotID.Loadout1_Dye_0);
				TrySendingItemArray(num47, player3.Loadouts[1].Armor, PlayerItemSlotID.Loadout2_Armor_0);
				TrySendingItemArray(num47, player3.Loadouts[1].Dye, PlayerItemSlotID.Loadout2_Dye_0);
				TrySendingItemArray(num47, player3.Loadouts[2].Armor, PlayerItemSlotID.Loadout3_Armor_0);
				TrySendingItemArray(num47, player3.Loadouts[2].Dye, PlayerItemSlotID.Loadout3_Dye_0);
				PlayerLoader.SyncPlayer(player3, -1, -1, newPlayer: true);
				NetMessage.TrySendData(6);
				if (Netplay.Connection.State == 2)
				{
					Netplay.Connection.State = 3;
				}
			}
			break;
		case 4:
		{
			int num121 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num121 = whoAmI;
			}
			if (num121 == Main.myPlayer && !Main.ServerSideCharacter)
			{
				break;
			}
			Player player5 = Main.player[num121];
			player5.whoAmI = num121;
			player5.skinVariant = reader.ReadByte();
			player5.skinVariant = (int)MathHelper.Clamp((float)player5.skinVariant, 0f, (float)(PlayerVariantID.Count - 1));
			player5.hair = reader.ReadByte();
			if (player5.hair >= HairLoader.Count)
			{
				player5.hair = 0;
			}
			player5.name = reader.ReadString().Trim().Trim();
			player5.hairDye = (ModNet.AllowVanillaClients ? reader.ReadByte() : reader.Read7BitEncodedInt());
			ReadAccessoryVisibility(reader, player5.hideVisibleAccessory);
			player5.hideMisc = reader.ReadByte();
			player5.hairColor = reader.ReadRGB();
			player5.skinColor = reader.ReadRGB();
			player5.eyeColor = reader.ReadRGB();
			player5.shirtColor = reader.ReadRGB();
			player5.underShirtColor = reader.ReadRGB();
			player5.pantsColor = reader.ReadRGB();
			player5.shoeColor = reader.ReadRGB();
			BitsByte bitsByte17 = reader.ReadByte();
			player5.difficulty = 0;
			if (bitsByte17[0])
			{
				player5.difficulty = 1;
			}
			if (bitsByte17[1])
			{
				player5.difficulty = 2;
			}
			if (bitsByte17[3])
			{
				player5.difficulty = 3;
			}
			if (player5.difficulty > 3)
			{
				player5.difficulty = 3;
			}
			player5.extraAccessory = bitsByte17[2];
			BitsByte bitsByte18 = reader.ReadByte();
			player5.UsingBiomeTorches = bitsByte18[0];
			player5.happyFunTorchTime = bitsByte18[1];
			player5.unlockedBiomeTorches = bitsByte18[2];
			player5.unlockedSuperCart = bitsByte18[3];
			player5.enabledSuperCart = bitsByte18[4];
			BitsByte bitsByte19 = reader.ReadByte();
			player5.usedAegisCrystal = bitsByte19[0];
			player5.usedAegisFruit = bitsByte19[1];
			player5.usedArcaneCrystal = bitsByte19[2];
			player5.usedGalaxyPearl = bitsByte19[3];
			player5.usedGummyWorm = bitsByte19[4];
			player5.usedAmbrosia = bitsByte19[5];
			player5.ateArtisanBread = bitsByte19[6];
			if (Main.netMode != 2)
			{
				break;
			}
			bool flag4 = false;
			if (Netplay.Clients[whoAmI].State < 10)
			{
				for (int num122 = 0; num122 < 255; num122++)
				{
					if (num122 != num121 && player5.name == Main.player[num122].name && Netplay.Clients[num122].IsActive)
					{
						flag4 = true;
					}
				}
			}
			if (flag4)
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey(Lang.mp[5].Key, player5.name));
			}
			else if (player5.name.Length > Player.nameLen)
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.NameTooLong"));
			}
			else if (player5.name == "")
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.EmptyName"));
			}
			else if (player5.difficulty == 3 && !Main.GameModeInfo.IsJourneyMode)
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.PlayerIsCreativeAndWorldIsNotCreative"));
			}
			else if (player5.difficulty != 3 && Main.GameModeInfo.IsJourneyMode)
			{
				NetMessage.TrySendData(2, whoAmI, -1, NetworkText.FromKey("Net.PlayerIsNotCreativeAndWorldIsCreative"));
			}
			else
			{
				Netplay.Clients[whoAmI].Name = player5.name;
				Netplay.Clients[whoAmI].Name = player5.name;
				NetMessage.TrySendData(4, -1, whoAmI, null, num121);
			}
			break;
		}
		case 5:
		{
			int num171 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num171 = whoAmI;
			}
			if (num171 == Main.myPlayer && !Main.ServerSideCharacter && !Main.player[num171].HasLockedInventory())
			{
				break;
			}
			Player player8 = Main.player[num171];
			lock (player8)
			{
				int num172 = reader.ReadInt16();
				int stack7 = (ModNet.AllowVanillaClients ? reader.ReadInt16() : (-1));
				int num174 = (ModNet.AllowVanillaClients ? reader.ReadByte() : (-1));
				int type8 = (ModNet.AllowVanillaClients ? reader.ReadInt16() : (-1));
				Item[] array3 = null;
				Item[] array4 = null;
				int num175 = 0;
				bool flag8 = false;
				Player clientPlayer = Main.clientPlayer;
				if (num172 >= PlayerItemSlotID.Loadout3_Dye_0)
				{
					num175 = num172 - PlayerItemSlotID.Loadout3_Dye_0;
					array3 = player8.Loadouts[2].Dye;
					array4 = clientPlayer.Loadouts[2].Dye;
				}
				else if (num172 >= PlayerItemSlotID.Loadout3_Armor_0)
				{
					num175 = num172 - PlayerItemSlotID.Loadout3_Armor_0;
					array3 = player8.Loadouts[2].Armor;
					array4 = clientPlayer.Loadouts[2].Armor;
				}
				else if (num172 >= PlayerItemSlotID.Loadout2_Dye_0)
				{
					num175 = num172 - PlayerItemSlotID.Loadout2_Dye_0;
					array3 = player8.Loadouts[1].Dye;
					array4 = clientPlayer.Loadouts[1].Dye;
				}
				else if (num172 >= PlayerItemSlotID.Loadout2_Armor_0)
				{
					num175 = num172 - PlayerItemSlotID.Loadout2_Armor_0;
					array3 = player8.Loadouts[1].Armor;
					array4 = clientPlayer.Loadouts[1].Armor;
				}
				else if (num172 >= PlayerItemSlotID.Loadout1_Dye_0)
				{
					num175 = num172 - PlayerItemSlotID.Loadout1_Dye_0;
					array3 = player8.Loadouts[0].Dye;
					array4 = clientPlayer.Loadouts[0].Dye;
				}
				else if (num172 >= PlayerItemSlotID.Loadout1_Armor_0)
				{
					num175 = num172 - PlayerItemSlotID.Loadout1_Armor_0;
					array3 = player8.Loadouts[0].Armor;
					array4 = clientPlayer.Loadouts[0].Armor;
				}
				else if (num172 >= PlayerItemSlotID.Bank4_0)
				{
					num175 = num172 - PlayerItemSlotID.Bank4_0;
					array3 = player8.bank4.item;
					array4 = clientPlayer.bank4.item;
					if (Main.netMode == 1 && player8.disableVoidBag == num175)
					{
						player8.disableVoidBag = -1;
						Recipe.FindRecipes(canDelayCheck: true);
					}
				}
				else if (num172 >= PlayerItemSlotID.Bank3_0)
				{
					num175 = num172 - PlayerItemSlotID.Bank3_0;
					array3 = player8.bank3.item;
					array4 = clientPlayer.bank3.item;
				}
				else if (num172 >= PlayerItemSlotID.TrashItem)
				{
					flag8 = true;
				}
				else if (num172 >= PlayerItemSlotID.Bank2_0)
				{
					num175 = num172 - PlayerItemSlotID.Bank2_0;
					array3 = player8.bank2.item;
					array4 = clientPlayer.bank2.item;
				}
				else if (num172 >= PlayerItemSlotID.Bank1_0)
				{
					num175 = num172 - PlayerItemSlotID.Bank1_0;
					array3 = player8.bank.item;
					array4 = clientPlayer.bank.item;
				}
				else if (num172 >= PlayerItemSlotID.MiscDye0)
				{
					num175 = num172 - PlayerItemSlotID.MiscDye0;
					array3 = player8.miscDyes;
					array4 = clientPlayer.miscDyes;
				}
				else if (num172 >= PlayerItemSlotID.Misc0)
				{
					num175 = num172 - PlayerItemSlotID.Misc0;
					array3 = player8.miscEquips;
					array4 = clientPlayer.miscEquips;
				}
				else if (num172 >= PlayerItemSlotID.Dye0)
				{
					num175 = num172 - PlayerItemSlotID.Dye0;
					array3 = player8.dye;
					array4 = clientPlayer.dye;
				}
				else if (num172 >= PlayerItemSlotID.Armor0)
				{
					num175 = num172 - PlayerItemSlotID.Armor0;
					array3 = player8.armor;
					array4 = clientPlayer.armor;
				}
				else
				{
					num175 = num172 - PlayerItemSlotID.Inventory0;
					array3 = player8.inventory;
					array4 = clientPlayer.inventory;
				}
				if (flag8)
				{
					if (!ModNet.AllowVanillaClients)
					{
						player8.trashItem = ItemIO.Receive(reader, readStack: true);
					}
					else
					{
						player8.trashItem = new Item();
						player8.trashItem.netDefaults(type8);
						player8.trashItem.stack = stack7;
						player8.trashItem.Prefix(num174);
					}
					if (num171 == Main.myPlayer && !Main.ServerSideCharacter)
					{
						clientPlayer.trashItem = player8.trashItem.Clone();
					}
				}
				else if (num172 <= 58)
				{
					int type9 = array3[num175].type;
					int stack8 = array3[num175].stack;
					if (!ModNet.AllowVanillaClients)
					{
						array3[num175] = ItemIO.Receive(reader, readStack: true);
					}
					else
					{
						array3[num175] = new Item();
						array3[num175].netDefaults(type8);
						array3[num175].stack = stack7;
						array3[num175].Prefix(num174);
					}
					if (num171 == Main.myPlayer && !Main.ServerSideCharacter)
					{
						array4[num175] = array3[num175].Clone();
					}
					if (num171 == Main.myPlayer && num175 == 58)
					{
						Main.mouseItem = array3[num175].Clone();
					}
					if (num171 == Main.myPlayer && Main.netMode == 1)
					{
						Main.player[num171].inventoryChestStack[num172] = false;
						if (array3[num175].stack != stack8 || array3[num175].type != type9)
						{
							Recipe.FindRecipes(canDelayCheck: true);
						}
					}
				}
				else
				{
					if (!ModNet.AllowVanillaClients)
					{
						array3[num175] = ItemIO.Receive(reader, readStack: true);
					}
					else
					{
						array3[num175] = new Item();
						array3[num175].netDefaults(type8);
						array3[num175].stack = stack7;
						array3[num175].Prefix(num174);
					}
					if (num171 == Main.myPlayer && !Main.ServerSideCharacter)
					{
						array4[num175] = array3[num175].Clone();
					}
				}
				bool[] canRelay = PlayerItemSlotID.CanRelay;
				if (Main.netMode == 2 && num171 == whoAmI && canRelay.IndexInRange(num172) && canRelay[num172])
				{
					NetMessage.TrySendData(5, -1, whoAmI, null, num171, num172, num174);
				}
				break;
			}
		}
		case 6:
			if (Main.netMode == 2)
			{
				if (Netplay.Clients[whoAmI].State == 1)
				{
					Netplay.Clients[whoAmI].State = 2;
				}
				NetMessage.TrySendData(7, whoAmI);
				Main.SyncAnInvasion(whoAmI);
			}
			break;
		case 7:
			if (Main.netMode == 1)
			{
				Main.time = reader.ReadInt32();
				BitsByte bitsByte5 = reader.ReadByte();
				Main.dayTime = bitsByte5[0];
				Main.bloodMoon = bitsByte5[1];
				Main.eclipse = bitsByte5[2];
				Main.moonPhase = reader.ReadByte();
				Main.maxTilesX = reader.ReadInt16();
				Main.maxTilesY = reader.ReadInt16();
				Main.spawnTileX = reader.ReadInt16();
				Main.spawnTileY = reader.ReadInt16();
				Main.worldSurface = reader.ReadInt16();
				Main.rockLayer = reader.ReadInt16();
				Main.worldID = reader.ReadInt32();
				Main.worldName = reader.ReadString();
				Main.GameMode = reader.ReadByte();
				Main.ActiveWorldFileData.UniqueId = new Guid(reader.ReadBytes(16));
				Main.ActiveWorldFileData.WorldGeneratorVersion = reader.ReadUInt64();
				Main.moonType = reader.ReadByte();
				WorldGen.setBG(0, reader.ReadByte());
				WorldGen.setBG(10, reader.ReadByte());
				WorldGen.setBG(11, reader.ReadByte());
				WorldGen.setBG(12, reader.ReadByte());
				WorldGen.setBG(1, reader.ReadByte());
				WorldGen.setBG(2, reader.ReadByte());
				WorldGen.setBG(3, reader.ReadByte());
				WorldGen.setBG(4, reader.ReadByte());
				WorldGen.setBG(5, reader.ReadByte());
				WorldGen.setBG(6, reader.ReadByte());
				WorldGen.setBG(7, reader.ReadByte());
				WorldGen.setBG(8, reader.ReadByte());
				WorldGen.setBG(9, reader.ReadByte());
				Main.iceBackStyle = reader.ReadByte();
				Main.jungleBackStyle = reader.ReadByte();
				Main.hellBackStyle = reader.ReadByte();
				Main.windSpeedTarget = reader.ReadSingle();
				Main.numClouds = reader.ReadByte();
				for (int num102 = 0; num102 < 3; num102++)
				{
					Main.treeX[num102] = reader.ReadInt32();
				}
				for (int num103 = 0; num103 < 4; num103++)
				{
					Main.treeStyle[num103] = reader.ReadByte();
				}
				for (int num104 = 0; num104 < 3; num104++)
				{
					Main.caveBackX[num104] = reader.ReadInt32();
				}
				for (int num105 = 0; num105 < 4; num105++)
				{
					Main.caveBackStyle[num105] = reader.ReadByte();
				}
				WorldGen.TreeTops.SyncReceive(reader);
				WorldGen.BackgroundsCache.UpdateCache();
				Main.maxRaining = reader.ReadSingle();
				Main.raining = Main.maxRaining > 0f;
				BitsByte bitsByte6 = reader.ReadByte();
				WorldGen.shadowOrbSmashed = bitsByte6[0];
				NPC.downedBoss1 = bitsByte6[1];
				NPC.downedBoss2 = bitsByte6[2];
				NPC.downedBoss3 = bitsByte6[3];
				Main.hardMode = bitsByte6[4];
				NPC.downedClown = bitsByte6[5];
				Main.ServerSideCharacter = bitsByte6[6];
				NPC.downedPlantBoss = bitsByte6[7];
				if (Main.ServerSideCharacter)
				{
					Main.ActivePlayerFileData.MarkAsServerSide();
				}
				BitsByte bitsByte7 = reader.ReadByte();
				NPC.downedMechBoss1 = bitsByte7[0];
				NPC.downedMechBoss2 = bitsByte7[1];
				NPC.downedMechBoss3 = bitsByte7[2];
				NPC.downedMechBossAny = bitsByte7[3];
				Main.cloudBGActive = (bitsByte7[4] ? 1 : 0);
				WorldGen.crimson = bitsByte7[5];
				Main.pumpkinMoon = bitsByte7[6];
				Main.snowMoon = bitsByte7[7];
				BitsByte bitsByte8 = reader.ReadByte();
				Main.fastForwardTimeToDawn = bitsByte8[1];
				Main.UpdateTimeRate();
				bool num254 = bitsByte8[2];
				NPC.downedSlimeKing = bitsByte8[3];
				NPC.downedQueenBee = bitsByte8[4];
				NPC.downedFishron = bitsByte8[5];
				NPC.downedMartians = bitsByte8[6];
				NPC.downedAncientCultist = bitsByte8[7];
				BitsByte bitsByte9 = reader.ReadByte();
				NPC.downedMoonlord = bitsByte9[0];
				NPC.downedHalloweenKing = bitsByte9[1];
				NPC.downedHalloweenTree = bitsByte9[2];
				NPC.downedChristmasIceQueen = bitsByte9[3];
				NPC.downedChristmasSantank = bitsByte9[4];
				NPC.downedChristmasTree = bitsByte9[5];
				NPC.downedGolemBoss = bitsByte9[6];
				BirthdayParty.ManualParty = bitsByte9[7];
				BitsByte bitsByte10 = reader.ReadByte();
				NPC.downedPirates = bitsByte10[0];
				NPC.downedFrost = bitsByte10[1];
				NPC.downedGoblins = bitsByte10[2];
				Sandstorm.Happening = bitsByte10[3];
				DD2Event.Ongoing = bitsByte10[4];
				DD2Event.DownedInvasionT1 = bitsByte10[5];
				DD2Event.DownedInvasionT2 = bitsByte10[6];
				DD2Event.DownedInvasionT3 = bitsByte10[7];
				BitsByte bitsByte11 = reader.ReadByte();
				NPC.combatBookWasUsed = bitsByte11[0];
				LanternNight.ManualLanterns = bitsByte11[1];
				NPC.downedTowerSolar = bitsByte11[2];
				NPC.downedTowerVortex = bitsByte11[3];
				NPC.downedTowerNebula = bitsByte11[4];
				NPC.downedTowerStardust = bitsByte11[5];
				Main.forceHalloweenForToday = bitsByte11[6];
				Main.forceXMasForToday = bitsByte11[7];
				BitsByte bitsByte13 = reader.ReadByte();
				NPC.boughtCat = bitsByte13[0];
				NPC.boughtDog = bitsByte13[1];
				NPC.boughtBunny = bitsByte13[2];
				NPC.freeCake = bitsByte13[3];
				Main.drunkWorld = bitsByte13[4];
				NPC.downedEmpressOfLight = bitsByte13[5];
				NPC.downedQueenSlime = bitsByte13[6];
				Main.getGoodWorld = bitsByte13[7];
				BitsByte bitsByte14 = reader.ReadByte();
				Main.tenthAnniversaryWorld = bitsByte14[0];
				Main.dontStarveWorld = bitsByte14[1];
				NPC.downedDeerclops = bitsByte14[2];
				Main.notTheBeesWorld = bitsByte14[3];
				Main.remixWorld = bitsByte14[4];
				NPC.unlockedSlimeBlueSpawn = bitsByte14[5];
				NPC.combatBookVolumeTwoWasUsed = bitsByte14[6];
				NPC.peddlersSatchelWasUsed = bitsByte14[7];
				BitsByte bitsByte15 = reader.ReadByte();
				NPC.unlockedSlimeGreenSpawn = bitsByte15[0];
				NPC.unlockedSlimeOldSpawn = bitsByte15[1];
				NPC.unlockedSlimePurpleSpawn = bitsByte15[2];
				NPC.unlockedSlimeRainbowSpawn = bitsByte15[3];
				NPC.unlockedSlimeRedSpawn = bitsByte15[4];
				NPC.unlockedSlimeYellowSpawn = bitsByte15[5];
				NPC.unlockedSlimeCopperSpawn = bitsByte15[6];
				Main.fastForwardTimeToDusk = bitsByte15[7];
				BitsByte bitsByte16 = reader.ReadByte();
				Main.noTrapsWorld = bitsByte16[0];
				Main.zenithWorld = bitsByte16[1];
				NPC.unlockedTruffleSpawn = bitsByte16[2];
				Main.sundialCooldown = reader.ReadByte();
				Main.moondialCooldown = reader.ReadByte();
				WorldGen.SavedOreTiers.Copper = reader.ReadInt16();
				WorldGen.SavedOreTiers.Iron = reader.ReadInt16();
				WorldGen.SavedOreTiers.Silver = reader.ReadInt16();
				WorldGen.SavedOreTiers.Gold = reader.ReadInt16();
				WorldGen.SavedOreTiers.Cobalt = reader.ReadInt16();
				WorldGen.SavedOreTiers.Mythril = reader.ReadInt16();
				WorldGen.SavedOreTiers.Adamantite = reader.ReadInt16();
				if (num254)
				{
					Main.StartSlimeRain();
				}
				else
				{
					Main.StopSlimeRain();
				}
				Main.invasionType = reader.ReadSByte();
				Main.LobbyId = reader.ReadUInt64();
				Sandstorm.IntendedSeverity = reader.ReadSingle();
				if (!ModNet.AllowVanillaClients && Netplay.Connection.State > 4)
				{
					WorldIO.ReceiveModData(reader);
				}
				if (Netplay.Connection.State == 3)
				{
					Main.windSpeedCurrent = Main.windSpeedTarget;
					Netplay.Connection.State = 4;
				}
				Main.checkHalloween();
				Main.checkXMas();
			}
			break;
		case 8:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			NetMessage.TrySendData(7, whoAmI);
			int num220 = reader.ReadInt32();
			int num221 = reader.ReadInt32();
			bool flag10 = true;
			if (num220 == -1 || num221 == -1)
			{
				flag10 = false;
			}
			else if (num220 < 10 || num220 > Main.maxTilesX - 10)
			{
				flag10 = false;
			}
			else if (num221 < 10 || num221 > Main.maxTilesY - 10)
			{
				flag10 = false;
			}
			int num222 = Netplay.GetSectionX(Main.spawnTileX) - 2;
			int num223 = Netplay.GetSectionY(Main.spawnTileY) - 1;
			int num224 = num222 + 5;
			int num225 = num223 + 3;
			if (num222 < 0)
			{
				num222 = 0;
			}
			if (num224 >= Main.maxSectionsX)
			{
				num224 = Main.maxSectionsX;
			}
			if (num223 < 0)
			{
				num223 = 0;
			}
			if (num225 >= Main.maxSectionsY)
			{
				num225 = Main.maxSectionsY;
			}
			int num226 = (num224 - num222) * (num225 - num223);
			List<Point> list = new List<Point>();
			for (int num227 = num222; num227 < num224; num227++)
			{
				for (int num228 = num223; num228 < num225; num228++)
				{
					list.Add(new Point(num227, num228));
				}
			}
			int num229 = -1;
			int num231 = -1;
			if (flag10)
			{
				num220 = Netplay.GetSectionX(num220) - 2;
				num221 = Netplay.GetSectionY(num221) - 1;
				num229 = num220 + 5;
				num231 = num221 + 3;
				if (num220 < 0)
				{
					num220 = 0;
				}
				if (num229 >= Main.maxSectionsX)
				{
					num229 = Main.maxSectionsX - 1;
				}
				if (num221 < 0)
				{
					num221 = 0;
				}
				if (num231 >= Main.maxSectionsY)
				{
					num231 = Main.maxSectionsY - 1;
				}
				for (int num232 = num220; num232 <= num229; num232++)
				{
					for (int num233 = num221; num233 <= num231; num233++)
					{
						if (num232 < num222 || num232 >= num224 || num233 < num223 || num233 >= num225)
						{
							list.Add(new Point(num232, num233));
							num226++;
						}
					}
				}
			}
			PortalHelper.SyncPortalsOnPlayerJoin(whoAmI, 1, list, out var portalSections);
			num226 += portalSections.Count;
			if (Netplay.Clients[whoAmI].State == 2)
			{
				Netplay.Clients[whoAmI].State = 3;
			}
			NetMessage.TrySendData(9, whoAmI, -1, Lang.inter[44].ToNetworkText(), num226);
			Netplay.Clients[whoAmI].StatusText2 = Language.GetTextValue("Net.IsReceivingTileData");
			Netplay.Clients[whoAmI].StatusMax += num226;
			for (int num234 = num222; num234 < num224; num234++)
			{
				for (int num235 = num223; num235 < num225; num235++)
				{
					NetMessage.SendSection(whoAmI, num234, num235);
				}
			}
			if (flag10)
			{
				for (int num236 = num220; num236 <= num229; num236++)
				{
					for (int num237 = num221; num237 <= num231; num237++)
					{
						NetMessage.SendSection(whoAmI, num236, num237);
					}
				}
			}
			for (int num238 = 0; num238 < portalSections.Count; num238++)
			{
				NetMessage.SendSection(whoAmI, portalSections[num238].X, portalSections[num238].Y);
			}
			for (int num239 = 0; num239 < 400; num239++)
			{
				if (Main.item[num239].active)
				{
					NetMessage.TrySendData(21, whoAmI, -1, null, num239);
					NetMessage.TrySendData(22, whoAmI, -1, null, num239);
				}
			}
			for (int num240 = 0; num240 < 200; num240++)
			{
				if (Main.npc[num240].active)
				{
					NetMessage.TrySendData(23, whoAmI, -1, null, num240);
				}
			}
			for (int num242 = 0; num242 < 1000; num242++)
			{
				if (Main.projectile[num242].active && (Main.projPet[Main.projectile[num242].type] || Main.projectile[num242].netImportant))
				{
					NetMessage.TrySendData(27, whoAmI, -1, null, num242);
				}
			}
			for (int num243 = 0; num243 < 290; num243++)
			{
				NetMessage.TrySendData(83, whoAmI, -1, null, num243);
			}
			NetMessage.TrySendData(57, whoAmI);
			NetMessage.TrySendData(103);
			NetMessage.TrySendData(101, whoAmI);
			NetMessage.TrySendData(136, whoAmI);
			NetMessage.TrySendData(49, whoAmI);
			Main.BestiaryTracker.OnPlayerJoining(whoAmI);
			CreativePowerManager.Instance.SyncThingsToJoiningPlayer(whoAmI);
			Main.PylonSystem.OnPlayerJoining(whoAmI);
			break;
		}
		case 9:
			if (Main.netMode == 1)
			{
				Netplay.Connection.StatusMax += reader.ReadInt32();
				Netplay.Connection.StatusText = NetworkText.Deserialize(reader).ToString();
				BitsByte bitsByte26 = reader.ReadByte();
				BitsByte serverSpecialFlags = Netplay.Connection.ServerSpecialFlags;
				serverSpecialFlags[0] = bitsByte26[0];
				serverSpecialFlags[1] = bitsByte26[1];
				Netplay.Connection.ServerSpecialFlags = serverSpecialFlags;
			}
			break;
		case 10:
			if (Main.netMode == 1)
			{
				NetMessage.DecompressTileBlock(reader.BaseStream);
			}
			break;
		case 11:
			if (Main.netMode == 1)
			{
				WorldGen.SectionTileFrame(reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16());
			}
			break;
		case 12:
		{
			int num244 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num244 = whoAmI;
			}
			Player player13 = Main.player[num244];
			player13.SpawnX = reader.ReadInt16();
			player13.SpawnY = reader.ReadInt16();
			player13.respawnTimer = reader.ReadInt32();
			player13.numberOfDeathsPVE = reader.ReadInt16();
			player13.numberOfDeathsPVP = reader.ReadInt16();
			if (player13.respawnTimer > 0)
			{
				player13.dead = true;
			}
			PlayerSpawnContext playerSpawnContext = (PlayerSpawnContext)reader.ReadByte();
			player13.Spawn(playerSpawnContext);
			if (Main.netMode != 2 || Netplay.Clients[whoAmI].State < 3)
			{
				break;
			}
			if (Netplay.Clients[whoAmI].State == 3)
			{
				Netplay.Clients[whoAmI].State = 10;
				NetMessage.buffer[whoAmI].broadcast = true;
				NetMessage.SyncConnectedPlayer(whoAmI);
				bool flag11 = NetMessage.DoesPlayerSlotCountAsAHost(whoAmI);
				Main.countsAsHostForGameplay[whoAmI] = flag11;
				if (NetMessage.DoesPlayerSlotCountAsAHost(whoAmI))
				{
					NetMessage.TrySendData(139, whoAmI, -1, null, whoAmI, flag11.ToInt());
				}
				NetMessage.TrySendData(12, -1, whoAmI, null, whoAmI, (int)(byte)playerSpawnContext);
				NetMessage.TrySendData(74, whoAmI, -1, NetworkText.FromLiteral(Main.player[whoAmI].name), Main.anglerQuest);
				NetMessage.TrySendData(129, whoAmI);
				NetMessage.greetPlayer(whoAmI);
				if (Main.player[num244].unlockedBiomeTorches)
				{
					NPC nPC2 = new NPC();
					nPC2.SetDefaults(664);
					Main.BestiaryTracker.Kills.RegisterKill(nPC2);
				}
			}
			else
			{
				NetMessage.TrySendData(12, -1, whoAmI, null, whoAmI, (int)(byte)playerSpawnContext);
			}
			break;
		}
		case 13:
		{
			int num42 = reader.ReadByte();
			if (num42 != Main.myPlayer || Main.ServerSideCharacter)
			{
				if (Main.netMode == 2)
				{
					num42 = whoAmI;
				}
				Player player2 = Main.player[num42];
				BitsByte bitsByte30 = reader.ReadByte();
				BitsByte bitsByte31 = reader.ReadByte();
				BitsByte bitsByte32 = reader.ReadByte();
				BitsByte bitsByte2 = reader.ReadByte();
				player2.controlUp = bitsByte30[0];
				player2.controlDown = bitsByte30[1];
				player2.controlLeft = bitsByte30[2];
				player2.controlRight = bitsByte30[3];
				player2.controlJump = bitsByte30[4];
				player2.controlUseItem = bitsByte30[5];
				player2.direction = (bitsByte30[6] ? 1 : (-1));
				if (bitsByte31[0])
				{
					player2.pulley = true;
					player2.pulleyDir = (byte)((!bitsByte31[1]) ? 1u : 2u);
				}
				else
				{
					player2.pulley = false;
				}
				player2.vortexStealthActive = bitsByte31[3];
				player2.gravDir = (bitsByte31[4] ? 1 : (-1));
				player2.TryTogglingShield(bitsByte31[5]);
				player2.ghost = bitsByte31[6];
				player2.selectedItem = reader.ReadByte();
				player2.position = reader.ReadVector2();
				if (bitsByte31[2])
				{
					player2.velocity = reader.ReadVector2();
				}
				else
				{
					player2.velocity = Vector2.Zero;
				}
				if (bitsByte32[6])
				{
					player2.PotionOfReturnOriginalUsePosition = reader.ReadVector2();
					player2.PotionOfReturnHomePosition = reader.ReadVector2();
				}
				else
				{
					player2.PotionOfReturnOriginalUsePosition = null;
					player2.PotionOfReturnHomePosition = null;
				}
				player2.tryKeepingHoveringUp = bitsByte32[0];
				player2.IsVoidVaultEnabled = bitsByte32[1];
				player2.sitting.isSitting = bitsByte32[2];
				player2.downedDD2EventAnyDifficulty = bitsByte32[3];
				player2.isPettingAnimal = bitsByte32[4];
				player2.isTheAnimalBeingPetSmall = bitsByte32[5];
				player2.tryKeepingHoveringDown = bitsByte32[7];
				player2.sleeping.SetIsSleepingAndAdjustPlayerRotation(player2, bitsByte2[0]);
				player2.autoReuseAllWeapons = bitsByte2[1];
				player2.controlDownHold = bitsByte2[2];
				player2.isOperatingAnotherEntity = bitsByte2[3];
				player2.controlUseTile = bitsByte2[4];
				if (Main.netMode == 2 && Netplay.Clients[whoAmI].State == 10)
				{
					NetMessage.TrySendData(13, -1, whoAmI, null, num42);
				}
			}
			break;
		}
		case 14:
		{
			int num166 = reader.ReadByte();
			int num167 = reader.ReadByte();
			if (Main.netMode != 1)
			{
				break;
			}
			bool active = Main.player[num166].active;
			if (num167 == 1)
			{
				if (!Main.player[num166].active)
				{
					Main.player[num166] = new Player();
				}
				Main.player[num166].active = true;
			}
			else
			{
				Main.player[num166].active = false;
			}
			if (active != Main.player[num166].active)
			{
				if (Main.player[num166].active)
				{
					Player.Hooks.PlayerConnect(num166);
				}
				else
				{
					Player.Hooks.PlayerDisconnect(num166);
				}
			}
			break;
		}
		case 16:
		{
			int num43 = reader.ReadByte();
			if (num43 != Main.myPlayer || Main.ServerSideCharacter)
			{
				if (Main.netMode == 2)
				{
					num43 = whoAmI;
				}
				Player obj6 = Main.player[num43];
				obj6.statLife = reader.ReadInt16();
				obj6.statLifeMax = reader.ReadInt16();
				obj6.dead = obj6.statLife <= 0;
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(16, -1, whoAmI, null, num43);
				}
			}
			break;
		}
		case 17:
		{
			byte b13 = reader.ReadByte();
			int num84 = reader.ReadInt16();
			int num85 = reader.ReadInt16();
			short num86 = reader.ReadInt16();
			int num87 = reader.ReadByte();
			bool flag16 = num86 == 1;
			if (!WorldGen.InWorld(num84, num85, 3))
			{
				break;
			}
			if (Main.tile[num84, num85] == null)
			{
				Main.tile[num84, num85] = default(Tile);
			}
			if (Main.netMode == 2)
			{
				if (!flag16)
				{
					if (b13 == 0 || b13 == 2 || b13 == 4)
					{
						Netplay.Clients[whoAmI].SpamDeleteBlock += 1f;
					}
					if (b13 == 1 || b13 == 3)
					{
						Netplay.Clients[whoAmI].SpamAddBlock += 1f;
					}
				}
				if (!Netplay.Clients[whoAmI].TileSections[Netplay.GetSectionX(num84), Netplay.GetSectionY(num85)])
				{
					flag16 = true;
				}
			}
			if (b13 == 0)
			{
				WorldGen.KillTile(num84, num85, flag16);
				if (Main.netMode == 1 && !flag16)
				{
					HitTile.ClearAllTilesAtThisLocation(num84, num85);
				}
			}
			bool flag2 = false;
			if (b13 == 1)
			{
				bool forced = true;
				if (WorldGen.CheckTileBreakability2_ShouldTileSurvive(num84, num85))
				{
					flag2 = true;
					forced = false;
				}
				WorldGen.PlaceTile(num84, num85, num86, mute: false, forced, -1, num87);
			}
			if (b13 == 2)
			{
				WorldGen.KillWall(num84, num85, flag16);
			}
			if (b13 == 3)
			{
				WorldGen.PlaceWall(num84, num85, num86);
			}
			if (b13 == 4)
			{
				WorldGen.KillTile(num84, num85, flag16, effectOnly: false, noItem: true);
			}
			if (b13 == 5)
			{
				WorldGen.PlaceWire(num84, num85);
			}
			if (b13 == 6)
			{
				WorldGen.KillWire(num84, num85);
			}
			if (b13 == 7)
			{
				WorldGen.PoundTile(num84, num85);
			}
			if (b13 == 8)
			{
				WorldGen.PlaceActuator(num84, num85);
			}
			if (b13 == 9)
			{
				WorldGen.KillActuator(num84, num85);
			}
			if (b13 == 10)
			{
				WorldGen.PlaceWire2(num84, num85);
			}
			if (b13 == 11)
			{
				WorldGen.KillWire2(num84, num85);
			}
			if (b13 == 12)
			{
				WorldGen.PlaceWire3(num84, num85);
			}
			if (b13 == 13)
			{
				WorldGen.KillWire3(num84, num85);
			}
			if (b13 == 14)
			{
				WorldGen.SlopeTile(num84, num85, num86);
			}
			if (b13 == 15)
			{
				Minecart.FrameTrack(num84, num85, pound: true);
			}
			if (b13 == 16)
			{
				WorldGen.PlaceWire4(num84, num85);
			}
			if (b13 == 17)
			{
				WorldGen.KillWire4(num84, num85);
			}
			switch (b13)
			{
			case 18:
				Wiring.SetCurrentUser(whoAmI);
				Wiring.PokeLogicGate(num84, num85);
				Wiring.SetCurrentUser();
				return;
			case 19:
				Wiring.SetCurrentUser(whoAmI);
				Wiring.Actuate(num84, num85);
				Wiring.SetCurrentUser();
				return;
			case 20:
				if (WorldGen.InWorld(num84, num85, 2))
				{
					int type2 = Main.tile[num84, num85].type;
					WorldGen.KillTile(num84, num85, flag16);
					num86 = (short)((Main.tile[num84, num85].active() && Main.tile[num84, num85].type == type2) ? 1 : 0);
					if (Main.netMode == 2)
					{
						NetMessage.TrySendData(17, -1, -1, null, b13, num84, num85, num86, num87);
					}
				}
				return;
			case 21:
				WorldGen.ReplaceTile(num84, num85, (ushort)num86, num87);
				break;
			}
			if (b13 == 22)
			{
				WorldGen.ReplaceWall(num84, num85, (ushort)num86);
			}
			if (b13 == 23)
			{
				WorldGen.SlopeTile(num84, num85, num86);
				WorldGen.PoundTile(num84, num85);
			}
			if (Main.netMode == 2)
			{
				if (flag2)
				{
					NetMessage.SendTileSquare(-1, num84, num85, 5);
				}
				else if ((b13 != 1 && b13 != 21) || !TileID.Sets.Falling[num86] || Main.tile[num84, num85].active())
				{
					NetMessage.TrySendData(17, -1, whoAmI, null, b13, num84, num85, num86, num87);
				}
			}
			break;
		}
		case 18:
			if (Main.netMode == 1)
			{
				Main.dayTime = reader.ReadByte() == 1;
				Main.time = reader.ReadInt32();
				Main.sunModY = reader.ReadInt16();
				Main.moonModY = reader.ReadInt16();
			}
			break;
		case 19:
		{
			byte b2 = reader.ReadByte();
			int num106 = reader.ReadInt16();
			int num109 = reader.ReadInt16();
			if (WorldGen.InWorld(num106, num109, 3))
			{
				int num110 = ((reader.ReadByte() != 0) ? 1 : (-1));
				switch (b2)
				{
				case 0:
					WorldGen.OpenDoor(num106, num109, num110);
					break;
				case 1:
					WorldGen.CloseDoor(num106, num109, forced: true);
					break;
				case 2:
					WorldGen.ShiftTrapdoor(num106, num109, num110 == 1, 1);
					break;
				case 3:
					WorldGen.ShiftTrapdoor(num106, num109, num110 == 1, 0);
					break;
				case 4:
					WorldGen.ShiftTallGate(num106, num109, closing: false, forced: true);
					break;
				case 5:
					WorldGen.ShiftTallGate(num106, num109, closing: true, forced: true);
					break;
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(19, -1, whoAmI, null, b2, num106, num109, (num110 == 1) ? 1 : 0);
				}
			}
			break;
		}
		case 20:
		{
			int num139 = reader.ReadInt16();
			int num140 = reader.ReadInt16();
			ushort num142 = reader.ReadByte();
			ushort num143 = reader.ReadByte();
			byte b4 = reader.ReadByte();
			if (!WorldGen.InWorld(num139, num140, 3))
			{
				break;
			}
			TileChangeType type4 = TileChangeType.None;
			if (Enum.IsDefined(typeof(TileChangeType), b4))
			{
				type4 = (TileChangeType)b4;
			}
			if (MessageBuffer.OnTileChangeReceived != null)
			{
				MessageBuffer.OnTileChangeReceived(num139, num140, Math.Max(num142, num143), type4);
			}
			BitsByte bitsByte21 = (byte)0;
			BitsByte bitsByte22 = (byte)0;
			BitsByte bitsByte24 = (byte)0;
			Tile tile4 = default(Tile);
			for (int num144 = num139; num144 < num139 + num142; num144++)
			{
				for (int num145 = num140; num145 < num140 + num143; num145++)
				{
					if (Main.tile[num144, num145] == null)
					{
						Main.tile[num144, num145] = default(Tile);
					}
					tile4 = Main.tile[num144, num145];
					bool flag5 = tile4.active();
					bitsByte21 = reader.ReadByte();
					bitsByte22 = reader.ReadByte();
					bitsByte24 = reader.ReadByte();
					tile4.active(bitsByte21[0]);
					tile4.wall = (byte)(bitsByte21[2] ? 1u : 0u);
					bool flag6 = bitsByte21[3];
					if (Main.netMode != 2)
					{
						tile4.liquid = (byte)(flag6 ? 1u : 0u);
					}
					tile4.wire(bitsByte21[4]);
					tile4.halfBrick(bitsByte21[5]);
					tile4.actuator(bitsByte21[6]);
					tile4.inActive(bitsByte21[7]);
					tile4.wire2(bitsByte22[0]);
					tile4.wire3(bitsByte22[1]);
					if (bitsByte22[2])
					{
						tile4.color(reader.ReadByte());
					}
					if (bitsByte22[3])
					{
						tile4.wallColor(reader.ReadByte());
					}
					if (tile4.active())
					{
						int type5 = tile4.type;
						tile4.type = reader.ReadUInt16();
						if (Main.tileFrameImportant[tile4.type])
						{
							tile4.frameX = reader.ReadInt16();
							tile4.frameY = reader.ReadInt16();
						}
						else if (!flag5 || tile4.type != type5)
						{
							tile4.frameX = -1;
							tile4.frameY = -1;
						}
						byte b5 = 0;
						if (bitsByte22[4])
						{
							b5++;
						}
						if (bitsByte22[5])
						{
							b5 += 2;
						}
						if (bitsByte22[6])
						{
							b5 += 4;
						}
						tile4.slope(b5);
					}
					tile4.wire4(bitsByte22[7]);
					tile4.fullbrightBlock(bitsByte24[0]);
					tile4.fullbrightWall(bitsByte24[1]);
					tile4.invisibleBlock(bitsByte24[2]);
					tile4.invisibleWall(bitsByte24[3]);
					if (tile4.wall > 0)
					{
						tile4.wall = reader.ReadUInt16();
					}
					if (flag6)
					{
						tile4.liquid = reader.ReadByte();
						tile4.liquidType(reader.ReadByte());
					}
				}
			}
			WorldGen.RangeFrame(num139, num140, num139 + num142, num140 + num143);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(b, -1, whoAmI, null, num139, num140, (int)num142, (int)num143, b4);
			}
			break;
		}
		case 21:
		case 90:
		case 145:
		case 148:
		{
			int num8 = reader.ReadInt16();
			Vector2 position2 = reader.ReadVector2();
			Vector2 velocity3 = reader.ReadVector2();
			int stack3 = (ModNet.AllowVanillaClients ? reader.ReadInt16() : reader.Read7BitEncodedInt());
			int prefixWeWant2 = (ModNet.AllowVanillaClients ? reader.ReadByte() : reader.Read7BitEncodedInt());
			int num9 = reader.ReadByte();
			int num10 = reader.ReadInt16();
			bool shimmered = false;
			float shimmerTime = 0f;
			int timeLeftInWhichTheItemCannotBeTakenByEnemies = 0;
			if (b == 145)
			{
				shimmered = reader.ReadBoolean();
				shimmerTime = reader.ReadSingle();
			}
			if (b == 148)
			{
				timeLeftInWhichTheItemCannotBeTakenByEnemies = reader.ReadByte();
			}
			if (Main.netMode == 1)
			{
				if (num10 == 0)
				{
					Main.item[num8].active = false;
					break;
				}
				int num11 = num8;
				Item item5 = Main.item[num11];
				ItemSyncPersistentStats itemSyncPersistentStats = default(ItemSyncPersistentStats);
				itemSyncPersistentStats.CopyFrom(item5);
				bool newAndShiny = (item5.newAndShiny || item5.netID != num10) && ItemSlot.Options.HighlightNewItems && (num10 < 0 || !ItemID.Sets.NeverAppearsAsNewInInventory[num10]);
				item5.netDefaults(num10);
				item5.newAndShiny = newAndShiny;
				item5.Prefix(prefixWeWant2);
				item5.stack = stack3;
				ItemIO.ReceiveModData(item5, reader);
				item5.position = position2;
				item5.velocity = velocity3;
				item5.active = true;
				item5.shimmered = shimmered;
				item5.shimmerTime = shimmerTime;
				if (b == 90)
				{
					item5.instanced = true;
					item5.playerIndexTheItemIsReservedFor = Main.myPlayer;
					item5.keepTime = 600;
				}
				item5.timeLeftInWhichTheItemCannotBeTakenByEnemies = timeLeftInWhichTheItemCannotBeTakenByEnemies;
				item5.wet = Collision.WetCollision(item5.position, item5.width, item5.height);
				itemSyncPersistentStats.PasteInto(item5);
			}
			else
			{
				if (Main.timeItemSlotCannotBeReusedFor[num8] > 0)
				{
					break;
				}
				if (num10 == 0)
				{
					if (num8 < 400)
					{
						Main.item[num8].active = false;
						NetMessage.TrySendData(21, -1, -1, null, num8);
					}
					break;
				}
				bool flag13 = false;
				if (num8 == 400)
				{
					flag13 = true;
				}
				if (flag13)
				{
					Item item6 = new Item();
					item6.netDefaults(num10);
					num8 = Item.NewItem(new EntitySource_Sync(), (int)position2.X, (int)position2.Y, item6.width, item6.height, item6.type, stack3, noBroadcast: true);
				}
				Item item7 = Main.item[num8];
				item7.netDefaults(num10);
				item7.Prefix(prefixWeWant2);
				item7.stack = stack3;
				ItemIO.ReceiveModData(item7, reader);
				item7.position = position2;
				item7.velocity = velocity3;
				item7.active = true;
				item7.playerIndexTheItemIsReservedFor = Main.myPlayer;
				item7.timeLeftInWhichTheItemCannotBeTakenByEnemies = timeLeftInWhichTheItemCannotBeTakenByEnemies;
				if (b == 145)
				{
					item7.shimmered = shimmered;
					item7.shimmerTime = shimmerTime;
				}
				if (flag13)
				{
					NetMessage.TrySendData(b, -1, -1, null, num8);
					if (num9 == 0)
					{
						Main.item[num8].ownIgnore = whoAmI;
						Main.item[num8].ownTime = 100;
					}
					Main.item[num8].FindOwner(num8);
				}
				else
				{
					NetMessage.TrySendData(b, -1, whoAmI, null, num8);
				}
			}
			break;
		}
		case 22:
		{
			int num193 = reader.ReadInt16();
			int num194 = reader.ReadByte();
			if (Main.netMode != 2 || Main.item[num193].playerIndexTheItemIsReservedFor == whoAmI)
			{
				Main.item[num193].playerIndexTheItemIsReservedFor = num194;
				if (num194 == Main.myPlayer)
				{
					Main.item[num193].keepTime = 15;
				}
				else
				{
					Main.item[num193].keepTime = 0;
				}
				if (Main.netMode == 2)
				{
					Main.item[num193].playerIndexTheItemIsReservedFor = 255;
					Main.item[num193].keepTime = 15;
					NetMessage.TrySendData(22, -1, -1, null, num193);
				}
			}
			break;
		}
		case 23:
		{
			if (Main.netMode != 1)
			{
				break;
			}
			int num61 = reader.ReadInt16();
			Vector2 vector5 = reader.ReadVector2();
			Vector2 velocity5 = reader.ReadVector2();
			int num62 = reader.ReadUInt16();
			if (num62 == 65535)
			{
				num62 = 0;
			}
			BitsByte bitsByte3 = reader.ReadByte();
			BitsByte bitsByte4 = reader.ReadByte();
			float[] array2 = ReUseTemporaryNPCAI();
			for (int num63 = 0; num63 < NPC.maxAI; num63++)
			{
				if (bitsByte3[num63 + 2])
				{
					array2[num63] = reader.ReadSingle();
				}
				else
				{
					array2[num63] = 0f;
				}
			}
			int num64 = reader.ReadInt16();
			int? playerCountForMultiplayerDifficultyOverride = 1;
			if (bitsByte4[0])
			{
				playerCountForMultiplayerDifficultyOverride = reader.ReadByte();
			}
			float value8 = 1f;
			if (bitsByte4[2])
			{
				value8 = reader.ReadSingle();
			}
			int num65 = 0;
			if (!bitsByte3[7])
			{
				num65 = reader.ReadByte() switch
				{
					2 => reader.ReadInt16(), 
					4 => reader.ReadInt32(), 
					_ => reader.ReadSByte(), 
				};
			}
			int num67 = -1;
			NPC nPC4 = Main.npc[num61];
			if (nPC4.active && Main.multiplayerNPCSmoothingRange > 0 && Vector2.DistanceSquared(nPC4.position, vector5) < 640000f)
			{
				nPC4.netOffset += nPC4.position - vector5;
			}
			if (!nPC4.active || nPC4.netID != num64)
			{
				nPC4.netOffset *= 0f;
				if (nPC4.active)
				{
					num67 = nPC4.type;
				}
				nPC4.active = true;
				nPC4.SetDefaults(num64, new NPCSpawnParams
				{
					playerCountForMultiplayerDifficultyOverride = playerCountForMultiplayerDifficultyOverride,
					strengthMultiplierOverride = value8
				});
			}
			nPC4.position = vector5;
			nPC4.velocity = velocity5;
			nPC4.target = num62;
			nPC4.direction = (bitsByte3[0] ? 1 : (-1));
			nPC4.directionY = (bitsByte3[1] ? 1 : (-1));
			nPC4.spriteDirection = (bitsByte3[6] ? 1 : (-1));
			if (bitsByte3[7])
			{
				num65 = (nPC4.life = nPC4.lifeMax);
			}
			else
			{
				nPC4.life = num65;
			}
			if (num65 <= 0)
			{
				nPC4.active = false;
			}
			nPC4.SpawnedFromStatue = bitsByte4[1];
			if (nPC4.SpawnedFromStatue)
			{
				nPC4.value = 0f;
			}
			for (int num68 = 0; num68 < NPC.maxAI; num68++)
			{
				nPC4.ai[num68] = array2[num68];
			}
			if (num67 > -1 && num67 != nPC4.type)
			{
				nPC4.TransformVisuals(num67, nPC4.type);
			}
			if (num64 == 262)
			{
				NPC.plantBoss = num61;
			}
			if (num64 == 245)
			{
				NPC.golemBoss = num61;
			}
			if (num64 == 668)
			{
				NPC.deerclopsBoss = num61;
			}
			if (nPC4.type >= 0 && Main.npcCatchable[nPC4.type])
			{
				nPC4.releaseOwner = reader.ReadByte();
			}
			if (!ModNet.AllowVanillaClients)
			{
				NPCLoader.ReceiveExtraAI(nPC4, NPCLoader.ReadExtraAI(reader));
			}
			break;
		}
		case 27:
		{
			int num184 = reader.ReadInt16();
			Vector2 position = reader.ReadVector2();
			Vector2 velocity2 = reader.ReadVector2();
			int num185 = reader.ReadByte();
			int num186 = reader.ReadInt16();
			BitsByte bitsByte12 = reader.ReadByte();
			BitsByte bitsByte23 = (byte)(bitsByte12[2] ? reader.ReadByte() : 0);
			float[] array = ReUseTemporaryProjectileAI();
			array[0] = (bitsByte12[0] ? reader.ReadSingle() : 0f);
			array[1] = (bitsByte12[1] ? reader.ReadSingle() : 0f);
			int bannerIdToRespondTo = (bitsByte12[3] ? reader.ReadUInt16() : 0);
			int damage2 = (bitsByte12[4] ? reader.ReadInt16() : 0);
			float knockBack2 = (bitsByte12[5] ? reader.ReadSingle() : 0f);
			int originalDamage = (bitsByte12[6] ? reader.ReadInt16() : 0);
			int num187 = (bitsByte12[7] ? reader.ReadInt16() : (-1));
			if (num187 >= 1000)
			{
				num187 = -1;
			}
			array[2] = (bitsByte23[0] ? reader.ReadSingle() : 0f);
			byte[] extraAI = (bitsByte23[1] ? ProjectileLoader.ReadExtraAI(reader) : null);
			if (Main.netMode == 2)
			{
				if (num186 == 949)
				{
					num185 = 255;
				}
				else
				{
					num185 = whoAmI;
					if (Main.projHostile[num186])
					{
						break;
					}
				}
			}
			int num188 = 1000;
			for (int n = 0; n < 1000; n++)
			{
				if (Main.projectile[n].owner == num185 && Main.projectile[n].identity == num184 && Main.projectile[n].active)
				{
					num188 = n;
					break;
				}
			}
			if (num188 == 1000)
			{
				for (int num189 = 0; num189 < 1000; num189++)
				{
					if (!Main.projectile[num189].active)
					{
						num188 = num189;
						break;
					}
				}
			}
			if (num188 == 1000)
			{
				num188 = Projectile.FindOldestProjectile();
			}
			Projectile projectile = Main.projectile[num188];
			if (!projectile.active || projectile.type != num186)
			{
				projectile.SetDefaults(num186);
				if (Main.netMode == 2)
				{
					Netplay.Clients[whoAmI].SpamProjectile += 1f;
				}
			}
			projectile.identity = num184;
			projectile.position = position;
			projectile.velocity = velocity2;
			projectile.type = num186;
			projectile.damage = damage2;
			projectile.bannerIdToRespondTo = bannerIdToRespondTo;
			projectile.originalDamage = originalDamage;
			projectile.knockBack = knockBack2;
			projectile.owner = num185;
			for (int num191 = 0; num191 < Projectile.maxAI; num191++)
			{
				projectile.ai[num191] = array[num191];
			}
			if (num187 >= 0)
			{
				projectile.projUUID = num187;
				Main.projectileIdentity[num185, num187] = num188;
			}
			if (extraAI != null)
			{
				ProjectileLoader.ReceiveExtraAI(projectile, extraAI);
			}
			projectile.ProjectileFixDesperation();
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(27, -1, whoAmI, null, num188);
			}
			break;
		}
		case 28:
		{
			int num100 = reader.ReadInt16();
			int num101 = reader.Read7BitEncodedInt();
			NPC.HitInfo hit = default(NPC.HitInfo);
			if (num101 >= 0)
			{
				NPC.HitInfo hitInfo = new NPC.HitInfo();
				hitInfo.Damage = num101;
				hitInfo.SourceDamage = reader.Read7BitEncodedInt();
				hitInfo.DamageType = DamageClassLoader.DamageClasses[reader.Read7BitEncodedInt()];
				hitInfo.HitDirection = reader.ReadSByte();
				hitInfo.Knockback = reader.ReadSingle();
				hit = hitInfo;
				BitsByte flags = reader.ReadByte();
				hit.Crit = flags[0];
				hit.InstantKill = flags[1];
				hit.HideCombatText = flags[2];
			}
			if (Main.netMode == 2)
			{
				if (num101 < 0)
				{
					num101 = 0;
				}
				Main.npc[num100].PlayerInteraction(whoAmI);
			}
			if (num101 >= 0)
			{
				Main.npc[num100].StrikeNPC(hit, fromNet: true);
			}
			else
			{
				Main.npc[num100].life = 0;
				Main.npc[num100].HitEffect();
				Main.npc[num100].active = false;
			}
			if (Main.netMode != 2)
			{
				break;
			}
			NetMessage.SendStrikeNPC(Main.npc[num100], in hit, whoAmI);
			if (Main.npc[num100].life <= 0)
			{
				NetMessage.TrySendData(23, -1, -1, null, num100);
			}
			else
			{
				Main.npc[num100].netUpdate = true;
			}
			if (Main.npc[num100].realLife >= 0)
			{
				if (Main.npc[Main.npc[num100].realLife].life <= 0)
				{
					NetMessage.TrySendData(23, -1, -1, null, Main.npc[num100].realLife);
				}
				else
				{
					Main.npc[Main.npc[num100].realLife].netUpdate = true;
				}
			}
			break;
		}
		case 29:
		{
			int num250 = reader.ReadInt16();
			int num251 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num251 = whoAmI;
			}
			for (int num3 = 0; num3 < 1000; num3++)
			{
				if (Main.projectile[num3].owner == num251 && Main.projectile[num3].identity == num250 && Main.projectile[num3].active)
				{
					Main.projectile[num3].Kill();
					break;
				}
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(29, -1, whoAmI, null, num250, num251);
			}
			break;
		}
		case 30:
		{
			int num41 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num41 = whoAmI;
			}
			bool flag15 = reader.ReadBoolean();
			Main.player[num41].hostile = flag15;
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(30, -1, whoAmI, null, num41);
				LocalizedText obj7 = (flag15 ? Lang.mp[11] : Lang.mp[12]);
				ChatHelper.BroadcastChatMessage(color: Main.teamColor[Main.player[num41].team], text: NetworkText.FromKey(obj7.Key, Main.player[num41].name));
			}
			break;
		}
		case 31:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			int num129 = reader.ReadInt16();
			int num131 = reader.ReadInt16();
			int num132 = Chest.FindChest(num129, num131);
			if (num132 > -1 && Chest.UsingChest(num132) == -1)
			{
				for (int num133 = 0; num133 < 40; num133++)
				{
					NetMessage.TrySendData(32, whoAmI, -1, null, num132, num133);
				}
				NetMessage.TrySendData(33, whoAmI, -1, null, num132);
				Main.player[whoAmI].chest = num132;
				if (Main.myPlayer == whoAmI)
				{
					Main.recBigList = false;
				}
				NetMessage.TrySendData(80, -1, whoAmI, null, whoAmI, num132);
				if (Main.netMode == 2 && WorldGen.IsChestRigged(num129, num131))
				{
					Wiring.SetCurrentUser(whoAmI);
					Wiring.HitSwitch(num129, num131);
					Wiring.SetCurrentUser();
					NetMessage.TrySendData(59, -1, whoAmI, null, num129, num131);
				}
			}
			break;
		}
		case 32:
		{
			int num71 = reader.ReadInt16();
			int num72 = reader.ReadByte();
			int stack5 = (ModNet.AllowVanillaClients ? reader.ReadInt16() : (-1));
			int prefixWeWant3 = (ModNet.AllowVanillaClients ? reader.ReadByte() : (-1));
			int type16 = (ModNet.AllowVanillaClients ? reader.ReadInt16() : (-1));
			if (num71 >= 0 && num71 < 8000)
			{
				if (Main.chest[num71] == null)
				{
					Main.chest[num71] = new Chest();
				}
				if (Main.chest[num71].item[num72] == null)
				{
					Main.chest[num71].item[num72] = new Item();
				}
				if (!ModNet.AllowVanillaClients)
				{
					ItemIO.Receive(Main.chest[num71].item[num72], reader, readStack: true);
				}
				else
				{
					Main.chest[num71].item[num72].netDefaults(type16);
					Main.chest[num71].item[num72].Prefix(prefixWeWant3);
					Main.chest[num71].item[num72].stack = stack5;
				}
				Recipe.FindRecipes(canDelayCheck: true);
			}
			break;
		}
		case 33:
		{
			int num107 = reader.ReadInt16();
			int num179 = reader.ReadInt16();
			int num190 = reader.ReadInt16();
			int num198 = reader.ReadByte();
			string name = string.Empty;
			if (num198 != 0)
			{
				if (num198 <= 20)
				{
					name = reader.ReadString();
				}
				else if (num198 != 255)
				{
					num198 = 0;
				}
			}
			if (Main.netMode == 1)
			{
				Player player = Main.player[Main.myPlayer];
				if (player.chest == -1)
				{
					Main.playerInventory = true;
					SoundEngine.PlaySound(10);
				}
				else if (player.chest != num107 && num107 != -1)
				{
					Main.playerInventory = true;
					SoundEngine.PlaySound(12);
					Main.recBigList = false;
				}
				else if (player.chest != -1 && num107 == -1)
				{
					SoundEngine.PlaySound(11);
					Main.recBigList = false;
				}
				player.chest = num107;
				player.chestX = num179;
				player.chestY = num190;
				Recipe.FindRecipes(canDelayCheck: true);
				if (Main.tile[num179, num190].frameX >= 36 && Main.tile[num179, num190].frameX < 72)
				{
					AchievementsHelper.HandleSpecialEvent(Main.player[Main.myPlayer], 16);
				}
			}
			else
			{
				if (num198 != 0)
				{
					int chest = Main.player[whoAmI].chest;
					Chest chest2 = Main.chest[chest];
					chest2.name = name;
					NetMessage.TrySendData(69, -1, whoAmI, null, chest, chest2.x, chest2.y);
				}
				Main.player[whoAmI].chest = num107;
				Recipe.FindRecipes(canDelayCheck: true);
				NetMessage.TrySendData(80, -1, whoAmI, null, whoAmI, num107);
			}
			break;
		}
		case 34:
		{
			byte b3 = reader.ReadByte();
			int num112 = reader.ReadInt16();
			int num113 = reader.ReadInt16();
			int num114 = reader.ReadInt16();
			int num115 = reader.ReadInt16();
			if (Main.netMode == 2)
			{
				num115 = 0;
			}
			ushort modType = 0;
			if (b3 >= 100)
			{
				modType = reader.ReadUInt16();
			}
			if (Main.netMode == 2)
			{
				if (b3 % 100 == 0)
				{
					if (modType == 0)
					{
						modType = 21;
					}
					int num118 = WorldGen.PlaceChest(num112, num113, modType, notNearOtherChests: false, num114);
					if (num118 == -1)
					{
						NetMessage.TrySendData(34, whoAmI, -1, null, b3, num112, num113, num114, num118, modType);
						int itemSpawn = ((b3 < 100) ? Chest.chestItemSpawn[num114] : TileLoader.GetItemDropFromTypeAndStyle(modType, num114));
						if (itemSpawn > 0)
						{
							Item.NewItem(new EntitySource_TileBreak(num112, num113), num112 * 16, num113 * 16, 32, 32, itemSpawn, 1, noBroadcast: true);
						}
					}
					else
					{
						NetMessage.TrySendData(34, -1, -1, null, b3, num112, num113, num114, num118, modType);
					}
					break;
				}
				if (b3 % 100 == 1 && (Main.tile[num112, num113].type == 21 || (b3 == 101 && TileID.Sets.BasicChest[Main.tile[num112, num113].type])))
				{
					Tile tile = Main.tile[num112, num113];
					if (tile.frameX % 36 != 0)
					{
						num112--;
					}
					if (tile.frameY % 36 != 0)
					{
						num113--;
					}
					int number = Chest.FindChest(num112, num113);
					WorldGen.KillTile(num112, num113);
					if (!tile.active())
					{
						NetMessage.TrySendData(34, -1, -1, null, b3, num112, num113, 0f, number);
					}
					break;
				}
				if (b3 % 100 == 2)
				{
					if (modType == 0)
					{
						modType = 88;
					}
					int num116 = WorldGen.PlaceChest(num112, num113, modType, notNearOtherChests: false, num114);
					if (num116 == -1)
					{
						NetMessage.TrySendData(34, whoAmI, -1, null, b3, num112, num113, num114, num116, modType);
						int itemSpawn2 = ((b3 < 100) ? Chest.dresserItemSpawn[num114] : TileLoader.GetItemDropFromTypeAndStyle(modType, num114));
						if (itemSpawn2 > 0)
						{
							Item.NewItem(new EntitySource_TileBreak(num112, num113), num112 * 16, num113 * 16, 32, 32, itemSpawn2, 1, noBroadcast: true);
						}
					}
					else
					{
						NetMessage.TrySendData(34, -1, -1, null, b3, num112, num113, num114, num116, modType);
					}
					break;
				}
				if (b3 % 100 == 3 && (Main.tile[num112, num113].type == 88 || (b3 == 103 && TileID.Sets.BasicDresser[Main.tile[num112, num113].type])))
				{
					Tile tile2 = Main.tile[num112, num113];
					num112 -= tile2.frameX % 54 / 18;
					if (tile2.frameY % 36 != 0)
					{
						num113--;
					}
					int number2 = Chest.FindChest(num112, num113);
					WorldGen.KillTile(num112, num113);
					if (!tile2.active())
					{
						NetMessage.TrySendData(34, -1, -1, null, b3, num112, num113, 0f, number2);
					}
					break;
				}
				switch (b3)
				{
				case 4:
				{
					int num117 = WorldGen.PlaceChest(num112, num113, 467, notNearOtherChests: false, num114);
					if (num117 == -1)
					{
						NetMessage.TrySendData(34, whoAmI, -1, null, b3, num112, num113, num114, num117);
						Item.NewItem(new EntitySource_TileBreak(num112, num113), num112 * 16, num113 * 16, 32, 32, Chest.chestItemSpawn2[num114], 1, noBroadcast: true);
					}
					else
					{
						NetMessage.TrySendData(34, -1, -1, null, b3, num112, num113, num114, num117);
					}
					break;
				}
				case 5:
					if (Main.tile[num112, num113].type == 467)
					{
						Tile tile3 = Main.tile[num112, num113];
						if (tile3.frameX % 36 != 0)
						{
							num112--;
						}
						if (tile3.frameY % 36 != 0)
						{
							num113--;
						}
						int number3 = Chest.FindChest(num112, num113);
						WorldGen.KillTile(num112, num113);
						if (!tile3.active())
						{
							NetMessage.TrySendData(34, -1, -1, null, b3, num112, num113, 0f, number3);
						}
					}
					break;
				}
				break;
			}
			byte b16 = b3;
			if (b3 % 100 == 0)
			{
				if (num115 == -1)
				{
					WorldGen.KillTile(num112, num113);
					break;
				}
				SoundEngine.PlaySound(0, num112 * 16, num113 * 16);
				if (modType == 0)
				{
					modType = 21;
				}
				WorldGen.PlaceChestDirect(num112, num113, modType, num114, num115);
			}
			else if (b3 % 100 != 2)
			{
				if (b16 == 4)
				{
					if (num115 == -1)
					{
						WorldGen.KillTile(num112, num113);
						break;
					}
					SoundEngine.PlaySound(0, num112 * 16, num113 * 16);
					WorldGen.PlaceChestDirect(num112, num113, 467, num114, num115);
				}
				else
				{
					Chest.DestroyChestDirect(num112, num113, num115);
					WorldGen.KillTile(num112, num113);
				}
			}
			else if (num115 == -1)
			{
				WorldGen.KillTile(num112, num113);
			}
			else
			{
				SoundEngine.PlaySound(0, num112 * 16, num113 * 16);
				if (modType == 0)
				{
					modType = 88;
				}
				WorldGen.PlaceDresserDirect(num112, num113, modType, num114, num115);
			}
			break;
		}
		case 35:
		{
			int num51 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num51 = whoAmI;
			}
			int num52 = reader.ReadInt16();
			if (num51 != Main.myPlayer || Main.ServerSideCharacter)
			{
				Main.player[num51].HealEffect(num52);
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(35, -1, whoAmI, null, num51, num52);
			}
			break;
		}
		case 36:
		{
			int num4 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num4 = whoAmI;
			}
			Player player14 = Main.player[num4];
			bool flag12 = player14.zone5[0];
			player14.zone1 = reader.ReadByte();
			player14.zone2 = reader.ReadByte();
			player14.zone3 = reader.ReadByte();
			player14.zone4 = reader.ReadByte();
			player14.zone5 = reader.ReadByte();
			if (!ModNet.AllowVanillaClients)
			{
				BiomeLoader.ReceiveCustomBiomes(player14, reader);
			}
			player14.ZonePurity = player14.InZonePurity();
			if (Main.netMode == 2)
			{
				if (!flag12 && player14.zone5[0])
				{
					NPC.SpawnFaelings(num4);
				}
				NetMessage.TrySendData(36, -1, whoAmI, null, num4);
			}
			break;
		}
		case 37:
			if (Main.netMode == 1)
			{
				if (Main.autoPass)
				{
					NetMessage.TrySendData(38);
					Main.autoPass = false;
				}
				else
				{
					Netplay.ServerPassword = "";
					Main.menuMode = 31;
				}
			}
			break;
		case 38:
			if (Main.netMode != 2)
			{
				break;
			}
			if (reader.ReadString() == Netplay.ServerPassword)
			{
				Netplay.Clients[whoAmI].State = 1;
				if (ModNet.isModdedClient[whoAmI])
				{
					ModNet.SyncMods(whoAmI);
				}
				else
				{
					NetMessage.TrySendData(3, whoAmI);
				}
			}
			else
			{
				NetMessage.TrySendData(2, whoAmI, -1, Lang.mp[1].ToNetworkText());
			}
			break;
		case 39:
			if (Main.netMode == 1)
			{
				int num141 = reader.ReadInt16();
				Main.item[num141].playerIndexTheItemIsReservedFor = 255;
				NetMessage.TrySendData(22, -1, -1, null, num141);
			}
			break;
		case 40:
		{
			int num168 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num168 = whoAmI;
			}
			int npcIndex = reader.ReadInt16();
			Main.player[num168].SetTalkNPC(npcIndex, fromNet: true);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(40, -1, whoAmI, null, num168);
			}
			break;
		}
		case 41:
		{
			int num137 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num137 = whoAmI;
			}
			Player player6 = Main.player[num137];
			float itemRotation = reader.ReadSingle();
			reader.ReadInt16();
			player6.itemRotation = itemRotation;
			player6.channel = player6.inventory[player6.selectedItem].channel;
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(41, -1, whoAmI, null, num137);
			}
			break;
		}
		case 42:
		{
			int num111 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num111 = whoAmI;
			}
			else if (Main.myPlayer == num111 && !Main.ServerSideCharacter)
			{
				break;
			}
			int statMana = reader.ReadInt16();
			int statManaMax = reader.ReadInt16();
			Main.player[num111].statMana = statMana;
			Main.player[num111].statManaMax = statManaMax;
			break;
		}
		case 43:
		{
			int num57 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num57 = whoAmI;
			}
			int num58 = reader.ReadInt16();
			if (num57 != Main.myPlayer)
			{
				Main.player[num57].ManaEffect(num58);
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(43, -1, whoAmI, null, num57, num58);
			}
			break;
		}
		case 45:
		{
			int num21 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num21 = whoAmI;
			}
			int num22 = reader.ReadByte();
			Player player15 = Main.player[num21];
			int team = player15.team;
			player15.team = num22;
			Color color = Main.teamColor[num22];
			if (Main.netMode != 2)
			{
				break;
			}
			NetMessage.TrySendData(45, -1, whoAmI, null, num21);
			LocalizedText localizedText = Lang.mp[13 + num22];
			if (num22 == 5)
			{
				localizedText = Lang.mp[22];
			}
			for (int num23 = 0; num23 < 255; num23++)
			{
				if (num23 == whoAmI || (team > 0 && Main.player[num23].team == team) || (num22 > 0 && Main.player[num23].team == num22))
				{
					ChatHelper.SendChatMessageToClient(NetworkText.FromKey(localizedText.Key, player15.name), color, num23);
				}
			}
			break;
		}
		case 46:
			if (Main.netMode == 2)
			{
				short i3 = reader.ReadInt16();
				int j3 = reader.ReadInt16();
				int num15 = Sign.ReadSign(i3, j3);
				if (num15 >= 0)
				{
					NetMessage.TrySendData(47, whoAmI, -1, null, num15, whoAmI);
				}
			}
			break;
		case 47:
		{
			int num230 = reader.ReadInt16();
			int x = reader.ReadInt16();
			int y = reader.ReadInt16();
			string text = reader.ReadString();
			int num241 = reader.ReadByte();
			BitsByte bitsByte = reader.ReadByte();
			if (num230 >= 0 && num230 < 1000)
			{
				string text2 = null;
				if (Main.sign[num230] != null)
				{
					text2 = Main.sign[num230].text;
				}
				Main.sign[num230] = new Sign();
				Main.sign[num230].x = x;
				Main.sign[num230].y = y;
				Sign.TextSign(num230, text);
				if (Main.netMode == 2 && text2 != text)
				{
					num241 = whoAmI;
					NetMessage.TrySendData(47, -1, whoAmI, null, num230, num241);
				}
				if (Main.netMode == 1 && num241 == Main.myPlayer && Main.sign[num230] != null && !bitsByte[0])
				{
					Main.playerInventory = false;
					Main.player[Main.myPlayer].SetTalkNPC(-1, fromNet: true);
					Main.npcChatCornerItem = 0;
					Main.editSign = false;
					SoundEngine.PlaySound(10);
					Main.player[Main.myPlayer].sign = num230;
					Main.npcChatText = Main.sign[num230].text;
				}
			}
			break;
		}
		case 48:
		{
			int num149 = reader.ReadInt16();
			int num150 = reader.ReadInt16();
			byte b6 = reader.ReadByte();
			byte liquidType = reader.ReadByte();
			if (Main.netMode == 2 && Netplay.SpamCheck)
			{
				int num151 = whoAmI;
				int num153 = (int)(Main.player[num151].position.X + (float)(Main.player[num151].width / 2));
				int num252 = (int)(Main.player[num151].position.Y + (float)(Main.player[num151].height / 2));
				int num154 = 10;
				int num155 = num153 - num154;
				int num156 = num153 + num154;
				int num157 = num252 - num154;
				int num158 = num252 + num154;
				if (num149 < num155 || num149 > num156 || num150 < num157 || num150 > num158)
				{
					Netplay.Clients[whoAmI].SpamWater += 1f;
				}
			}
			if (Main.tile[num149, num150] == null)
			{
				Main.tile[num149, num150] = default(Tile);
			}
			Main.tile[num149, num150].liquid = b6;
			Main.tile[num149, num150].liquidType(liquidType);
			if (Main.netMode == 2)
			{
				WorldGen.SquareTileFrame(num149, num150);
				if (b6 == 0)
				{
					NetMessage.SendData(48, -1, whoAmI, null, num149, num150);
				}
			}
			break;
		}
		case 49:
			if (Netplay.Connection.State == 6)
			{
				Netplay.Connection.State = 10;
				Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
			}
			break;
		case 50:
		{
			int num89 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num89 = whoAmI;
			}
			else if (num89 == Main.myPlayer && !Main.ServerSideCharacter)
			{
				break;
			}
			Player player4 = Main.player[num89];
			for (int num90 = 0; num90 < Player.maxBuffs; num90++)
			{
				player4.buffType[num90] = reader.ReadUInt16();
				if (player4.buffType[num90] > 0)
				{
					player4.buffTime[num90] = 60;
				}
				else
				{
					player4.buffTime[num90] = 0;
				}
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(50, -1, whoAmI, null, num89);
			}
			break;
		}
		case 51:
		{
			byte b14 = reader.ReadByte();
			byte b15 = reader.ReadByte();
			switch (b15)
			{
			case 1:
				NPC.SpawnSkeletron(b14);
				break;
			case 2:
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(51, -1, whoAmI, null, b14, (int)b15);
				}
				else
				{
					SoundEngine.PlaySound(SoundID.Item1, (int)Main.player[b14].position.X, (int)Main.player[b14].position.Y);
				}
				break;
			case 3:
				if (Main.netMode == 2)
				{
					Main.Sundialing();
				}
				break;
			case 4:
				Main.npc[b14].BigMimicSpawnSmoke();
				break;
			case 5:
				if (Main.netMode == 2)
				{
					NPC nPC5 = new NPC();
					nPC5.SetDefaults(664);
					Main.BestiaryTracker.Kills.RegisterKill(nPC5);
				}
				break;
			case 6:
				if (Main.netMode == 2)
				{
					Main.Moondialing();
				}
				break;
			}
			break;
		}
		case 52:
		{
			int num53 = reader.ReadByte();
			int num54 = reader.ReadInt16();
			int num56 = reader.ReadInt16();
			if (num53 == 1)
			{
				Chest.Unlock(num54, num56);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(52, -1, whoAmI, null, 0, num53, num54, num56);
					NetMessage.SendTileSquare(-1, num54, num56, 2);
				}
			}
			if (num53 == 2)
			{
				WorldGen.UnlockDoor(num54, num56);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(52, -1, whoAmI, null, 0, num53, num54, num56);
					NetMessage.SendTileSquare(-1, num54, num56, 2);
				}
			}
			if (num53 == 3)
			{
				Chest.Lock(num54, num56);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(52, -1, whoAmI, null, 0, num53, num54, num56);
					NetMessage.SendTileSquare(-1, num54, num56, 2);
				}
			}
			break;
		}
		case 53:
		{
			int num25 = reader.ReadInt16();
			int type14 = reader.ReadUInt16();
			int time2 = reader.ReadInt16();
			Main.npc[num25].AddBuff(type14, time2, quiet: true);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(54, -1, -1, null, num25);
			}
			break;
		}
		case 54:
			if (Main.netMode == 1)
			{
				int num12 = reader.ReadInt16();
				NPC nPC3 = Main.npc[num12];
				for (int num14 = 0; num14 < NPC.maxBuffs; num14++)
				{
					nPC3.buffType[num14] = reader.ReadUInt16();
					nPC3.buffTime[num14] = reader.ReadInt16();
				}
			}
			break;
		case 55:
		{
			int num196 = reader.ReadByte();
			int num197 = reader.ReadUInt16();
			int num199 = reader.ReadInt32();
			if (Main.netMode != 2 || num196 == whoAmI || Main.pvpBuff[num197])
			{
				if (Main.netMode == 1 && num196 == Main.myPlayer)
				{
					Main.player[num196].AddBuff(num197, num199);
				}
				else if (Main.netMode == 2)
				{
					NetMessage.TrySendData(55, -1, -1, null, num196, num197, num199);
				}
			}
			break;
		}
		case 56:
		{
			int num176 = reader.ReadInt16();
			if (num176 >= 0 && num176 < 200)
			{
				if (Main.netMode == 1)
				{
					string givenName = reader.ReadString();
					Main.npc[num176].GivenName = givenName;
					int townNpcVariationIndex = reader.ReadInt32();
					Main.npc[num176].townNpcVariationIndex = townNpcVariationIndex;
				}
				else if (Main.netMode == 2)
				{
					NetMessage.TrySendData(56, whoAmI, -1, null, num176);
				}
			}
			break;
		}
		case 57:
			if (Main.netMode == 1)
			{
				WorldGen.tGood = reader.ReadByte();
				WorldGen.tEvil = reader.ReadByte();
				WorldGen.tBlood = reader.ReadByte();
			}
			break;
		case 58:
		{
			int num169 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num169 = whoAmI;
			}
			float num170 = reader.ReadSingle();
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(58, -1, whoAmI, null, whoAmI, num170);
				break;
			}
			Player player7 = Main.player[num169];
			int type6 = player7.inventory[player7.selectedItem].type;
			switch (type6)
			{
			case 4057:
			case 4372:
			case 4715:
				player7.PlayGuitarChord(num170);
				break;
			case 4673:
				player7.PlayDrums(num170);
				break;
			default:
			{
				Main.musicPitch = num170;
				SoundStyle type7 = SoundID.Item26;
				if (type6 == 507)
				{
					type7 = SoundID.Item35;
				}
				if (type6 == 1305)
				{
					type7 = SoundID.Item47;
				}
				SoundEngine.PlaySound(in type7, player7.position);
				break;
			}
			}
			break;
		}
		case 59:
		{
			int num2 = reader.ReadInt16();
			int num13 = reader.ReadInt16();
			Wiring.SetCurrentUser(whoAmI);
			Wiring.HitSwitch(num2, num13);
			Wiring.SetCurrentUser();
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(59, -1, whoAmI, null, num2, num13);
			}
			break;
		}
		case 60:
		{
			int num163 = reader.ReadInt16();
			int num164 = reader.ReadInt16();
			int num165 = reader.ReadInt16();
			byte b7 = reader.ReadByte();
			if (num163 >= 200)
			{
				NetMessage.BootPlayer(whoAmI, NetworkText.FromKey("Net.CheatingInvalid"));
				break;
			}
			NPC nPC6 = Main.npc[num163];
			bool isLikeATownNPC = nPC6.isLikeATownNPC;
			if (Main.netMode == 1)
			{
				nPC6.homeless = b7 == 1;
				nPC6.homeTileX = num164;
				nPC6.homeTileY = num165;
			}
			if (!isLikeATownNPC)
			{
				break;
			}
			if (Main.netMode == 1)
			{
				switch (b7)
				{
				case 1:
					WorldGen.TownManager.KickOut(nPC6.type);
					break;
				case 2:
					WorldGen.TownManager.SetRoom(nPC6.type, num164, num165);
					break;
				}
			}
			else if (b7 == 1)
			{
				WorldGen.kickOut(num163);
			}
			else
			{
				WorldGen.moveRoom(num164, num165, num163);
			}
			break;
		}
		case 61:
		{
			int num125 = reader.ReadInt16();
			int num126 = reader.ReadInt16();
			if (Main.netMode != 2)
			{
				break;
			}
			if (num126 >= 0 && NPCID.Sets.MPAllowedEnemies[num126])
			{
				if (!NPC.AnyNPCs(num126))
				{
					NPC.SpawnOnPlayer(num125, num126);
				}
			}
			else if (num126 == -4)
			{
				if (!Main.dayTime && !DD2Event.Ongoing)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[31].Key), new Color(50, 255, 130));
					Main.startPumpkinMoon();
					NetMessage.TrySendData(7);
					NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 2f, 1f);
				}
			}
			else if (num126 == -5)
			{
				if (!Main.dayTime && !DD2Event.Ongoing)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[34].Key), new Color(50, 255, 130));
					Main.startSnowMoon();
					NetMessage.TrySendData(7);
					NetMessage.TrySendData(78, -1, -1, null, 0, 1f, 1f, 1f);
				}
			}
			else if (num126 == -6)
			{
				if (Main.dayTime && !Main.eclipse)
				{
					if (Main.remixWorld)
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[106].Key), new Color(50, 255, 130));
					}
					else
					{
						ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[20].Key), new Color(50, 255, 130));
					}
					Main.eclipse = true;
					NetMessage.TrySendData(7);
				}
			}
			else if (num126 == -7)
			{
				Main.invasionDelay = 0;
				Main.StartInvasion(4);
				NetMessage.TrySendData(7);
				NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
			}
			else if (num126 == -8)
			{
				if (NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger() && !NPC.AnyoneNearCultists())
				{
					WorldGen.StartImpendingDoom(720);
					NetMessage.TrySendData(7);
				}
			}
			else if (num126 == -10)
			{
				if (!Main.dayTime && !Main.bloodMoon)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[8].Key), new Color(50, 255, 130));
					Main.bloodMoon = true;
					if (Main.GetMoonPhase() == MoonPhase.Empty)
					{
						Main.moonPhase = 5;
					}
					AchievementsHelper.NotifyProgressionEvent(4);
					NetMessage.TrySendData(7);
				}
			}
			else if (num126 == -11)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookUsed"), new Color(50, 255, 130));
				NPC.combatBookWasUsed = true;
				NetMessage.TrySendData(7);
			}
			else if (num126 == -12)
			{
				NPC.UnlockOrExchangePet(ref NPC.boughtCat, 637, "Misc.LicenseCatUsed", num126);
			}
			else if (num126 == -13)
			{
				NPC.UnlockOrExchangePet(ref NPC.boughtDog, 638, "Misc.LicenseDogUsed", num126);
			}
			else if (num126 == -14)
			{
				NPC.UnlockOrExchangePet(ref NPC.boughtBunny, 656, "Misc.LicenseBunnyUsed", num126);
			}
			else if (num126 == -15)
			{
				NPC.UnlockOrExchangePet(ref NPC.unlockedSlimeBlueSpawn, 670, "Misc.LicenseSlimeUsed", num126);
			}
			else if (num126 == -16)
			{
				NPC.SpawnMechQueen(num125);
			}
			else if (num126 == -17)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookVolumeTwoUsed"), new Color(50, 255, 130));
				NPC.combatBookVolumeTwoWasUsed = true;
				NetMessage.TrySendData(7);
			}
			else if (num126 == -18)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PeddlersSatchelUsed"), new Color(50, 255, 130));
				NPC.peddlersSatchelWasUsed = true;
				NetMessage.TrySendData(7);
			}
			else if (num126 < 0)
			{
				int num127 = 1;
				if (num126 > -InvasionID.Count)
				{
					num127 = -num126;
				}
				if (num127 > 0 && Main.invasionType == 0)
				{
					Main.invasionDelay = 0;
					Main.StartInvasion(num127);
				}
				NetMessage.TrySendData(78, -1, -1, null, 0, 1f, Main.invasionType + 3);
			}
			break;
		}
		case 62:
		{
			int num73 = reader.ReadByte();
			int num74 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num73 = whoAmI;
			}
			if (num74 == 1)
			{
				Main.player[num73].NinjaDodge();
			}
			if (num74 == 2)
			{
				Main.player[num73].ShadowDodge();
			}
			if (num74 == 4)
			{
				Main.player[num73].BrainOfConfusionDodge();
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(62, -1, whoAmI, null, num73, num74);
			}
			break;
		}
		case 63:
		{
			int num49 = reader.ReadInt16();
			int num50 = reader.ReadInt16();
			byte b11 = reader.ReadByte();
			byte b12 = reader.ReadByte();
			if (b12 == 0)
			{
				WorldGen.paintTile(num49, num50, b11);
			}
			else
			{
				WorldGen.paintCoatTile(num49, num50, b11);
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(63, -1, whoAmI, null, num49, num50, (int)b11, (int)b12);
			}
			break;
		}
		case 64:
		{
			int num35 = reader.ReadInt16();
			int num36 = reader.ReadInt16();
			byte b9 = reader.ReadByte();
			byte b10 = reader.ReadByte();
			if (b10 == 0)
			{
				WorldGen.paintWall(num35, num36, b9);
			}
			else
			{
				WorldGen.paintCoatWall(num35, num36, b9);
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(64, -1, whoAmI, null, num35, num36, (int)b9, (int)b10);
			}
			break;
		}
		case 65:
		{
			BitsByte bitsByte29 = reader.ReadByte();
			int num200 = reader.ReadInt16();
			if (Main.netMode == 2)
			{
				num200 = whoAmI;
			}
			Vector2 vector = reader.ReadVector2();
			int num201 = 0;
			num201 = reader.ReadByte();
			int num202 = 0;
			if (bitsByte29[0])
			{
				num202++;
			}
			if (bitsByte29[1])
			{
				num202 += 2;
			}
			bool flag9 = false;
			if (bitsByte29[2])
			{
				flag9 = true;
			}
			int num203 = 0;
			if (bitsByte29[3])
			{
				num203 = reader.ReadInt32();
			}
			if (flag9)
			{
				vector = Main.player[num200].position;
			}
			switch (num202)
			{
			case 0:
				Main.player[num200].Teleport(vector, num201, num203);
				break;
			case 1:
				Main.npc[num200].Teleport(vector, num201, num203);
				break;
			case 2:
			{
				Main.player[num200].Teleport(vector, num201, num203);
				if (Main.netMode != 2)
				{
					break;
				}
				RemoteClient.CheckSection(whoAmI, vector);
				NetMessage.TrySendData(65, -1, -1, null, 0, num200, vector.X, vector.Y, num201, flag9.ToInt(), num203);
				int num204 = -1;
				float num205 = 9999f;
				for (int num206 = 0; num206 < 255; num206++)
				{
					if (Main.player[num206].active && num206 != whoAmI)
					{
						Vector2 vector2 = Main.player[num206].position - Main.player[whoAmI].position;
						if (((Vector2)(ref vector2)).Length() < num205)
						{
							num205 = ((Vector2)(ref vector2)).Length();
							num204 = num206;
						}
					}
				}
				if (num204 >= 0)
				{
					ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Game.HasTeleportedTo", Main.player[whoAmI].name, Main.player[num204].name), new Color(250, 250, 0));
				}
				break;
			}
			}
			if (Main.netMode == 2 && num202 == 0)
			{
				NetMessage.TrySendData(65, -1, whoAmI, null, num202, num200, vector.X, vector.Y, num201, flag9.ToInt(), num203);
			}
			break;
		}
		case 66:
		{
			int num180 = reader.ReadByte();
			int num181 = reader.ReadInt16();
			if (num181 > 0)
			{
				Player player10 = Main.player[num180];
				player10.statLife += num181;
				if (player10.statLife > player10.statLifeMax2)
				{
					player10.statLife = player10.statLifeMax2;
				}
				player10.HealEffect(num181, broadcast: false);
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(66, -1, whoAmI, null, num180, num181);
				}
			}
			break;
		}
		case 68:
			reader.ReadString();
			break;
		case 69:
		{
			int num159 = reader.ReadInt16();
			int num160 = reader.ReadInt16();
			int num161 = reader.ReadInt16();
			if (Main.netMode == 1)
			{
				if (num159 >= 0 && num159 < 8000)
				{
					Chest chest3 = Main.chest[num159];
					if (chest3 == null)
					{
						chest3 = new Chest();
						chest3.x = num160;
						chest3.y = num161;
						Main.chest[num159] = chest3;
					}
					else if (chest3.x != num160 || chest3.y != num161)
					{
						break;
					}
					chest3.name = reader.ReadString();
				}
			}
			else
			{
				if (num159 < -1 || num159 >= 8000)
				{
					break;
				}
				if (num159 == -1)
				{
					num159 = Chest.FindChest(num160, num161);
					if (num159 == -1)
					{
						break;
					}
				}
				Chest chest4 = Main.chest[num159];
				if (chest4.x == num160 && chest4.y == num161)
				{
					NetMessage.TrySendData(69, whoAmI, -1, null, num159, num160, num161);
				}
			}
			break;
		}
		case 70:
			if (Main.netMode == 2)
			{
				int num138 = reader.ReadInt16();
				int who = reader.ReadByte();
				if (Main.netMode == 2)
				{
					who = whoAmI;
				}
				if (num138 < 200 && num138 >= 0)
				{
					NPC.CatchNPC(num138, who);
				}
			}
			break;
		case 71:
			if (Main.netMode == 2)
			{
				int x8 = reader.ReadInt32();
				int y5 = reader.ReadInt32();
				int type3 = reader.ReadInt16();
				byte style3 = reader.ReadByte();
				NPC.ReleaseNPC(x8, y5, type3, style3, whoAmI);
			}
			break;
		case 72:
			if (Main.netMode == 1)
			{
				for (int num120 = 0; num120 < 40; num120++)
				{
					Main.travelShop[num120] = reader.ReadInt16();
				}
			}
			break;
		case 73:
			switch (reader.ReadByte())
			{
			case 0:
				Main.player[whoAmI].TeleportationPotion();
				break;
			case 1:
				Main.player[whoAmI].MagicConch();
				break;
			case 2:
				Main.player[whoAmI].DemonConch();
				break;
			case 3:
				Main.player[whoAmI].Shellphone_Spawn();
				break;
			}
			break;
		case 74:
			if (Main.netMode == 1)
			{
				Main.anglerQuest = reader.ReadByte();
				Main.anglerQuestFinished = reader.ReadBoolean();
			}
			break;
		case 75:
			if (Main.netMode == 2)
			{
				string name2 = Main.player[whoAmI].name;
				if (!Main.anglerWhoFinishedToday.Contains(name2))
				{
					Main.anglerWhoFinishedToday.Add(name2);
				}
			}
			break;
		case 76:
		{
			int num79 = reader.ReadByte();
			if (num79 != Main.myPlayer || Main.ServerSideCharacter)
			{
				if (Main.netMode == 2)
				{
					num79 = whoAmI;
				}
				Player obj8 = Main.player[num79];
				obj8.anglerQuestsFinished = reader.ReadInt32();
				obj8.golferScoreAccumulated = reader.ReadInt32();
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(76, -1, whoAmI, null, num79);
				}
			}
			break;
		}
		case 77:
		{
			short type17 = reader.ReadInt16();
			ushort tileType = reader.ReadUInt16();
			short x2 = reader.ReadInt16();
			short y3 = reader.ReadInt16();
			Animation.NewTemporaryAnimation(type17, tileType, x2, y3);
			break;
		}
		case 78:
			if (Main.netMode == 1)
			{
				Main.ReportInvasionProgress(reader.ReadInt32(), reader.ReadInt32(), reader.ReadSByte(), reader.ReadSByte());
			}
			break;
		case 79:
		{
			int x7 = reader.ReadInt16();
			int y14 = reader.ReadInt16();
			short type15 = reader.ReadInt16();
			int style2 = reader.ReadInt16();
			int num45 = reader.ReadByte();
			int random = reader.ReadSByte();
			int direction = (reader.ReadBoolean() ? 1 : (-1));
			if (Main.netMode == 2)
			{
				Netplay.Clients[whoAmI].SpamAddBlock += 1f;
				if (!WorldGen.InWorld(x7, y14, 10) || !Netplay.Clients[whoAmI].TileSections[Netplay.GetSectionX(x7), Netplay.GetSectionY(y14)])
				{
					break;
				}
			}
			WorldGen.PlaceObject(x7, y14, type15, mute: false, style2, num45, random, direction);
			if (Main.netMode == 2)
			{
				NetMessage.SendObjectPlacement(whoAmI, x7, y14, type15, style2, num45, random, direction);
			}
			break;
		}
		case 80:
			if (Main.netMode == 1)
			{
				int num30 = reader.ReadByte();
				int num31 = reader.ReadInt16();
				if (num31 >= -3 && num31 < 8000)
				{
					Main.player[num30].chest = num31;
					Recipe.FindRecipes(canDelayCheck: true);
				}
			}
			break;
		case 81:
			if (Main.netMode == 1)
			{
				int num256 = (int)reader.ReadSingle();
				int y12 = (int)reader.ReadSingle();
				CombatText.NewText(color: reader.ReadRGB(), amount: reader.ReadInt32(), location: new Rectangle(num256, y12, 0, 0));
			}
			break;
		case 119:
			if (Main.netMode == 1)
			{
				int num255 = (int)reader.ReadSingle();
				int y13 = (int)reader.ReadSingle();
				CombatText.NewText(color: reader.ReadRGB(), text: NetworkText.Deserialize(reader).ToString(), location: new Rectangle(num255, y13, 0, 0));
			}
			break;
		case 82:
			NetManager.Instance.Read(reader, whoAmI, length);
			break;
		case 83:
			if (Main.netMode == 1)
			{
				int num6 = reader.ReadInt16();
				int num7 = reader.ReadInt32();
				if (num6 >= 0)
				{
					NPC.killCount[num6] = num7;
				}
			}
			break;
		case 84:
		{
			int num5 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num5 = whoAmI;
			}
			float stealth = reader.ReadSingle();
			Main.player[num5].stealth = stealth;
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(84, -1, whoAmI, null, num5);
			}
			break;
		}
		case 85:
		{
			int num249 = whoAmI;
			int slot = reader.ReadInt16();
			if (Main.netMode == 2 && num249 < 255)
			{
				Chest.ServerPlaceItem(whoAmI, slot);
			}
			break;
		}
		case 86:
		{
			if (Main.netMode != 1)
			{
				break;
			}
			int num217 = reader.ReadInt32();
			if (!reader.ReadBoolean())
			{
				if (TileEntity.ByID.TryGetValue(num217, out var value3))
				{
					TileEntity.ByID.Remove(num217);
					TileEntity.ByPosition.Remove(value3.Position);
				}
			}
			else
			{
				TileEntity tileEntity = TileEntity.Read(reader, networkSend: true, lightSend: true);
				tileEntity.ID = num217;
				TileEntity.ByID[tileEntity.ID] = tileEntity;
				TileEntity.ByPosition[tileEntity.Position] = tileEntity;
			}
			break;
		}
		case 87:
			if (Main.netMode == 2)
			{
				int x6 = reader.ReadInt16();
				int y11 = reader.ReadInt16();
				int type11 = reader.ReadByte();
				if (WorldGen.InWorld(x6, y11) && !TileEntity.ByPosition.ContainsKey(new Point16(x6, y11)))
				{
					TileEntity.PlaceEntityNet(x6, y11, type11);
				}
			}
			break;
		case 88:
		{
			if (Main.netMode != 1)
			{
				break;
			}
			int num128 = reader.ReadInt16();
			if (num128 < 0 || num128 > 400)
			{
				break;
			}
			Item item8 = Main.item[num128];
			BitsByte bitsByte20 = reader.ReadByte();
			if (bitsByte20[0])
			{
				((Color)(ref item8.color)).PackedValue = reader.ReadUInt32();
			}
			if (bitsByte20[1])
			{
				item8.damage = reader.ReadUInt16();
			}
			if (bitsByte20[2])
			{
				item8.knockBack = reader.ReadSingle();
			}
			if (bitsByte20[3])
			{
				item8.useAnimation = reader.ReadUInt16();
			}
			if (bitsByte20[4])
			{
				item8.useTime = reader.ReadUInt16();
			}
			if (bitsByte20[5])
			{
				item8.shoot = reader.ReadInt16();
			}
			if (bitsByte20[6])
			{
				item8.shootSpeed = reader.ReadSingle();
			}
			if (bitsByte20[7])
			{
				bitsByte20 = reader.ReadByte();
				if (bitsByte20[0])
				{
					item8.width = reader.ReadInt16();
				}
				if (bitsByte20[1])
				{
					item8.height = reader.ReadInt16();
				}
				if (bitsByte20[2])
				{
					item8.scale = reader.ReadSingle();
				}
				if (bitsByte20[3])
				{
					item8.ammo = reader.ReadInt16();
				}
				if (bitsByte20[4])
				{
					item8.useAmmo = reader.ReadInt16();
				}
				if (bitsByte20[5])
				{
					item8.notAmmo = reader.ReadBoolean();
				}
			}
			break;
		}
		case 89:
			if (Main.netMode == 2)
			{
				short x11 = reader.ReadInt16();
				int y4 = reader.ReadInt16();
				Item item4;
				if (!ModNet.AllowVanillaClients)
				{
					item4 = ItemIO.Receive(reader);
					item4.stack = reader.Read7BitEncodedInt();
				}
				else
				{
					short setDefaultsToType3 = reader.ReadInt16();
					int prefix3 = reader.ReadByte();
					int stack6 = reader.ReadInt16();
					item4 = new Item(setDefaultsToType3, stack6, prefix3);
				}
				TEItemFrame.TryPlacing(x11, y4, item4, 1);
			}
			break;
		case 91:
		{
			if (Main.netMode != 1)
			{
				break;
			}
			int num94 = reader.ReadInt32();
			int num95 = reader.ReadByte();
			if (!ModNet.AllowVanillaClients && num95 == 2)
			{
				int owner = reader.ReadByte();
				num95 |= owner << 8;
			}
			if (num95 == 255)
			{
				if (EmoteBubble.byID.ContainsKey(num94))
				{
					EmoteBubble.byID.Remove(num94);
				}
				break;
			}
			int num96 = reader.ReadUInt16();
			int num97 = reader.ReadUInt16();
			int num98 = reader.ReadByte();
			int metadata = 0;
			if (num98 < 0)
			{
				metadata = reader.ReadInt16();
			}
			WorldUIAnchor worldUIAnchor = EmoteBubble.DeserializeNetAnchor(num95, num96);
			if (num95 == 1)
			{
				Main.player[num96].emoteTime = 360;
			}
			lock (EmoteBubble.byID)
			{
				if (!EmoteBubble.byID.ContainsKey(num94))
				{
					EmoteBubble.byID[num94] = new EmoteBubble(num98, worldUIAnchor, num97);
				}
				else
				{
					EmoteBubble.byID[num94].lifeTime = num97;
					EmoteBubble.byID[num94].lifeTimeStart = num97;
					EmoteBubble.byID[num94].emote = num98;
					EmoteBubble.byID[num94].anchor = worldUIAnchor;
				}
				EmoteBubble.byID[num94].ID = num94;
				EmoteBubble.byID[num94].metadata = metadata;
				EmoteBubble.OnBubbleChange(num94);
				EmoteBubbleLoader.OnSpawn(EmoteBubble.byID[num94]);
				break;
			}
		}
		case 92:
		{
			int num80 = reader.ReadInt16();
			int num81 = reader.ReadInt32();
			float num82 = reader.ReadSingle();
			float num83 = reader.ReadSingle();
			if (num80 >= 0 && num80 <= 200)
			{
				if (Main.netMode == 1)
				{
					Main.npc[num80].moneyPing(new Vector2(num82, num83));
					Main.npc[num80].extraValue = num81;
				}
				else
				{
					Main.npc[num80].extraValue += num81;
					NetMessage.TrySendData(92, -1, -1, null, num80, Main.npc[num80].extraValue, num82, num83);
				}
			}
			break;
		}
		case 95:
		{
			ushort num75 = reader.ReadUInt16();
			int num76 = reader.ReadByte();
			if (Main.netMode != 2)
			{
				break;
			}
			for (int num78 = 0; num78 < 1000; num78++)
			{
				if (Main.projectile[num78].owner == num75 && Main.projectile[num78].active && Main.projectile[num78].type == 602 && Main.projectile[num78].ai[1] == (float)num76)
				{
					Main.projectile[num78].Kill();
					NetMessage.TrySendData(29, -1, -1, null, Main.projectile[num78].identity, (int)num75);
					break;
				}
			}
			break;
		}
		case 96:
		{
			int num69 = reader.ReadByte();
			Player obj5 = Main.player[num69];
			int num70 = reader.ReadInt16();
			Vector2 newPos2 = reader.ReadVector2();
			Vector2 velocity6 = reader.ReadVector2();
			int lastPortalColorIndex2 = num70 + ((num70 % 2 == 0) ? 1 : (-1));
			obj5.lastPortalColorIndex = lastPortalColorIndex2;
			obj5.Teleport(newPos2, 4, num70);
			obj5.velocity = velocity6;
			if (Main.netMode == 2)
			{
				NetMessage.SendData(96, -1, -1, null, num69, newPos2.X, newPos2.Y, num70);
			}
			break;
		}
		case 97:
			if (Main.netMode == 1)
			{
				AchievementsHelper.NotifyNPCKilledDirect(Main.player[Main.myPlayer], reader.ReadInt16());
			}
			break;
		case 98:
			if (Main.netMode == 1)
			{
				AchievementsHelper.NotifyProgressionEvent(reader.ReadInt16());
			}
			break;
		case 99:
		{
			int num46 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num46 = whoAmI;
			}
			Main.player[num46].MinionRestTargetPoint = reader.ReadVector2();
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(99, -1, whoAmI, null, num46);
			}
			break;
		}
		case 115:
		{
			int num40 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num40 = whoAmI;
			}
			Main.player[num40].MinionAttackTargetNPC = reader.ReadInt16();
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(115, -1, whoAmI, null, num40);
			}
			break;
		}
		case 100:
		{
			int num32 = reader.ReadUInt16();
			NPC obj4 = Main.npc[num32];
			int num34 = reader.ReadInt16();
			Vector2 newPos = reader.ReadVector2();
			Vector2 velocity4 = reader.ReadVector2();
			int lastPortalColorIndex = num34 + ((num34 % 2 == 0) ? 1 : (-1));
			obj4.lastPortalColorIndex = lastPortalColorIndex;
			obj4.Teleport(newPos, 4, num34);
			obj4.velocity = velocity4;
			obj4.netOffset *= 0f;
			break;
		}
		case 101:
			if (Main.netMode != 2)
			{
				NPC.ShieldStrengthTowerSolar = reader.ReadUInt16();
				NPC.ShieldStrengthTowerVortex = reader.ReadUInt16();
				NPC.ShieldStrengthTowerNebula = reader.ReadUInt16();
				NPC.ShieldStrengthTowerStardust = reader.ReadUInt16();
				if (NPC.ShieldStrengthTowerSolar < 0)
				{
					NPC.ShieldStrengthTowerSolar = 0;
				}
				if (NPC.ShieldStrengthTowerVortex < 0)
				{
					NPC.ShieldStrengthTowerVortex = 0;
				}
				if (NPC.ShieldStrengthTowerNebula < 0)
				{
					NPC.ShieldStrengthTowerNebula = 0;
				}
				if (NPC.ShieldStrengthTowerStardust < 0)
				{
					NPC.ShieldStrengthTowerStardust = 0;
				}
				if (NPC.ShieldStrengthTowerSolar > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerSolar = NPC.LunarShieldPowerMax;
				}
				if (NPC.ShieldStrengthTowerVortex > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerVortex = NPC.LunarShieldPowerMax;
				}
				if (NPC.ShieldStrengthTowerNebula > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerNebula = NPC.LunarShieldPowerMax;
				}
				if (NPC.ShieldStrengthTowerStardust > NPC.LunarShieldPowerMax)
				{
					NPC.ShieldStrengthTowerStardust = NPC.LunarShieldPowerMax;
				}
			}
			break;
		case 102:
		{
			int num207 = reader.ReadByte();
			ushort num208 = reader.ReadUInt16();
			Vector2 other = reader.ReadVector2();
			if (Main.netMode == 2)
			{
				num207 = whoAmI;
				NetMessage.TrySendData(102, -1, -1, null, num207, (int)num208, other.X, other.Y);
				break;
			}
			Player player11 = Main.player[num207];
			Vector2 spinningpoint = default(Vector2);
			Vector2 vector4 = default(Vector2);
			for (int num210 = 0; num210 < 255; num210++)
			{
				Player player12 = Main.player[num210];
				if (!player12.active || player12.dead || (player11.team != 0 && player11.team != player12.team) || !(player12.Distance(other) < 700f))
				{
					continue;
				}
				Vector2 value2 = player11.Center - player12.Center;
				Vector2 vector3 = Vector2.Normalize(value2);
				if (!vector3.HasNaNs())
				{
					int type12 = 90;
					float num211 = 0f;
					float num212 = (float)Math.PI / 15f;
					((Vector2)(ref spinningpoint))._002Ector(0f, -8f);
					((Vector2)(ref vector4))._002Ector(-3f);
					float num213 = 0f;
					float num214 = 0.005f;
					switch (num208)
					{
					case 179:
						type12 = 86;
						break;
					case 173:
						type12 = 90;
						break;
					case 176:
						type12 = 88;
						break;
					}
					for (int num215 = 0; (float)num215 < ((Vector2)(ref value2)).Length() / 6f; num215++)
					{
						Vector2 position3 = player12.Center + 6f * (float)num215 * vector3 + spinningpoint.RotatedBy(num211) + vector4;
						num211 += num212;
						int num216 = Dust.NewDust(position3, 6, 6, type12, 0f, 0f, 100, default(Color), 1.5f);
						Main.dust[num216].noGravity = true;
						Main.dust[num216].velocity = Vector2.Zero;
						num213 = (Main.dust[num216].fadeIn = num213 + num214);
						Dust obj3 = Main.dust[num216];
						obj3.velocity += vector3 * 1.5f;
					}
				}
				player12.NebulaLevelup(num208);
			}
			break;
		}
		case 103:
			if (Main.netMode == 1)
			{
				NPC.MaxMoonLordCountdown = reader.ReadInt32();
				NPC.MoonLordCountdown = reader.ReadInt32();
			}
			break;
		case 104:
			if (Main.netMode == 1 && Main.npcShop > 0)
			{
				Item[] item3 = Main.instance.shop[Main.npcShop].item;
				int num195 = reader.ReadByte();
				int type10 = reader.ReadInt16();
				int stack2 = reader.ReadInt16();
				int prefixWeWant = reader.ReadByte();
				int value = reader.ReadInt32();
				BitsByte bitsByte28 = reader.ReadByte();
				if (num195 < item3.Length)
				{
					item3[num195] = new Item();
					item3[num195].netDefaults(type10);
					item3[num195].stack = stack2;
					item3[num195].Prefix(prefixWeWant);
					item3[num195].value = value;
					item3[num195].buyOnce = bitsByte28[0];
				}
			}
			break;
		case 105:
			if (Main.netMode != 1)
			{
				short i2 = reader.ReadInt16();
				int j2 = reader.ReadInt16();
				bool on = reader.ReadBoolean();
				WorldGen.ToggleGemLock(i2, j2, on);
			}
			break;
		case 106:
			if (Main.netMode == 1)
			{
				HalfVector2 halfVector = default(HalfVector2);
				((HalfVector2)(ref halfVector)).PackedValue = reader.ReadUInt32();
				Utils.PoofOfSmoke(((HalfVector2)(ref halfVector)).ToVector2());
			}
			break;
		case 107:
			if (Main.netMode == 1)
			{
				Color c = reader.ReadRGB();
				string text3 = NetworkText.Deserialize(reader).ToString();
				int widthLimit = reader.ReadInt16();
				Main.NewTextMultiline(text3, force: false, c, widthLimit);
			}
			break;
		case 108:
			if (Main.netMode == 1)
			{
				int damage = reader.ReadInt16();
				float knockBack = reader.ReadSingle();
				int x5 = reader.ReadInt16();
				int y9 = reader.ReadInt16();
				int angle = reader.ReadInt16();
				int ammo = reader.ReadInt16();
				int num182 = reader.ReadByte();
				if (num182 == Main.myPlayer)
				{
					WorldGen.ShootFromCannon(x5, y9, angle, ammo, damage, knockBack, num182, fromWire: true);
				}
			}
			break;
		case 109:
			if (Main.netMode == 2)
			{
				short num253 = reader.ReadInt16();
				int y7 = reader.ReadInt16();
				int x4 = reader.ReadInt16();
				int y8 = reader.ReadInt16();
				byte toolMode3 = reader.ReadByte();
				int num178 = whoAmI;
				WiresUI.Settings.MultiToolMode toolMode2 = WiresUI.Settings.ToolMode;
				WiresUI.Settings.ToolMode = (WiresUI.Settings.MultiToolMode)toolMode3;
				Wiring.MassWireOperation(new Point((int)num253, y7), new Point(x4, y8), Main.player[num178]);
				WiresUI.Settings.ToolMode = toolMode2;
			}
			break;
		case 110:
		{
			if (Main.netMode != 1)
			{
				break;
			}
			int type = reader.ReadInt16();
			int num99 = reader.ReadInt16();
			int num108 = reader.ReadByte();
			if (num108 == Main.myPlayer)
			{
				Player player9 = Main.player[num108];
				for (int k = 0; k < num99; k++)
				{
					player9.ConsumeItem(type);
				}
				player9.wireOperationsCooldown = 0;
			}
			break;
		}
		case 111:
			if (Main.netMode == 2)
			{
				BirthdayParty.ToggleManualParty();
			}
			break;
		case 112:
		{
			int num33 = reader.ReadByte();
			int num44 = reader.ReadInt32();
			int num55 = reader.ReadInt32();
			int num66 = reader.ReadByte();
			int num77 = reader.ReadInt16();
			switch (num33)
			{
			case 1:
				if (Main.netMode == 1)
				{
					WorldGen.TreeGrowFX(num44, num55, num66, num77);
				}
				if (Main.netMode == 2)
				{
					NetMessage.TrySendData(b, -1, -1, null, num33, num44, num55, num66, num77);
				}
				break;
			case 2:
				NPC.FairyEffects(new Vector2((float)num44, (float)num55), num77);
				break;
			}
			break;
		}
		case 113:
		{
			int x3 = reader.ReadInt16();
			int y6 = reader.ReadInt16();
			if (Main.netMode == 2 && !Main.snowMoon && !Main.pumpkinMoon)
			{
				if (DD2Event.WouldFailSpawningHere(x3, y6))
				{
					DD2Event.FailureMessage(whoAmI);
				}
				DD2Event.SummonCrystal(x3, y6, whoAmI);
			}
			break;
		}
		case 114:
			if (Main.netMode == 1)
			{
				DD2Event.WipeEntities();
			}
			break;
		case 116:
			if (Main.netMode == 1)
			{
				DD2Event.TimeLeftBetweenWaves = reader.ReadInt32();
			}
			break;
		case 117:
		{
			int num146 = reader.ReadByte();
			bool directHurt = false;
			if (num146 == 255)
			{
				num146 = reader.ReadByte();
				directHurt = true;
			}
			if (Main.netMode == 2 && whoAmI != num146 && (!Main.player[num146].hostile || !Main.player[whoAmI].hostile))
			{
				break;
			}
			if (!directHurt)
			{
				PlayerDeathReason playerDeathReason2 = PlayerDeathReason.FromReader(reader);
				int damage3 = reader.ReadInt16();
				int num147 = reader.ReadByte() - 1;
				BitsByte bitsByte25 = reader.ReadByte();
				bool flag7 = bitsByte25[0];
				bool pvp2 = bitsByte25[1];
				int num148 = reader.ReadSByte();
				Main.player[num146].Hurt(playerDeathReason2, damage3, num147, pvp2, quiet: true, flag7, num148);
				if (Main.netMode == 2)
				{
					NetMessage.SendPlayerHurt(num146, playerDeathReason2, damage3, num147, flag7, pvp2, num148, -1, whoAmI);
				}
				break;
			}
			BitsByte pack = reader.ReadByte();
			Player.HurtInfo hurtInfo = new Player.HurtInfo();
			hurtInfo.DamageSource = PlayerDeathReason.FromReader(reader);
			hurtInfo.PvP = pack[0];
			hurtInfo.CooldownCounter = reader.ReadSByte();
			hurtInfo.Dodgeable = pack[1];
			hurtInfo.SourceDamage = reader.Read7BitEncodedInt();
			hurtInfo.Damage = reader.Read7BitEncodedInt();
			hurtInfo.HitDirection = reader.ReadSByte();
			hurtInfo.Knockback = reader.ReadSingle();
			hurtInfo.DustDisabled = pack[2];
			hurtInfo.SoundDisabled = pack[3];
			Player.HurtInfo args = hurtInfo;
			Main.player[num146].Hurt(args, quiet: true);
			if (Main.netMode == 2)
			{
				NetMessage.SendPlayerHurt(num146, args, whoAmI);
			}
			break;
		}
		case 118:
		{
			int num134 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num134 = whoAmI;
			}
			PlayerDeathReason playerDeathReason = PlayerDeathReason.FromReader(reader);
			int num135 = reader.ReadInt16();
			int num136 = reader.ReadByte() - 1;
			bool pvp = ((BitsByte)reader.ReadByte())[0];
			Main.player[num134].KillMe(playerDeathReason, num135, num136, pvp);
			if (Main.netMode == 2)
			{
				NetMessage.SendPlayerDeath(num134, playerDeathReason, num135, num136, pvp, -1, whoAmI);
			}
			break;
		}
		case 120:
		{
			int num123 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num123 = whoAmI;
			}
			int num124 = reader.ReadByte();
			if (num124 >= 0 && num124 < EmoteBubbleLoader.EmoteBubbleCount && Main.netMode == 2)
			{
				EmoteBubble.NewBubble(num124, new WorldUIAnchor((Entity)Main.player[num123]), 360);
				EmoteBubble.CheckForNPCsToReactToEmoteBubble(num124, Main.player[num123]);
			}
			break;
		}
		case 121:
		{
			int num91 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num91 = whoAmI;
			}
			int num92 = reader.ReadInt32();
			int num93 = reader.ReadByte();
			bool flag3 = false;
			if (num93 >= 8)
			{
				flag3 = true;
				num93 -= 8;
			}
			if (!TileEntity.ByID.TryGetValue(num92, out var value9))
			{
				reader.ReadInt32();
				reader.ReadByte();
				break;
			}
			if (num93 >= 8)
			{
				value9 = null;
			}
			if (value9 is TEDisplayDoll tEDisplayDoll)
			{
				tEDisplayDoll.ReadItem(num93, reader, flag3);
			}
			else
			{
				reader.ReadInt32();
				reader.ReadByte();
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(b, -1, num91, null, num91, num92, num93, flag3.ToInt());
			}
			break;
		}
		case 122:
		{
			int num59 = reader.ReadInt32();
			int num60 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num60 = whoAmI;
			}
			if (Main.netMode == 2)
			{
				if (num59 == -1)
				{
					Main.player[num60].tileEntityAnchor.Clear();
					NetMessage.TrySendData(b, -1, -1, null, num59, num60);
					break;
				}
				if (!TileEntity.IsOccupied(num59, out interactingPlayer) && TileEntity.ByID.TryGetValue(num59, out var value6))
				{
					Main.player[num60].tileEntityAnchor.Set(num59, value6.Position.X, value6.Position.Y);
					NetMessage.TrySendData(b, -1, -1, null, num59, num60);
				}
			}
			if (Main.netMode == 1)
			{
				TileEntity value7;
				if (num59 == -1)
				{
					Main.player[num60].tileEntityAnchor.Clear();
				}
				else if (TileEntity.ByID.TryGetValue(num59, out value7))
				{
					TileEntity.SetInteractionAnchor(Main.player[num60], value7.Position.X, value7.Position.Y, num59);
				}
			}
			break;
		}
		case 123:
			if (Main.netMode == 2)
			{
				short x10 = reader.ReadInt16();
				int y2 = reader.ReadInt16();
				Item item2;
				if (!ModNet.AllowVanillaClients)
				{
					item2 = ItemIO.Receive(reader, readStack: true);
				}
				else
				{
					short setDefaultsToType2 = reader.ReadInt16();
					int prefix2 = reader.ReadByte();
					int stack4 = reader.ReadInt16();
					item2 = new Item(setDefaultsToType2, stack4, prefix2);
				}
				TEWeaponsRack.TryPlacing(x10, y2, item2, item2.stack);
			}
			break;
		case 124:
		{
			int num37 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num37 = whoAmI;
			}
			int num38 = reader.ReadInt32();
			int num39 = reader.ReadByte();
			bool flag14 = false;
			if (num39 >= 2)
			{
				flag14 = true;
				num39 -= 2;
			}
			if (!TileEntity.ByID.TryGetValue(num38, out var value4))
			{
				reader.ReadInt32();
				reader.ReadByte();
				break;
			}
			if (num39 >= 2)
			{
				value4 = null;
			}
			if (value4 is TEHatRack tEHatRack)
			{
				tEHatRack.ReadItem(num39, reader, flag14);
			}
			else
			{
				reader.ReadInt32();
				reader.ReadByte();
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(b, -1, num37, null, num37, num38, num39, flag14.ToInt());
			}
			break;
		}
		case 125:
		{
			int num26 = reader.ReadByte();
			int num27 = reader.ReadInt16();
			int num28 = reader.ReadInt16();
			int num29 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num26 = whoAmI;
			}
			if (Main.netMode == 1)
			{
				Main.player[Main.myPlayer].GetOtherPlayersPickTile(num27, num28, num29);
			}
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(125, -1, num26, null, num26, num27, num28, num29);
			}
			break;
		}
		case 126:
			if (Main.netMode == 1)
			{
				NPC.RevengeManager.AddMarkerFromReader(reader);
			}
			break;
		case 127:
		{
			int markerUniqueID = reader.ReadInt32();
			if (Main.netMode == 1)
			{
				NPC.RevengeManager.DestroyMarker(markerUniqueID);
			}
			break;
		}
		case 128:
		{
			int num16 = reader.ReadByte();
			int num17 = reader.ReadUInt16();
			int num18 = reader.ReadUInt16();
			int num19 = reader.ReadUInt16();
			int num20 = reader.ReadUInt16();
			if (Main.netMode == 2)
			{
				NetMessage.SendData(128, -1, num16, null, num16, num19, num20, 0f, num17, num18);
			}
			else
			{
				GolfHelper.ContactListener.PutBallInCup_TextAndEffects(new Point(num17, num18), num16, num19, num20);
			}
			break;
		}
		case 129:
			if (Main.netMode == 1)
			{
				Main.FixUIScale();
				Main.TrySetPreparationState(Main.WorldPreparationState.ProcessingData);
			}
			break;
		case 130:
		{
			if (Main.netMode != 2)
			{
				break;
			}
			int num245 = reader.ReadUInt16();
			int num246 = reader.ReadUInt16();
			int num247 = reader.ReadInt16();
			if (num247 == 682)
			{
				if (NPC.unlockedSlimeRedSpawn)
				{
					break;
				}
				NPC.unlockedSlimeRedSpawn = true;
				NetMessage.TrySendData(7);
			}
			num245 *= 16;
			num246 *= 16;
			NPC nPC7 = new NPC();
			nPC7.SetDefaults(num247);
			int type13 = nPC7.type;
			int netID = nPC7.netID;
			int num248 = NPC.NewNPC(new EntitySource_FishedOut(Main.player[whoAmI]), num245, num246, num247);
			if (netID != type13)
			{
				Main.npc[num248].SetDefaults(netID);
				NetMessage.TrySendData(23, -1, -1, null, num248);
			}
			if (num247 == 682)
			{
				WorldGen.CheckAchievement_RealEstateAndTownSlimes();
			}
			break;
		}
		case 131:
			if (Main.netMode == 1)
			{
				int num218 = reader.ReadUInt16();
				NPC nPC = null;
				nPC = ((num218 >= 200) ? new NPC() : Main.npc[num218]);
				if (reader.ReadByte() == 1)
				{
					int time = reader.ReadInt32();
					int fromWho = reader.ReadInt16();
					nPC.GetImmuneTime(fromWho, time);
				}
			}
			break;
		case 132:
			if (Main.netMode == 1)
			{
				Point point = reader.ReadVector2().ToPoint();
				ushort key = reader.ReadUInt16();
				SoundStyle legacySoundStyle = SoundID.SoundByIndex[key];
				BitsByte bitsByte27 = reader.ReadByte();
				if (bitsByte27[0])
				{
					legacySoundStyle.Variants = new int[1] { reader.ReadInt32() };
				}
				if (bitsByte27[1])
				{
					legacySoundStyle.Volume = MathHelper.Clamp(reader.ReadSingle(), 0f, 1f);
				}
				if (bitsByte27[2])
				{
					legacySoundStyle.Pitch = MathHelper.Clamp(reader.ReadSingle(), 0f, 1f);
				}
				SoundEngine.PlaySound(in legacySoundStyle, point.ToVector2());
			}
			break;
		case 133:
			if (Main.netMode == 2)
			{
				short x9 = reader.ReadInt16();
				int y10 = reader.ReadInt16();
				Item item;
				if (!ModNet.AllowVanillaClients)
				{
					item = ItemIO.Receive(reader, readStack: true);
				}
				else
				{
					short setDefaultsToType = reader.ReadInt16();
					int prefix = reader.ReadByte();
					int stack = reader.ReadInt16();
					item = new Item(setDefaultsToType, stack, prefix);
				}
				TEFoodPlatter.TryPlacing(x9, y10, item, item.stack);
			}
			break;
		case 134:
		{
			int num192 = reader.ReadByte();
			int ladyBugLuckTimeLeft = reader.ReadInt32();
			float torchLuck = reader.ReadSingle();
			byte luckPotion = reader.ReadByte();
			bool hasGardenGnomeNearby = reader.ReadBoolean();
			float equipmentBasedLuckBonus = reader.ReadSingle();
			float coinLuck = reader.ReadSingle();
			if (Main.netMode == 2)
			{
				num192 = whoAmI;
			}
			Player obj2 = Main.player[num192];
			obj2.ladyBugLuckTimeLeft = ladyBugLuckTimeLeft;
			obj2.torchLuck = torchLuck;
			obj2.luckPotion = luckPotion;
			obj2.HasGardenGnomeNearby = hasGardenGnomeNearby;
			obj2.equipmentBasedLuckBonus = equipmentBasedLuckBonus;
			obj2.coinLuck = coinLuck;
			obj2.RecalculateLuck();
			if (Main.netMode == 2)
			{
				NetMessage.SendData(134, -1, num192, null, num192);
			}
			break;
		}
		case 135:
		{
			int num183 = reader.ReadByte();
			if (Main.netMode == 1)
			{
				Main.player[num183].immuneAlpha = 255;
			}
			break;
		}
		case 136:
		{
			for (int l = 0; l < 2; l++)
			{
				for (int m = 0; m < 3; m++)
				{
					NPC.cavernMonsterType[l, m] = reader.ReadUInt16();
				}
			}
			break;
		}
		case 137:
			if (Main.netMode == 2)
			{
				int num177 = reader.ReadInt16();
				int buffTypeToRemove = reader.ReadUInt16();
				if (num177 >= 0 && num177 < 200)
				{
					Main.npc[num177].RequestBuffRemoval(buffTypeToRemove);
				}
			}
			break;
		case 139:
			if (Main.netMode != 2)
			{
				int num173 = reader.ReadByte();
				bool flag = reader.ReadBoolean();
				Main.countsAsHostForGameplay[num173] = flag;
			}
			break;
		case 140:
		{
			int num152 = reader.ReadByte();
			int num162 = reader.ReadInt32();
			switch (num152)
			{
			case 0:
				if (Main.netMode == 1)
				{
					CreditsRollEvent.SetRemainingTimeDirect(num162);
				}
				break;
			case 1:
				if (Main.netMode == 2)
				{
					NPC.TransformCopperSlime(num162);
				}
				break;
			case 2:
				if (Main.netMode == 2)
				{
					NPC.TransformElderSlime(num162);
				}
				break;
			}
			break;
		}
		case 141:
		{
			LucyAxeMessage.MessageSource messageSource = (LucyAxeMessage.MessageSource)reader.ReadByte();
			byte b8 = reader.ReadByte();
			Vector2 velocity = reader.ReadVector2();
			int num119 = reader.ReadInt32();
			int num130 = reader.ReadInt32();
			if (Main.netMode == 2)
			{
				NetMessage.SendData(141, -1, whoAmI, null, (int)messageSource, (int)b8, velocity.X, velocity.Y, num119, num130);
			}
			else
			{
				LucyAxeMessage.CreateFromNet(messageSource, b8, new Vector2((float)num119, (float)num130), velocity);
			}
			break;
		}
		case 142:
		{
			int num88 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num88 = whoAmI;
			}
			Player obj = Main.player[num88];
			obj.piggyBankProjTracker.TryReading(reader);
			obj.voidLensChest.TryReading(reader);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(142, -1, whoAmI, null, num88);
			}
			break;
		}
		case 143:
			if (Main.netMode == 2)
			{
				DD2Event.AttemptToSkipWaitTime();
			}
			break;
		case 144:
			if (Main.netMode == 2)
			{
				NPC.HaveDryadDoStardewAnimation();
			}
			break;
		case 146:
			switch (reader.ReadByte())
			{
			case 0:
				Item.ShimmerEffect(reader.ReadVector2());
				break;
			case 1:
			{
				Vector2 coinPosition = reader.ReadVector2();
				int coinAmount = reader.ReadInt32();
				Main.player[Main.myPlayer].AddCoinLuck(coinPosition, coinAmount);
				break;
			}
			case 2:
			{
				int num24 = reader.ReadInt32();
				Main.npc[num24].SetNetShimmerEffect();
				break;
			}
			}
			break;
		case 147:
		{
			int num209 = reader.ReadByte();
			if (Main.netMode == 2)
			{
				num209 = whoAmI;
			}
			int num219 = reader.ReadByte();
			Main.player[num209].TrySwitchingLoadout(num219);
			ReadAccessoryVisibility(reader, Main.player[num209].hideVisibleAccessory);
			if (Main.netMode == 2)
			{
				NetMessage.TrySendData(b, -1, num209, null, num209, num219);
			}
			break;
		}
		case 249:
			ConfigManager.HandleInGameChangeConfigPacket(reader, whoAmI);
			break;
		case 250:
			ModNet.HandleModPacket(reader, whoAmI, length);
			break;
		case 251:
			if (Main.netMode == 1)
			{
				ModNet.SyncClientMods(reader);
				break;
			}
			ModNet.SendNetIDs(whoAmI);
			NetMessage.SendData(3, whoAmI);
			break;
		case 252:
			if (Main.netMode == 1)
			{
				ModNet.ReceiveMod(reader);
			}
			else
			{
				ModNet.SendMod(reader.ReadString(), whoAmI);
			}
			break;
		default:
			if (Netplay.Clients[whoAmI].State == 0)
			{
				NetMessage.BootPlayer(whoAmI, Lang.mp[2].ToNetworkText());
			}
			break;
		case 15:
		case 25:
		case 26:
		case 44:
		case 67:
		case 93:
			break;
		}
	}

	private static void ReadAccessoryVisibility(BinaryReader reader, bool[] hideVisibleAccessory)
	{
		ushort num = reader.ReadUInt16();
		for (int i = 0; i < hideVisibleAccessory.Length; i++)
		{
			hideVisibleAccessory[i] = (num & (1 << i)) != 0;
		}
	}

	private static void TrySendingItemArray(int plr, Item[] array, int slotStartIndex)
	{
		for (int i = 0; i < array.Length; i++)
		{
			NetMessage.TrySendData(5, -1, -1, null, plr, slotStartIndex + i, array[i].prefix);
		}
	}
}
