using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Default;

public sealed class ModAccessorySlotPlayer : ModPlayer
{
	internal static class NetHandler
	{
		public const byte InventorySlot = 1;

		public const byte VisualState = 2;

		public const byte Server = 2;

		public const byte Client = 1;

		public const byte SP = 0;

		public static void SendSlot(int toWho, int plr, int slot, Item item)
		{
			ModPacket p = ModLoaderMod.GetPacket(0);
			p.Write((byte)1);
			if (Main.netMode == 2)
			{
				p.Write((byte)plr);
			}
			p.Write((sbyte)slot);
			ItemIO.Send(item, p, writeStack: true);
			p.Send(toWho, plr);
		}

		private static void HandleSlot(BinaryReader r, int fromWho)
		{
			if (Main.netMode == 1)
			{
				fromWho = r.ReadByte();
			}
			ModAccessorySlotPlayer dPlayer = Main.player[fromWho].GetModPlayer<ModAccessorySlotPlayer>();
			sbyte slot = r.ReadSByte();
			Item item = ItemIO.Receive(r, readStack: true);
			SetSlot(slot, item, dPlayer);
			if (Main.netMode == 2)
			{
				SendSlot(-1, fromWho, slot, item);
			}
		}

		public static void SendVisualState(int toWho, int plr, int slot, bool hideVisual)
		{
			ModPacket p = ModLoaderMod.GetPacket(0);
			p.Write((byte)2);
			if (Main.netMode == 2)
			{
				p.Write((byte)plr);
			}
			p.Write((sbyte)slot);
			p.Write(hideVisual);
			p.Send(toWho, plr);
		}

		private static void HandleVisualState(BinaryReader r, int fromWho)
		{
			if (Main.netMode == 1)
			{
				fromWho = r.ReadByte();
			}
			ModAccessorySlotPlayer dPlayer = Main.player[fromWho].GetModPlayer<ModAccessorySlotPlayer>();
			sbyte slot = r.ReadSByte();
			dPlayer.exHideAccessory[slot] = r.ReadBoolean();
			if (Main.netMode == 2)
			{
				SendVisualState(-1, fromWho, slot, dPlayer.exHideAccessory[slot]);
			}
		}

		public static void HandlePacket(BinaryReader r, int fromWho)
		{
			switch (r.ReadByte())
			{
			case 1:
				HandleSlot(r, fromWho);
				break;
			case 2:
				HandleVisualState(r, fromWho);
				break;
			}
		}

		public static void SetSlot(sbyte slot, Item item, ModAccessorySlotPlayer dPlayer)
		{
			if (slot < 0)
			{
				dPlayer.exDyesAccessory[-(slot + 1)] = item;
			}
			else
			{
				dPlayer.exAccessorySlot[slot] = item;
			}
		}
	}

	internal Item[] exAccessorySlot;

	internal Item[] exDyesAccessory;

	internal bool[] exHideAccessory;

	internal Dictionary<string, int> slots = new Dictionary<string, int>();

	internal bool scrollSlots;

	internal int scrollbarSlotPosition;

	internal static AccessorySlotLoader Loader => LoaderManager.Get<AccessorySlotLoader>();

	public int SlotCount => slots.Count;

	public int LoadedSlotCount => Loader.TotalCount;

	public ModAccessorySlotPlayer()
	{
		foreach (ModAccessorySlot slot in Loader.list)
		{
			slots.Add(slot.FullName, slot.Type);
		}
		ResetAndSizeAccessoryArrays();
	}

