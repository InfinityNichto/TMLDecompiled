using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Terraria.DataStructures;

public abstract class TileEntity
{
	public static TileEntitiesManager manager;

	public const int MaxEntitiesPerChunk = 1000;

	public static object EntityCreationLock = new object();

	public static Dictionary<int, TileEntity> ByID = new Dictionary<int, TileEntity>();

	public static Dictionary<Point16, TileEntity> ByPosition = new Dictionary<Point16, TileEntity>();

	public static int TileEntitiesNextID;

	public int ID;

	public Point16 Position;

	public byte type;

	public static event Action _UpdateStart;

	public static event Action _UpdateEnd;

	internal TileEntity()
	{
	}

	public static int AssignNewID()
	{
		return TileEntitiesNextID++;
	}

	public static void Clear()
	{
		ByID.Clear();
		ByPosition.Clear();
		TileEntitiesNextID = 0;
	}

	public static void UpdateStart()
	{
		if (TileEntity._UpdateStart != null)
		{
			TileEntity._UpdateStart();
		}
	}

	public static void UpdateEnd()
	{
		if (TileEntity._UpdateEnd != null)
		{
			TileEntity._UpdateEnd();
		}
	}

	public static void InitializeAll()
	{
		manager = new TileEntitiesManager();
		manager.RegisterAll();
		ModTileEntity.Initialize();
	}

	public static void PlaceEntityNet(int x, int y, int type)
	{
		if (WorldGen.InWorld(x, y) && !ByPosition.ContainsKey(new Point16(x, y)))
		{
			manager.NetPlaceEntity(type, x, y);
		}
	}

	public virtual void Update()
	{
	}

	public static void Write(BinaryWriter writer, TileEntity ent, bool networkSend = false, bool lightSend = false)
	{
		lightSend = lightSend && networkSend;
		writer.Write(ent.type);
		ent.WriteInner(writer, networkSend, lightSend);
	}

	public static TileEntity Read(BinaryReader reader, bool networkSend = false, bool lightSend = false)
	{
		lightSend = lightSend && networkSend;
		byte id = reader.ReadByte();
		TileEntity tileEntity = manager.GenerateInstance(id);
		tileEntity.type = id;
		tileEntity.ReadInner(reader, networkSend, lightSend);
		return tileEntity;
	}

	private void WriteInner(BinaryWriter writer, bool networkSend, bool lightSend)
	{
		if (!lightSend)
		{
			writer.Write(ID);
		}
		writer.Write(Position.X);
		writer.Write(Position.Y);
		if (networkSend && !ModNet.AllowVanillaClients)
		{
			NetSend(writer);
		}
		else
		{
			WriteExtraData(writer, lightSend);
		}
	}

	private void ReadInner(BinaryReader reader, bool networkSend, bool lightSend)
	{
		if (!lightSend)
		{
			ID = reader.ReadInt32();
		}
		Position = new Point16(reader.ReadInt16(), reader.ReadInt16());
		if (networkSend && !ModNet.AllowVanillaClients)
		{
			NetReceive(reader);
		}
		else
		{
			ReadExtraData(reader, lightSend);
		}
	}

	public virtual void WriteExtraData(BinaryWriter writer, bool networkSend)
	{
	}

	public virtual void ReadExtraData(BinaryReader reader, bool networkSend)
	{
	}

	public virtual void OnPlayerUpdate(Player player)
	{
	}

	public static bool IsOccupied(int id, out int interactingPlayer)
	{
		interactingPlayer = -1;
		for (int i = 0; i < 255; i++)
		{
			Player player = Main.player[i];
			if (player.active && !player.dead && player.tileEntityAnchor.interactEntityID == id)
			{
				interactingPlayer = i;
				return true;
			}
		}
		return false;
	}

	public virtual void OnInventoryDraw(Player player, SpriteBatch spriteBatch)
	{
	}

	public virtual string GetItemGamepadInstructions(int slot = 0)
	{
		return "";
	}

	public virtual bool TryGetItemGamepadOverrideInstructions(Item[] inv, int context, int slot, out string instruction)
	{
		instruction = null;
		return false;
	}

	public virtual bool OverrideItemSlotHover(Item[] inv, int context = 0, int slot = 0)
	{
		return false;
	}

