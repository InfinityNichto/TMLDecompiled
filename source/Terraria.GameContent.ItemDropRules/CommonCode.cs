using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Terraria.GameContent.ItemDropRules;

public static class CommonCode
{
	[Obsolete("Use DropItem(DropAttemptInfo, ...)", true)]
	public static void DropItemFromNPC(NPC npc, int itemId, int stack, bool scattered = false)
	{
		_DropItemFromNPC(npc, itemId, stack, scattered);
	}

	private static void _DropItemFromNPC(NPC npc, int itemId, int stack, bool scattered = false)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (itemId > 0 && itemId < ItemLoader.ItemCount)
		{
			int itemIndex = DropItem(npc.Hitbox, npc.GetItemSource_Loot(), itemId, stack, scattered);
			ModifyItemDropFromNPC(npc, itemIndex);
		}
	}

	public static void DropItem(DropAttemptInfo info, int item, int stack, bool scattered = false)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (info.npc != null)
		{
			_DropItemFromNPC(info.npc, item, stack, scattered);
		}
		else
		{
			DropItem(info.player.Hitbox, info.player.GetItemSource_OpenItem(info.item), item, stack, scattered);
		}
	}

	public static int DropItem(Rectangle rectangle, IEntitySource entitySource, int itemId, int stack, bool scattered)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		return DropItem(scattered ? new Vector2((float)(rectangle.X + Main.rand.Next(rectangle.Width + 1)), (float)(rectangle.Y + Main.rand.Next(rectangle.Height + 1))) : new Vector2((float)rectangle.X + (float)rectangle.Width / 2f, (float)rectangle.Y + (float)rectangle.Height / 2f), entitySource, itemId, stack);
	}

	public static int DropItem(Vector2 position, IEntitySource entitySource, int itemId, int stack)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		int number = Item.NewItem(entitySource, position, itemId, stack, noBroadcast: false, -1);
		if (Main.netMode == 1)
		{
			NetMessage.SendData(21, -1, -1, null, number, 1f);
		}
		return number;
	}

	public static void DropItemLocalPerClientAndSetNPCMoneyTo0(NPC npc, int itemId, int stack, bool interactionRequired = true)
	{
		if (itemId <= 0 || itemId >= ItemLoader.ItemCount)
		{
			return;
		}
		if (Main.netMode == 2)
		{
			int num = Item.NewItem(npc.GetItemSource_Loot(), (int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, itemId, stack, noBroadcast: true, -1);
			Main.timeItemSlotCannotBeReusedFor[num] = 54000;
			for (int i = 0; i < 255; i++)
			{
				if (Main.player[i].active && (npc.playerInteraction[i] || !interactionRequired))
				{
					NetMessage.SendData(90, i, -1, null, num);
				}
			}
			Main.item[num].active = false;
		}
		else
		{
			_DropItemFromNPC(npc, itemId, stack);
		}
		npc.value = 0f;
	}

	public static void DropItemForEachInteractingPlayerOnThePlayer(NPC npc, int itemId, UnifiedRandom rng, int chanceNumerator, int chanceDenominator, int stack = 1, bool interactionRequired = true)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (itemId <= 0 || itemId >= ItemLoader.ItemCount)
		{
			return;
		}
		if (Main.netMode == 2)
		{
			for (int i = 0; i < 255; i++)
			{
				Player player = Main.player[i];
				if (player.active && (npc.playerInteraction[i] || !interactionRequired) && rng.Next(chanceDenominator) < chanceNumerator)
				{
					int itemIndex = Item.NewItem(npc.GetItemSource_Loot(), player.position, player.Size, itemId, stack, noBroadcast: false, -1);
					ModifyItemDropFromNPC(npc, itemIndex);
				}
			}
		}
		else if (rng.Next(chanceDenominator) < chanceNumerator)
		{
			_DropItemFromNPC(npc, itemId, stack);
		}
		npc.value = 0f;
	}

	public static void ModifyItemDropFromNPC(NPC npc, int itemIndex)
	{
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Item item = Main.item[itemIndex];
		switch (item.type)
		{
		case 23:
			if (npc.type == 1 && npc.netID != -1 && npc.netID != -2 && npc.netID != -5 && npc.netID != -6)
			{
				item.color = npc.color;
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
			}
			if (Main.remixWorld && npc.type == 59)
			{
				item.color = new Color(255, 127, 0);
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
			}
			break;
		case 319:
			switch (npc.netID)
			{
			case 542:
				item.color = new Color(189, 148, 96, 255);
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
				break;
			case 543:
				item.color = new Color(112, 85, 89, 255);
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
				break;
			case 544:
				item.color = new Color(145, 27, 40, 255);
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
				break;
			case 545:
				item.color = new Color(158, 113, 164, 255);
				NetMessage.SendData(88, -1, -1, null, itemIndex, 1f);
				break;
			}
			break;
		}
	}
}