	internal void ResetAndSizeAccessoryArrays()
	{
		int size = slots.Count;
		exAccessorySlot = new Item[2 * size];
		exDyesAccessory = new Item[size];
		exHideAccessory = new bool[size];
		for (int i = 0; i < size; i++)
		{
			exDyesAccessory[i] = new Item();
			exHideAccessory[i] = false;
			exAccessorySlot[i * 2] = new Item();
			exAccessorySlot[i * 2 + 1] = new Item();
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag["order"] = slots.Keys.ToList();
		tag["items"] = exAccessorySlot.Select(ItemIO.Save).ToList();
		tag["dyes"] = exDyesAccessory.Select(ItemIO.Save).ToList();
		tag["visible"] = exHideAccessory.ToList();
	}

	public override void LoadData(TagCompound tag)
	{
		List<string> order = tag.GetList<string>("order").ToList();
		foreach (string name in order)
		{
			if (!slots.ContainsKey(name))
			{
				slots.Add(name, slots.Count);
			}
		}
		ResetAndSizeAccessoryArrays();
		List<Item> items = tag.GetList<TagCompound>("items").Select(ItemIO.Load).ToList();
		List<Item> dyes = tag.GetList<TagCompound>("dyes").Select(ItemIO.Load).ToList();
		List<bool> visible = tag.GetList<bool>("visible").ToList();
		for (int i = 0; i < order.Count; i++)
		{
			int type = slots[order[i]];
			exDyesAccessory[type] = dyes[i];
			exHideAccessory[type] = visible[i];
			exAccessorySlot[type] = items[i];
			exAccessorySlot[type + SlotCount] = items[i + order.Count];
		}
	}

	/// <summary>
	/// Updates functional slot visibility information on the player for Mod Slots, in a similar fashion to Player.UpdateVisibleAccessories()
	/// </summary>
	public override void UpdateVisibleAccessories()
	{
		AccessorySlotLoader loader = LoaderManager.Get<AccessorySlotLoader>();
		for (int i = 0; i < SlotCount; i++)
		{
			if (loader.ModdedIsItemSlotUnlockedAndUsable(i, base.Player))
			{
				base.Player.UpdateVisibleAccessories(exAccessorySlot[i], exHideAccessory[i], i, modded: true);
			}
		}
	}

	/// <summary>
	/// Updates vanity slot information on the player for Mod Slots, in a similar fashion to Player.UpdateVisibleAccessories()
	/// </summary>
	public override void UpdateVisibleVanityAccessories()
	{
		AccessorySlotLoader loader = LoaderManager.Get<AccessorySlotLoader>();
		for (int i = 0; i < SlotCount; i++)
		{
			if (loader.ModdedIsItemSlotUnlockedAndUsable(i, base.Player))
			{
				int vanitySlot = i + SlotCount;
				if (!base.Player.ItemIsVisuallyIncompatible(exAccessorySlot[vanitySlot]))
				{
					base.Player.UpdateVisibleAccessory(vanitySlot, exAccessorySlot[vanitySlot], modded: true);
				}
			}
		}
	}

	/// <summary>
	/// Mirrors Player.UpdateDyes() for modded slots
	/// Runs On Player Select, so is Player instance sensitive!!!
	/// </summary>
	public void UpdateDyes(bool socialSlots)
	{
		AccessorySlotLoader loader = LoaderManager.Get<AccessorySlotLoader>();
		int num2 = (socialSlots ? SlotCount : 0);
		int end = (socialSlots ? (SlotCount * 2) : SlotCount);
		for (int i = num2; i < end; i++)
		{
			if (loader.ModdedIsItemSlotUnlockedAndUsable(i, base.Player))
			{
				int num = i % exDyesAccessory.Length;
				base.Player.UpdateItemDye(i < exDyesAccessory.Length, exHideAccessory[num], exAccessorySlot[i], exDyesAccessory[num]);
			}
		}
	}

	/// <summary>
	/// Runs a simplified version of Player.UpdateEquips for the Modded Accessory Slots
	/// </summary>
	public override void UpdateEquips()
	{
		AccessorySlotLoader loader = LoaderManager.Get<AccessorySlotLoader>();
		for (int i = 0; i < SlotCount; i++)
		{
			if (loader.ModdedIsItemSlotUnlockedAndUsable(i, base.Player))
			{
				loader.CustomUpdateEquips(i, base.Player);
			}
		}
	}

	public void DropItems(IEntitySource itemSource)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		AccessorySlotLoader loader = LoaderManager.Get<AccessorySlotLoader>();
		Vector2 pos = base.Player.position + base.Player.Size / 2f;
		for (int i = 0; i < SlotCount; i++)
		{
			if (loader.ModdedIsItemSlotUnlockedAndUsable(i, base.Player))
			{
				base.Player.DropItem(itemSource, pos, ref exAccessorySlot[i]);
				base.Player.DropItem(itemSource, pos, ref exAccessorySlot[i + SlotCount]);
				base.Player.DropItem(itemSource, pos, ref exDyesAccessory[i]);
			}
		}
	}

	public override void CopyClientState(ModPlayer targetCopy)
	{
		ModAccessorySlotPlayer defaultInv = (ModAccessorySlotPlayer)targetCopy;
		for (int i = 0; i < LoadedSlotCount; i++)
		{
			exAccessorySlot[i].CopyNetStateTo(defaultInv.exAccessorySlot[i]);
			exAccessorySlot[i + SlotCount].CopyNetStateTo(defaultInv.exAccessorySlot[i + LoadedSlotCount]);
			exDyesAccessory[i].CopyNetStateTo(defaultInv.exDyesAccessory[i]);
			defaultInv.exHideAccessory[i] = exHideAccessory[i];
		}
	}

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		for (int i = 0; i < LoadedSlotCount; i++)
		{
			NetHandler.SendSlot(toWho, base.Player.whoAmI, i, exAccessorySlot[i]);
			NetHandler.SendSlot(toWho, base.Player.whoAmI, i + LoadedSlotCount, exAccessorySlot[i + SlotCount]);
			NetHandler.SendSlot(toWho, base.Player.whoAmI, -i - 1, exDyesAccessory[i]);
			NetHandler.SendVisualState(toWho, base.Player.whoAmI, i, exHideAccessory[i]);
		}
	}

	public override void SendClientChanges(ModPlayer clientPlayer)
	{
		ModAccessorySlotPlayer clientInv = (ModAccessorySlotPlayer)clientPlayer;
		for (int i = 0; i < LoadedSlotCount; i++)
		{
			if (exAccessorySlot[i].IsNetStateDifferent(clientInv.exAccessorySlot[i]))
			{
				NetHandler.SendSlot(-1, base.Player.whoAmI, i, exAccessorySlot[i]);
			}
			if (exAccessorySlot[i + SlotCount].IsNetStateDifferent(clientInv.exAccessorySlot[i + LoadedSlotCount]))
			{
				NetHandler.SendSlot(-1, base.Player.whoAmI, i + LoadedSlotCount, exAccessorySlot[i + SlotCount]);
			}
			if (exDyesAccessory[i].IsNetStateDifferent(clientInv.exDyesAccessory[i]))
			{
				NetHandler.SendSlot(-1, base.Player.whoAmI, -i - 1, exDyesAccessory[i]);
			}
			if (exHideAccessory[i] != clientInv.exHideAccessory[i])
			{
				NetHandler.SendVisualState(-1, base.Player.whoAmI, i, exHideAccessory[i]);
			}
		}
	}
}