	public virtual bool OverrideItemSlotLeftClick(Item[] inv, int context = 0, int slot = 0)
	{
		return false;
	}

	public static void BasicOpenCloseInteraction(Player player, int x, int y, int id)
	{
		player.CloseSign();
		int interactingPlayer;
		if (Main.netMode != 1)
		{
			Main.stackSplit = 600;
			player.GamepadEnableGrappleCooldown();
			if (IsOccupied(id, out interactingPlayer))
			{
				if (interactingPlayer == player.whoAmI)
				{
					Recipe.FindRecipes();
					SoundEngine.PlaySound(11);
					player.tileEntityAnchor.Clear();
				}
			}
			else
			{
				SetInteractionAnchor(player, x, y, id);
			}
			return;
		}
		Main.stackSplit = 600;
		player.GamepadEnableGrappleCooldown();
		if (IsOccupied(id, out interactingPlayer))
		{
			if (interactingPlayer == player.whoAmI)
			{
				Recipe.FindRecipes();
				SoundEngine.PlaySound(11);
				player.tileEntityAnchor.Clear();
				NetMessage.SendData(122, -1, -1, null, -1, Main.myPlayer);
			}
		}
		else
		{
			NetMessage.SendData(122, -1, -1, null, id, Main.myPlayer);
		}
	}

	public static void SetInteractionAnchor(Player player, int x, int y, int id)
	{
		player.chest = -1;
		player.SetTalkNPC(-1);
		if (player.whoAmI == Main.myPlayer)
		{
			Main.playerInventory = true;
			Main.recBigList = false;
			Main.CreativeMenu.CloseMenu();
			if (PlayerInput.GrappleAndInteractAreShared)
			{
				PlayerInput.Triggers.JustPressed.Grapple = false;
			}
			if (player.tileEntityAnchor.interactEntityID != -1)
			{
				SoundEngine.PlaySound(12);
			}
			else
			{
				SoundEngine.PlaySound(10);
			}
		}
		player.tileEntityAnchor.Set(id, x, y);
	}

	public virtual void RegisterTileEntityID(int assignedID)
	{
	}

	public virtual void NetPlaceEntityAttempt(int x, int y)
	{
	}

	public virtual bool IsTileValidForEntity(int x, int y)
	{
		return false;
	}

	public virtual TileEntity GenerateInstance()
	{
		return null;
	}

	/// <summary>
	/// Allows you to save custom data for this tile entity.
	/// <br />
	/// <br /><b>NOTE:</b> The provided tag is always empty by default, and is provided as an argument only for the sake of convenience and optimization.
	/// <br /><b>NOTE:</b> Try to only save data that isn't default values.
	/// </summary>
	/// <param name="tag"> The TagCompound to save data into. Note that this is always empty by default, and is provided as an argument only for the sake of convenience and optimization. </param>
	public virtual void SaveData(TagCompound tag)
	{
	}

	/// <summary>
	/// Allows you to load custom data that you have saved for this tile entity.
	/// <br /><b>Try to write defensive loading code that won't crash if something's missing.</b>
	/// </summary>
	/// <param name="tag"> The TagCompound to load data from. </param>
	public virtual void LoadData(TagCompound tag)
	{
	}

	/// <summary>
	/// Allows you to send custom data for this tile entity between client and server, which will be handled in <see cref="M:Terraria.DataStructures.TileEntity.NetReceive(System.IO.BinaryReader)" />.
	/// <br />Called while sending tile data (!lightSend) and when <see cref="F:Terraria.ID.MessageID.TileEntitySharing" /> is sent (lightSend).
	/// <br />Only called on the server.
	/// </summary>
	/// <param name="writer">The writer.</param>
	public virtual void NetSend(BinaryWriter writer)
	{
		WriteExtraData(writer, networkSend: true);
	}

	/// <summary>
	/// Receives custom data sent in the <see cref="M:Terraria.DataStructures.TileEntity.NetSend(System.IO.BinaryWriter)" /> hook.
	/// <br />Called while receiving tile data (!lightReceive) and when <see cref="F:Terraria.ID.MessageID.TileEntitySharing" /> is received (lightReceive).
	/// <br />Only called on the client.
	/// </summary>
	/// <param name="reader">The reader.</param>
	public virtual void NetReceive(BinaryReader reader)
	{
		ReadExtraData(reader, networkSend: true);
	}
}
