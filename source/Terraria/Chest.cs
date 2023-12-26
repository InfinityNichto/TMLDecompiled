using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Terraria;

/// <summary>
/// Represents a non-<see cref="T:Terraria.Player" /> inventory, such as chests, portable storage, or NPC shops.
/// </summary>
public class Chest : IFixLoadedData
{
	/// <summary>
	/// The maximum distance, in world coordinates, that a player can quick-stack items into chests.
	/// </summary>
	public const float chestStackRange = 600f;

	/// <summary>
	/// The number of vanilla chest styles contained in <see cref="F:Terraria.ID.TileID.Containers" />.
	/// </summary>
	public const int maxChestTypes = 52;

	/// <summary>
	/// Associates a <see cref="F:Terraria.ID.TileID.Containers" /> style with the item type (<see cref="F:Terraria.Item.type" />) that is shown when the chest is moused over. Differs from <see cref="F:Terraria.Chest.chestItemSpawn" /> to show key items for locked chest styles.
	/// <br /> <see cref="F:Terraria.Chest.maxChestTypes" /> elements long.
	/// </summary>
	public static int[] chestTypeToIcon = new int[52];

	/// <summary>
	/// Associates a <see cref="F:Terraria.ID.TileID.Containers" /> style with the item type (<see cref="F:Terraria.Item.type" />) that is dropped when the chest is destroyed.
	/// <br /> <see cref="F:Terraria.Chest.maxChestTypes" /> elements long.
	/// </summary>
	public static int[] chestItemSpawn = new int[52];

	/// <summary>
	/// The number of vanilla chest styles contained in <see cref="F:Terraria.ID.TileID.Containers2" />.
	/// </summary>
	public const int maxChestTypes2 = 17;

	/// <summary>
	/// Associates a <see cref="F:Terraria.ID.TileID.Containers2" /> style with the item type (<see cref="F:Terraria.Item.type" />) that is shown when the chest is moused over.
	/// <br /> <see cref="F:Terraria.Chest.maxChestTypes2" /> elements long. Differs from <see cref="F:Terraria.Chest.chestItemSpawn2" /> to show key items for locked chest styles.
	/// </summary>
	public static int[] chestTypeToIcon2 = new int[17];

	/// <summary>
	/// Associates a <see cref="F:Terraria.ID.TileID.Containers2" /> style with the item type (<see cref="F:Terraria.Item.type" />) that is dropped when the chest is destroyed.
	/// <br /> <see cref="F:Terraria.Chest.maxChestTypes2" /> elements long.
	/// </summary>
	public static int[] chestItemSpawn2 = new int[17];

	/// <summary>
	/// The number of vanilla dresser styles contained in <see cref="F:Terraria.ID.TileID.Dressers" />.
	/// </summary>
	public const int maxDresserTypes = 43;

	/// <summary>
	/// Associates a <see cref="F:Terraria.ID.TileID.Dressers" /> style with the item type (<see cref="F:Terraria.Item.type" />) that is shown when the dresser is moused over.
	/// <br /> <see cref="F:Terraria.Chest.maxDresserTypes" /> elements long.
	/// </summary>
	public static int[] dresserTypeToIcon = new int[43];

	/// <summary>
	/// Associates a <see cref="F:Terraria.ID.TileID.Dressers" /> style with the item type (<see cref="F:Terraria.Item.type" />) that is dropped when the dresser is destroyed.
	/// <br /> <see cref="F:Terraria.Chest.maxDresserTypes" /> elements long.
	/// </summary>
	public static int[] dresserItemSpawn = new int[43];

	/// <summary>
	/// The maximum number of items chests can contain.
	/// </summary>
	public const int maxItems = 40;

	/// <summary>
	/// The maximum length that <see cref="F:Terraria.Chest.name" /> is allowed to be.
	/// </summary>
	public const int MaxNameLength = 20;

	/// <summary>
	/// The items this chest contains.
	/// <br /> <see cref="F:Terraria.Chest.maxItems" /> elements long.
	/// </summary>
	public Item[] item;

	/// <summary>
	/// The x-coordinate of this chest in the world, in tile coordinates. (Top left corner tile)
	/// </summary>
	public int x;

	/// <summary>
	/// The y-coordinate of this chest in the world, in tile coordinates. (Top left corner tile)
	/// </summary>
	public int y;

	/// <summary>
	/// <strong>Unused.</strong>
	/// <br /> If <see langword="true" />, this chest is a portable storage (such as <see cref="F:Terraria.ID.ItemID.PiggyBank" />) linked to a player.
	/// </summary>
	public bool bankChest;

	/// <summary>
	/// The player-assigned name of this chest. May not exceed <see cref="F:Terraria.Chest.MaxNameLength" /> characters in length.
	/// </summary>
	public string name;

	/// <summary>
	/// The number of ticks this chest has been on its current <see cref="F:Terraria.Chest.frame" /> value.
	/// </summary>
	public int frameCounter;

	/// <summary>
	/// The current frame of this chest's animation.
	/// </summary>
	public int frame;

	/// <summary>
	/// The duration of this chest's eating animation in ticks.
	/// <br /> The eating animation plays when an item is quick-stacked into this chest.
	/// </summary>
	public int eatingAnimationTime;

	private static HashSet<int> _chestInUse = new HashSet<int>();

	/// <summary>
	/// Create a new <see cref="T:Terraria.Chest" /> with no name.
	/// </summary>
	public Chest(bool bank = false)
	{
		item = new Item[40];
		bankChest = bank;
		name = string.Empty;
	}

	public override string ToString()
	{
		int num = 0;
		for (int i = 0; i < item.Length; i++)
		{
			if (item[i].stack > 0)
			{
				num++;
			}
		}
		return $"{{X: {x}, Y: {y}, Count: {num}}}";
	}

	public static void Initialize()
	{
		int[] array = chestItemSpawn;
		int[] array3 = chestTypeToIcon;
		array3[0] = (array[0] = 48);
		array3[1] = (array[1] = 306);
		array3[2] = 327;
		array[2] = 306;
		array3[3] = (array[3] = 328);
		array3[4] = 329;
		array[4] = 328;
		array3[5] = (array[5] = 343);
		array3[6] = (array[6] = 348);
		array3[7] = (array[7] = 625);
		array3[8] = (array[8] = 626);
		array3[9] = (array[9] = 627);
		array3[10] = (array[10] = 680);
		array3[11] = (array[11] = 681);
		array3[12] = (array[12] = 831);
		array3[13] = (array[13] = 838);
		array3[14] = (array[14] = 914);
		array3[15] = (array[15] = 952);
		array3[16] = (array[16] = 1142);
		array3[17] = (array[17] = 1298);
		array3[18] = (array[18] = 1528);
		array3[19] = (array[19] = 1529);
		array3[20] = (array[20] = 1530);
		array3[21] = (array[21] = 1531);
		array3[22] = (array[22] = 1532);
		array3[23] = 1533;
		array[23] = 1528;
		array3[24] = 1534;
		array[24] = 1529;
		array3[25] = 1535;
		array[25] = 1530;
		array3[26] = 1536;
		array[26] = 1531;
		array3[27] = 1537;
		array[27] = 1532;
		array3[28] = (array[28] = 2230);
		array3[29] = (array[29] = 2249);
		array3[30] = (array[30] = 2250);
		array3[31] = (array[31] = 2526);
		array3[32] = (array[32] = 2544);
		array3[33] = (array[33] = 2559);
		array3[34] = (array[34] = 2574);
		array3[35] = (array[35] = 2612);
		array3[36] = 327;
		array[36] = 2612;
		array3[37] = (array[37] = 2613);
		array3[38] = 327;
		array[38] = 2613;
		array3[39] = (array[39] = 2614);
		array3[40] = 327;
		array[40] = 2614;
		array3[41] = (array[41] = 2615);
		array3[42] = (array[42] = 2616);
		array3[43] = (array[43] = 2617);
		array3[44] = (array[44] = 2618);
		array3[45] = (array[45] = 2619);
		array3[46] = (array[46] = 2620);
		array3[47] = (array[47] = 2748);
		array3[48] = (array[48] = 2814);
		array3[49] = (array[49] = 3180);
		array3[50] = (array[50] = 3125);
		array3[51] = (array[51] = 3181);
		int[] array2 = chestItemSpawn2;
		int[] array4 = chestTypeToIcon2;
		array4[0] = (array2[0] = 3884);
		array4[1] = (array2[1] = 3885);
		array4[2] = (array2[2] = 3939);
		array4[3] = (array2[3] = 3965);
		array4[4] = (array2[4] = 3988);
		array4[5] = (array2[5] = 4153);
		array4[6] = (array2[6] = 4174);
		array4[7] = (array2[7] = 4195);
		array4[8] = (array2[8] = 4216);
		array4[9] = (array2[9] = 4265);
		array4[10] = (array2[10] = 4267);
		array4[11] = (array2[11] = 4574);
		array4[12] = (array2[12] = 4712);
		array4[13] = 4714;
		array2[13] = 4712;
		array4[14] = (array2[14] = 5156);
		array4[15] = (array2[15] = 5177);
		array4[16] = (array2[16] = 5198);
		dresserTypeToIcon[0] = (dresserItemSpawn[0] = 334);
		dresserTypeToIcon[1] = (dresserItemSpawn[1] = 647);
		dresserTypeToIcon[2] = (dresserItemSpawn[2] = 648);
		dresserTypeToIcon[3] = (dresserItemSpawn[3] = 649);
		dresserTypeToIcon[4] = (dresserItemSpawn[4] = 918);
		dresserTypeToIcon[5] = (dresserItemSpawn[5] = 2386);
		dresserTypeToIcon[6] = (dresserItemSpawn[6] = 2387);
		dresserTypeToIcon[7] = (dresserItemSpawn[7] = 2388);
		dresserTypeToIcon[8] = (dresserItemSpawn[8] = 2389);
		dresserTypeToIcon[9] = (dresserItemSpawn[9] = 2390);
		dresserTypeToIcon[10] = (dresserItemSpawn[10] = 2391);
		dresserTypeToIcon[11] = (dresserItemSpawn[11] = 2392);
		dresserTypeToIcon[12] = (dresserItemSpawn[12] = 2393);
		dresserTypeToIcon[13] = (dresserItemSpawn[13] = 2394);
		dresserTypeToIcon[14] = (dresserItemSpawn[14] = 2395);
		dresserTypeToIcon[15] = (dresserItemSpawn[15] = 2396);
		dresserTypeToIcon[16] = (dresserItemSpawn[16] = 2529);
		dresserTypeToIcon[17] = (dresserItemSpawn[17] = 2545);
		dresserTypeToIcon[18] = (dresserItemSpawn[18] = 2562);
		dresserTypeToIcon[19] = (dresserItemSpawn[19] = 2577);
		dresserTypeToIcon[20] = (dresserItemSpawn[20] = 2637);
		dresserTypeToIcon[21] = (dresserItemSpawn[21] = 2638);
		dresserTypeToIcon[22] = (dresserItemSpawn[22] = 2639);
		dresserTypeToIcon[23] = (dresserItemSpawn[23] = 2640);
		dresserTypeToIcon[24] = (dresserItemSpawn[24] = 2816);
		dresserTypeToIcon[25] = (dresserItemSpawn[25] = 3132);
		dresserTypeToIcon[26] = (dresserItemSpawn[26] = 3134);
		dresserTypeToIcon[27] = (dresserItemSpawn[27] = 3133);
		dresserTypeToIcon[28] = (dresserItemSpawn[28] = 3911);
		dresserTypeToIcon[29] = (dresserItemSpawn[29] = 3912);
		dresserTypeToIcon[30] = (dresserItemSpawn[30] = 3913);
		dresserTypeToIcon[31] = (dresserItemSpawn[31] = 3914);
		dresserTypeToIcon[32] = (dresserItemSpawn[32] = 3934);
		dresserTypeToIcon[33] = (dresserItemSpawn[33] = 3968);
		dresserTypeToIcon[34] = (dresserItemSpawn[34] = 4148);
		dresserTypeToIcon[35] = (dresserItemSpawn[35] = 4169);
		dresserTypeToIcon[36] = (dresserItemSpawn[36] = 4190);
		dresserTypeToIcon[37] = (dresserItemSpawn[37] = 4211);
		dresserTypeToIcon[38] = (dresserItemSpawn[38] = 4301);
		dresserTypeToIcon[39] = (dresserItemSpawn[39] = 4569);
		dresserTypeToIcon[40] = (dresserItemSpawn[40] = 5151);
		dresserTypeToIcon[41] = (dresserItemSpawn[41] = 5172);
		dresserTypeToIcon[42] = (dresserItemSpawn[42] = 5193);
	}

	private static bool IsPlayerInChest(int i)
	{
		for (int j = 0; j < 255; j++)
		{
			if (Main.player[j].chest == i)
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Gets a list of currently-open chests in the world.
	/// <br /> Never called on <see cref="F:Terraria.ID.NetmodeID.MultiplayerClient" />.
	/// </summary>
	/// <returns>A <see cref="T:System.Collections.Generic.List`1" /> containing the indices in <see cref="F:Terraria.Main.chest" /> of all chests any active <see cref="T:Terraria.Player" /> has open.</returns>
	public static List<int> GetCurrentlyOpenChests()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].chest > -1)
			{
				list.Add(Main.player[i].chest);
			}
		}
		return list;
	}

	/// <summary>
	/// Determines if the chest at <see cref="F:Terraria.Main.tile" />[<paramref name="x" />, <paramref name="y" />] is locked.
	/// </summary>
	/// <param name="x">The x-coordinate of the chest to check, in tile coordinates.</param>
	/// <param name="y">The y-coordinate of the chest to check, in tile coordinates.</param>
	/// <returns><see langword="true" /> if the chest is locked, <see langword="false" /> otherwise.</returns>
	public static bool IsLocked(int x, int y)
	{
		return IsLocked(x, y, Main.tile[x, y]);
	}

	/// <inheritdoc cref="M:Terraria.Chest.IsLocked(System.Int32,System.Int32)" />
	/// <param name="x"><inheritdoc cref="M:Terraria.Chest.IsLocked(System.Int32,System.Int32)" /></param>
	/// <param name="y"><inheritdoc cref="M:Terraria.Chest.IsLocked(System.Int32,System.Int32)" /></param>
	/// <param name="t">The <see cref="T:Terraria.Tile" /> at <see cref="F:Terraria.Main.tile" />[<paramref name="x" />, <paramref name="y" />]</param>
	public static bool IsLocked(int x, int y, Tile t)
	{
		if (t == null)
		{
			return true;
		}
		if (t.type >= TileID.Count)
		{
			return TileLoader.IsLockedChest(x, y, t.type);
		}
		if (t.type == 21 && ((t.frameX >= 72 && t.frameX <= 106) || (t.frameX >= 144 && t.frameX <= 178) || (t.frameX >= 828 && t.frameX <= 1006) || (t.frameX >= 1296 && t.frameX <= 1330) || (t.frameX >= 1368 && t.frameX <= 1402) || (t.frameX >= 1440 && t.frameX <= 1474)))
		{
			return true;
		}
		if (t.type == 467)
		{
			return t.frameX / 36 == 13;
		}
		return false;
	}

	/// <summary>
	/// Attempts to move the item in <paramref name="plr" />'s inventory or Void Vault into nearby chests.
	/// <br /> Only called on <see cref="F:Terraria.ID.NetmodeID.Server" />.
	/// </summary>
	/// <param name="plr">The index of the <see cref="T:Terraria.Player" /> in <see cref="F:Terraria.Main.player" /> to try moving an item for.</param>
	/// <param name="slot">
	/// The <see cref="T:Terraria.ID.PlayerItemSlotID" /> to move an item from.
	/// <br /> If <c><paramref name="slot" /> &lt; 58</c>, then that slot of <paramref name="plr" />'s inventory is moved.
	/// <br /> If <c><paramref name="slot" /> &gt;= <see cref="F:Terraria.ID.PlayerItemSlotID.Bank4_0" /> &amp;&amp; <paramref name="slot" /> &lt; <see cref="F:Terraria.ID.PlayerItemSlotID.Bank4_0" /> + 40</c>, then that slot (minus <see cref="F:Terraria.ID.PlayerItemSlotID.Bank4_0" />) of <paramref name="plr" />'s Void Vault is moved.
	/// <br /> Otherwise, no item is moved.
	/// </param>
	/// <remarks>
	/// It's recommended to use <see cref="M:Terraria.Player.QuickStackAllChests" /> to handle this automatically.
	/// </remarks>
	public static void ServerPlaceItem(int plr, int slot)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (slot >= PlayerItemSlotID.Bank4_0 && slot < PlayerItemSlotID.Bank4_0 + 40)
		{
			int num = slot - PlayerItemSlotID.Bank4_0;
			Main.player[plr].bank4.item[num] = PutItemInNearbyChest(Main.player[plr].bank4.item[num], Main.player[plr].Center);
			NetMessage.SendData(5, -1, -1, null, plr, slot, Main.player[plr].bank4.item[num].prefix);
		}
		else if (slot < 58)
		{
			Main.player[plr].inventory[slot] = PutItemInNearbyChest(Main.player[plr].inventory[slot], Main.player[plr].Center);
			NetMessage.SendData(5, -1, -1, null, plr, slot, Main.player[plr].inventory[slot].prefix);
		}
	}

	/// <summary>
	/// Attempts to move the given item into any chests near the given position. Cannot place items into a chest if <see cref="M:Terraria.Chest.IsPlayerInChest(System.Int32)" /> or  if that chest <see cref="M:Terraria.Chest.IsLocked(System.Int32,System.Int32)" />
	/// </summary>
	/// <param name="item">The item to attempt moving.</param>
	/// <param name="position">The position to attempt moving from.</param>
	/// <returns>
	/// If on <see cref="F:Terraria.ID.NetmodeID.MultiplayerClient" />, <paramref name="item" /> without modification.
	/// <br /> Otherwise, <paramref name="item" /> after being quick-stacked into any nearby chests. May be <see cref="P:Terraria.Item.IsAir" />.
	/// </returns>
	public static Item PutItemInNearbyChest(Item item, Vector2 position)
	{
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 1)
		{
			return item;
		}
		bool flag = true;
		Vector2 vector = default(Vector2);
		for (int i = 0; i < 8000; i++)
		{
			bool flag2 = false;
			bool flag3 = false;
			if (Main.chest[i] == null || IsPlayerInChest(i) || IsLocked(Main.chest[i].x, Main.chest[i].y))
			{
				continue;
			}
			((Vector2)(ref vector))._002Ector((float)(Main.chest[i].x * 16 + 16), (float)(Main.chest[i].y * 16 + 16));
			Vector2 val = vector - position;
			if (((Vector2)(ref val)).Length() >= 600f)
			{
				continue;
			}
			for (int j = 0; j < Main.chest[i].item.Length; j++)
			{
				if (Main.chest[i].item[j].IsAir)
				{
					flag3 = true;
				}
				else if (item.IsTheSameAs(Main.chest[i].item[j]))
				{
					flag2 = true;
					if (ItemLoader.TryStackItems(Main.chest[i].item[j], item, out var numTransfered))
					{
						if (numTransfered > 0)
						{
							VisualizeChestTransfer(position, vector, item, numTransfered);
						}
						if (item.stack <= 0)
						{
							item.SetDefaults();
							return item;
						}
					}
				}
				else
				{
					flag3 = true;
				}
			}
			if (!(flag2 && flag3) || item.stack <= 0)
			{
				continue;
			}
			for (int k = 0; k < Main.chest[i].item.Length; k++)
			{
				if (Main.chest[i].item[k].type == 0 || Main.chest[i].item[k].stack == 0)
				{
					VisualizeChestTransfer(position, vector, item, item.stack);
					if (flag)
					{
						Main.chest[i].item[k] = item.Clone();
						item.SetDefaults();
					}
					return item;
				}
			}
		}
		return item;
	}

	/// <summary>
	/// Visually moves <paramref name="item" /> from <paramref name="position" /> to <paramref name="chestPosition" />.
	/// <br /> If called on <see cref="F:Terraria.ID.NetmodeID.MultiplayerClient" />, sync the transfer to all clients.
	/// </summary>
	/// <param name="position">The original position of <paramref name="item" /> in world coordinates.</param>
	/// <param name="chestPosition">The position of the destination chest in world coordinates.</param>
	/// <param name="item">The item to move. Only <see cref="F:Terraria.Item.type" /> is read.</param>
	/// <param name="amountMoved"><strong>Unused.</strong></param>
	public static void VisualizeChestTransfer(Vector2 position, Vector2 chestPosition, Item item, int amountMoved)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.ItemTransfer, new ParticleOrchestraSettings
		{
			PositionInWorld = position,
			MovementVector = chestPosition - position,
			UniqueInfoPiece = item.type
		});
	}

	/// <summary>
	/// Visually moves <paramref name="coinsMoved" /> worth of coins from <paramref name="position" /> to <paramref name="chestPosition" />.
	/// <br /> If called on <see cref="F:Terraria.ID.NetmodeID.MultiplayerClient" />, sync the transfer to all clients.
	/// </summary>
	/// <param name="position">The original position of the coins in world coordinates.</param>
	/// <param name="chestPosition">The position of the destination chest in world coordinates.</param>
	/// <param name="coinsMoved">The value of the coins moved in copper coins.</param>
	public static void VisualizeChestTransfer_CoinsBatch(Vector2 position, Vector2 chestPosition, long coinsMoved)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		int[] array = Utils.CoinsSplit(coinsMoved);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] >= 1)
			{
				ParticleOrchestrator.BroadcastOrRequestParticleSpawn(ParticleOrchestraType.ItemTransfer, new ParticleOrchestraSettings
				{
					PositionInWorld = position,
					MovementVector = chestPosition - position,
					UniqueInfoPiece = 71 + i
				});
			}
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	/// <summary>
	/// Unlocks a locked chest.
	/// </summary>
	/// <param name="X">The x-coordinate of the locked chest, in tile coordinates.</param>
	/// <param name="Y">The y-coordinate of the locked chest, in tile coordinates.</param>
	/// <returns><see langword="true" /> if the chest successfully unlocked, <see langword="false" /> otherwise.</returns>
	public static bool Unlock(int X, int Y)
	{
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		if (Main.tile[X, Y] == null || Main.tile[X + 1, Y] == null || Main.tile[X, Y + 1] == null || Main.tile[X + 1, Y + 1] == null)
		{
			return false;
		}
		short num = 0;
		int type = 0;
		Tile tileSafely = Framing.GetTileSafely(X, Y);
		int type2 = tileSafely.type;
		int num2 = tileSafely.frameX / 36;
		if (Main.tile[X, Y].type >= TileID.Count)
		{
			num = -36;
			type = 11;
			bool manual = false;
			if (!TileLoader.UnlockChest(X, Y, Main.tile[X, Y].type, ref num, ref type, ref manual))
			{
				return false;
			}
			if (manual)
			{
				return true;
			}
			num *= -1;
		}
		else
		{
			switch (type2)
			{
			case 21:
				switch (num2)
				{
				case 2:
					num = 36;
					type = 11;
					AchievementsHelper.NotifyProgressionEvent(19);
					break;
				case 4:
					num = 36;
					type = 11;
					break;
				case 23:
				case 24:
				case 25:
				case 26:
				case 27:
					if (!NPC.downedPlantBoss)
					{
						return false;
					}
					num = 180;
					type = 11;
					AchievementsHelper.NotifyProgressionEvent(20);
					break;
				case 36:
				case 38:
				case 40:
					num = 36;
					type = 11;
					break;
				default:
					return false;
				}
				break;
			case 467:
				if (num2 == 13)
				{
					if (!NPC.downedPlantBoss)
					{
						return false;
					}
					num = 36;
					type = 11;
					AchievementsHelper.NotifyProgressionEvent(20);
					break;
				}
				return false;
			}
		}
		SoundEngine.PlaySound(22, X * 16, Y * 16);
		for (int i = X; i <= X + 1; i++)
		{
			for (int j = Y; j <= Y + 1; j++)
			{
				Tile tileSafely2 = Framing.GetTileSafely(i, j);
				if (tileSafely2.type == type2)
				{
					tileSafely2.frameX -= num;
					for (int k = 0; k < 4; k++)
					{
						Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, type);
					}
				}
			}
		}
		return true;
	}

	/// <summary>
	/// Locks an unlocked chest.
	/// </summary>
	/// <param name="X">The x-coordinate of the unlocked chest, in tile coordinates.</param>
	/// <param name="Y">The y-coordinate of the unlocked chest, in tile coordinates.</param>
	/// <returns><see langword="true" /> if the chest successfully locked, <see langword="false" /> otherwise.</returns>
	public static bool Lock(int X, int Y)
	{
		if (Main.tile[X, Y] == null || Main.tile[X + 1, Y] == null || Main.tile[X, Y + 1] == null || Main.tile[X + 1, Y + 1] == null)
		{
			return false;
		}
		short num = 0;
		Tile tileSafely = Framing.GetTileSafely(X, Y);
		int type = tileSafely.type;
		int num2 = tileSafely.frameX / 36;
		if (Main.tile[X, Y].type >= TileID.Count)
		{
			num = 36;
			bool manual = false;
			if (!TileLoader.LockChest(X, Y, Main.tile[X, Y].type, ref num, ref manual))
			{
				return false;
			}
			if (manual)
			{
				return true;
			}
		}
		else
		{
			switch (type)
			{
			case 21:
				switch (num2)
				{
				case 1:
					num = 36;
					break;
				case 3:
					num = 36;
					break;
				case 18:
				case 19:
				case 20:
				case 21:
				case 22:
					if (!NPC.downedPlantBoss)
					{
						return false;
					}
					num = 180;
					break;
				case 35:
				case 37:
				case 39:
					num = 36;
					break;
				default:
					return false;
				}
				break;
			case 467:
				if (num2 == 12)
				{
					if (!NPC.downedPlantBoss)
					{
						return false;
					}
					num = 36;
					AchievementsHelper.NotifyProgressionEvent(20);
					break;
				}
				return false;
			}
		}
		SoundEngine.PlaySound(22, X * 16, Y * 16);
		for (int i = X; i <= X + 1; i++)
		{
			for (int j = Y; j <= Y + 1; j++)
			{
				Tile tileSafely2 = Framing.GetTileSafely(i, j);
				if (tileSafely2.type == type)
				{
					tileSafely2.frameX += num;
				}
			}
		}
		return true;
	}

	/// <summary>
	/// Gets the <see cref="T:Terraria.Player" /> currently using the given chest, if any.
	/// </summary>
	/// <param name="i">The index of the chest in <see cref="F:Terraria.Main.chest" /> to check.</param>
	/// <returns>The index in <see cref="F:Terraria.Main.player" /> of the <see cref="T:Terraria.Player" /> using this chest, or <c>-1</c> if no player is using this chest.</returns>
	public static int UsingChest(int i)
	{
		if (Main.chest[i] != null)
		{
			for (int j = 0; j < 255; j++)
			{
				if (Main.player[j].active && Main.player[j].chest == i)
				{
					return j;
				}
			}
		}
		return -1;
	}

	/// <summary>
	/// Tries to find a <see cref="T:Terraria.Chest" /> located at (<paramref name="X" />, <paramref name="Y" />) (tile coordinates).
	/// </summary>
	/// <param name="X">The x-coordinate to check, in tile coordinates.</param>
	/// <param name="Y">The y-coordinate to check, in tile coordinates.</param>
	/// <returns>The index in <see cref="F:Terraria.Main.chest" /> of the chest at (<paramref name="X" />, <paramref name="Y" />), or <c>-1</c> if one doesn't exist.</returns>
	/// <remarks>
	/// If you don't have the top-left corner of the chest tile, use <see cref="M:Terraria.Chest.FindChestByGuessing(System.Int32,System.Int32)" />.
	/// </remarks>
	public static int FindChest(int X, int Y)
	{
		for (int i = 0; i < 8000; i++)
		{
			if (Main.chest[i] != null && Main.chest[i].x == X && Main.chest[i].y == Y)
			{
				return i;
			}
		}
		return -1;
	}

	/// <summary>
	/// Tries to find the <see cref="T:Terraria.Chest" /> associated with the tile at (<paramref name="X" />, <paramref name="Y" />) (tile coordinates).
	/// </summary>
	/// <param name="X">The x-coordinate to check, in tile coordinates.</param>
	/// <param name="Y">The y-coordinate to check, in tile coordinates.</param>
	/// <returns>The index in <see cref="F:Terraria.Main.chest" /> of the chest associated with the tile at (<paramref name="X" />, <paramref name="Y" />), or <c>-1</c> if one doesn't exist.</returns>
	/// <remarks>
	/// If you have the top-left corner of the chest tile, use <see cref="M:Terraria.Chest.FindChest(System.Int32,System.Int32)" />.
	/// </remarks>
	public static int FindChestByGuessing(int X, int Y)
	{
		for (int i = 0; i < 8000; i++)
		{
			if (Main.chest[i] != null && Main.chest[i].x >= X && Main.chest[i].x < X + 2 && Main.chest[i].y >= Y && Main.chest[i].y < Y + 2)
			{
				return i;
			}
		}
		return -1;
	}

	/// <summary>
	/// Determines if a new chest can exist at tile coordinates (<paramref name="x" />, <paramref name="y" />).
	/// </summary>
	/// <param name="x">The x-coordinate to check, in tile coordinates.</param>
	/// <param name="y">The y-coordinate to check, in tile coordinates.</param>
	/// <param name="type">Unused, but required for use with <see cref="F:Terraria.DataStructures.PlacementHook.hook" />.</param>
	/// <param name="style">Unused, but required for use with <see cref="F:Terraria.DataStructures.PlacementHook.hook" />.</param>
	/// <param name="direction">Unused, but required for use with <see cref="F:Terraria.DataStructures.PlacementHook.hook" />.</param>
	/// <param name="alternate">Unused, but required for use with <see cref="F:Terraria.DataStructures.PlacementHook.hook" />.</param>
	/// <returns>
	/// <c>-1</c> if there exists a chest in <see cref="F:Terraria.Main.chest" /> at the coordinates (<paramref name="x" />, <paramref name="y" />), or if <see cref="F:Terraria.Main.chest" /> contains no <see langword="null" /> elements.
	/// <br /> Otherwise, the index in <see cref="F:Terraria.Main.chest" /> of the first <see langword="null" /> element.
	/// </returns>
	public static int FindEmptyChest(int x, int y, int type = 21, int style = 0, int direction = 1, int alternate = 0)
	{
		int num = -1;
		for (int i = 0; i < 8000; i++)
		{
			Chest chest = Main.chest[i];
			if (chest != null)
			{
				if (chest.x == x && chest.y == y)
				{
					return -1;
				}
			}
			else if (num == -1)
			{
				num = i;
			}
		}
		return num;
	}

	/// <summary>
	/// Determines if there are any chests near the tile coordinates (<paramref name="x" />, <paramref name="y" />).
	/// </summary>
	/// <param name="x">The x-coordinate to check, in tile coordinates.</param>
	/// <param name="y">The y-coordinate to check, in tile coordinates.</param>
	/// <returns><see langword="true" /> if any position in <see cref="F:Terraria.Main.tile" /> within [-25, 25) tiles horizontally and [-8, 8) tiles vertically has a tile (<see cref="P:Terraria.Tile.HasTile" />) and if that tile's <see cref="P:Terraria.Tile.TileType" /> is in <see cref="F:Terraria.ID.TileID.Sets.BasicChest" />. Otherwise, returns <see langword="false" />.</returns>
	public static bool NearOtherChests(int x, int y)
	{
		for (int i = x - 25; i < x + 25; i++)
		{
			for (int j = y - 8; j < y + 8; j++)
			{
				Tile tileSafely = Framing.GetTileSafely(i, j);
				if (tileSafely.active() && TileID.Sets.BasicChest[tileSafely.type])
				{
					return true;
				}
			}
		}
		return false;
	}

	/// <summary>
	/// Registers a placed chest into the world.
	/// </summary>
	/// <param name="x">The x-coordinate to check, in tile coordinates.</param>
	/// <param name="y">The y-coordinate to check, in tile coordinates.</param>
	/// <param name="type">The tile type of the placed chest.</param>
	/// <param name="style">The tile style of the placed chest.</param>
	/// <param name="direction">Unused, but required for use with <see cref="F:Terraria.DataStructures.PlacementHook.hook" />.</param>
	/// <param name="alternate">Unused, but required for use with <see cref="F:Terraria.DataStructures.PlacementHook.hook" />.</param>
	/// <returns>
	/// <c>-1</c> if there exists a chest in <see cref="F:Terraria.Main.chest" /> at the coordinates (<paramref name="x" />, <paramref name="y" />), or if <see cref="F:Terraria.Main.chest" /> contains no <see langword="null" /> elements.
	/// <br /> Otherwise, the index in <see cref="F:Terraria.Main.chest" /> of the newly-created chest.
	/// </returns>
	public static int AfterPlacement_Hook(int x, int y, int type = 21, int style = 0, int direction = 1, int alternate = 0)
	{
		Point16 baseCoords = new Point16(x, y);
		TileObjectData.OriginToTopLeft(type, style, ref baseCoords);
		int num = FindEmptyChest(baseCoords.X, baseCoords.Y);
		if (num == -1)
		{
			return -1;
		}
		if (Main.netMode != 1)
		{
			Chest chest = new Chest();
			chest.x = baseCoords.X;
			chest.y = baseCoords.Y;
			for (int i = 0; i < 40; i++)
			{
				chest.item[i] = new Item();
			}
			Main.chest[num] = chest;
		}
		else
		{
			switch (type)
			{
			case 21:
				NetMessage.SendData(34, -1, -1, null, 0, x, y, style);
				break;
			case 467:
				NetMessage.SendData(34, -1, -1, null, 4, x, y, style);
				break;
			case 88:
				NetMessage.SendData(34, -1, -1, null, 2, x, y, style);
				break;
			default:
				if (TileID.Sets.BasicChest[type])
				{
					NetMessage.SendData(34, -1, -1, null, 100, x, y, style, 0, type);
				}
				else if (TileID.Sets.BasicDresser[type])
				{
					NetMessage.SendData(34, -1, -1, null, 102, x, y, style, 0, type);
				}
				break;
			}
		}
		return num;
	}

	/// <summary>
	/// Creates and registers a new chest into the world.
	/// </summary>
	/// <param name="X">The x-coordinate to check, in tile coordinates.</param>
	/// <param name="Y">The y-coordinate to check, in tile coordinates.</param>
	/// <param name="id">The index in <see cref="F:Terraria.Main.chest" /> that this chest should use. If <c><paramref name="id" /> != -1</c>, placement cannot fail.</param>
	/// <returns>
	/// <c>-1</c> if <c><paramref name="id" /> == -1</c> and <see cref="M:Terraria.Chest.FindEmptyChest(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)" /> returns <c>-1</c>.
	/// <br /> Otherwise, the index in <see cref="F:Terraria.Main.chest" /> of the newly-created chest.
	/// </returns>
	/// <remarks>
	/// It is recommended to use <see cref="M:Terraria.WorldGen.PlaceChest(System.Int32,System.Int32,System.UInt16,System.Boolean,System.Int32)" /> to place chests into the world.
	/// </remarks>
	public static int CreateChest(int X, int Y, int id = -1)
	{
		int num = id;
		if (num == -1)
		{
			num = FindEmptyChest(X, Y);
			if (num == -1)
			{
				return -1;
			}
			if (Main.netMode == 1)
			{
				return num;
			}
		}
		Main.chest[num] = new Chest();
		Main.chest[num].x = X;
		Main.chest[num].y = Y;
		for (int i = 0; i < 40; i++)
		{
			Main.chest[num].item[i] = new Item();
		}
		return num;
	}

	/// <summary>
	/// Determines if the chest at (<paramref name="X" />, <paramref name="Y" />) can be destroyed.
	/// </summary>
	/// <param name="X">The x-coordinate to check, in tile coordinates.</param>
	/// <param name="Y">The y-coordinate to check, in tile coordinates.</param>
	/// <returns><see langword="false" /> if a chest exists at (<paramref name="X" />, <paramref name="Y" />) and if any element in that chest's <see cref="F:Terraria.Chest.item" /> array is not <see cref="P:Terraria.Item.IsAir" />. Otherwise, returns <see langword="true" />.</returns>
	public static bool CanDestroyChest(int X, int Y)
	{
		for (int i = 0; i < 8000; i++)
		{
			Chest chest = Main.chest[i];
			if (chest == null || chest.x != X || chest.y != Y)
			{
				continue;
			}
			for (int j = 0; j < 40; j++)
			{
				if (chest.item[j] != null && chest.item[j].type > 0 && chest.item[j].stack > 0)
				{
					return false;
				}
			}
			return true;
		}
		return true;
	}

	/// <summary>
	/// Attempts to destroy the chest at (<paramref name="X" />, <paramref name="Y" />).
	/// </summary>
	/// <param name="X">The x-coordinate to check, in tile coordinates.</param>
	/// <param name="Y">The y-coordinate to check, in tile coordinates.</param>
	/// <returns><see langword="false" /> if a chest exists at (<paramref name="X" />, <paramref name="Y" />) and if any element in that chest's <see cref="F:Terraria.Chest.item" /> array is not <see cref="P:Terraria.Item.IsAir" />. Otherwise, returns <see langword="true" />.</returns>
	public static bool DestroyChest(int X, int Y)
	{
		for (int i = 0; i < 8000; i++)
		{
			Chest chest = Main.chest[i];
			if (chest == null || chest.x != X || chest.y != Y)
			{
				continue;
			}
			for (int j = 0; j < 40; j++)
			{
				if (chest.item[j] != null && chest.item[j].type > 0 && chest.item[j].stack > 0)
				{
					return false;
				}
			}
			Main.chest[i] = null;
			if (Main.player[Main.myPlayer].chest == i)
			{
				Main.player[Main.myPlayer].chest = -1;
			}
			Recipe.FindRecipes();
			return true;
		}
		return true;
	}

	/// <summary>
	/// Forcibly destroys the chest at (<paramref name="X" />, <paramref name="Y" />) with ID <paramref name="id" /> if it exists.
	/// <br /> <strong>Warning: This will destroy any items contained within the chest.</strong>
	/// </summary>
	/// <param name="X">The x-coordinate to check, in tile coordinates.</param>
	/// <param name="Y">The y-coordinate to check, in tile coordinates.</param>
	/// <param name="id">The index in <see cref="F:Terraria.Main.chest" /> of the chest to destroy.</param>
	public static void DestroyChestDirect(int X, int Y, int id)
	{
		if (id < 0 || id >= Main.chest.Length)
		{
			return;
		}
		try
		{
			Chest chest = Main.chest[id];
			if (chest != null && chest.x == X && chest.y == Y)
			{
				Main.chest[id] = null;
				if (Main.player[Main.myPlayer].chest == id)
				{
					Main.player[Main.myPlayer].chest = -1;
				}
				Recipe.FindRecipes();
			}
		}
		catch
		{
		}
	}

	/// <summary>
	/// Adds an item to the first empty slot in this shop.
	/// </summary>
	/// <param name="newItem">The item to add.</param>
	/// <returns><c>0</c> if the item could not be added to the shop, or the index in <see cref="F:Terraria.Chest.item" /> if the item was successfully added.</returns>
	public int AddItemToShop(Item newItem)
	{
		int num = Main.shopSellbackHelper.Remove(newItem);
		if (num >= newItem.stack)
		{
			return 0;
		}
		for (int i = 0; i < 38; i++)
		{
			if (item[i] == null || item[i].type == 0)
			{
				item[i] = newItem.Clone();
				item[i].favorited = false;
				item[i].buyOnce = true;
				item[i].stack -= num;
				_ = item[i].value;
				return i;
			}
		}
		return 0;
	}

	public static void SetupTravelShop_AddToShop(int it, ref int added, ref int count)
	{
		if (it == 0)
		{
			return;
		}
		added++;
		Main.travelShop[count] = it;
		count++;
		if (it == 2260)
		{
			Main.travelShop[count] = 2261;
			count++;
			Main.travelShop[count] = 2262;
			count++;
		}
		if (it == 4555)
		{
			Main.travelShop[count] = 4556;
			count++;
			Main.travelShop[count] = 4557;
			count++;
		}
		if (it == 4321)
		{
			Main.travelShop[count] = 4322;
			count++;
		}
		if (it == 4323)
		{
			Main.travelShop[count] = 4324;
			count++;
			Main.travelShop[count] = 4365;
			count++;
		}
		if (it == 5390)
		{
			Main.travelShop[count] = 5386;
			count++;
			Main.travelShop[count] = 5387;
			count++;
		}
		if (it == 4666)
		{
			Main.travelShop[count] = 4664;
			count++;
			Main.travelShop[count] = 4665;
			count++;
		}
		if (it == 3637)
		{
			count--;
			switch (Main.rand.Next(6))
			{
			case 0:
				Main.travelShop[count++] = 3637;
				Main.travelShop[count++] = 3642;
				break;
			case 1:
				Main.travelShop[count++] = 3621;
				Main.travelShop[count++] = 3622;
				break;
			case 2:
				Main.travelShop[count++] = 3634;
				Main.travelShop[count++] = 3639;
				break;
			case 3:
				Main.travelShop[count++] = 3633;
				Main.travelShop[count++] = 3638;
				break;
			case 4:
				Main.travelShop[count++] = 3635;
				Main.travelShop[count++] = 3640;
				break;
			case 5:
				Main.travelShop[count++] = 3636;
				Main.travelShop[count++] = 3641;
				break;
			}
		}
	}

	public static bool SetupTravelShop_CanAddItemToShop(int it)
	{
		if (it == 0)
		{
			return false;
		}
		for (int i = 0; i < 40; i++)
		{
			if (Main.travelShop[i] == it)
			{
				return false;
			}
			if (it == 3637)
			{
				int num = Main.travelShop[i];
				if ((uint)(num - 3621) <= 1u || (uint)(num - 3633) <= 9u)
				{
					return false;
				}
			}
		}
		return true;
	}

	public static void SetupTravelShop_GetPainting(Player playerWithHighestLuck, int[] rarity, ref int it, int minimumRarity = 0)
	{
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
		{
			it = 5121;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
		{
			it = 5122;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
		{
			it = 5124;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && !Main.dontStarveWorld)
		{
			it = 5123;
		}
		if (minimumRarity > 2)
		{
			return;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMoonlord)
		{
			it = 3596;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMartians)
		{
			it = 2865;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMartians)
		{
			it = 2866;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMartians)
		{
			it = 2867;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
		{
			it = 3055;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
		{
			it = 3056;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
		{
			it = 3057;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
		{
			it = 3058;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedFrost)
		{
			it = 3059;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && Main.hardMode && NPC.downedMoonlord)
		{
			it = 5243;
		}
		if (minimumRarity <= 1)
		{
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5121;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5122;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5124;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0 && Main.dontStarveWorld)
			{
				it = 5123;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5225;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5229;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5232;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5389;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5233;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5241;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5244;
			}
			if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
			{
				it = 5242;
			}
		}
	}

	public static void SetupTravelShop_AdjustSlotRarities(int slotItemAttempts, ref int[] rarity)
	{
		if (rarity[5] > 1 && slotItemAttempts > 4700)
		{
			rarity[5] = 1;
		}
		if (rarity[4] > 1 && slotItemAttempts > 4600)
		{
			rarity[4] = 1;
		}
		if (rarity[3] > 1 && slotItemAttempts > 4500)
		{
			rarity[3] = 1;
		}
		if (rarity[2] > 1 && slotItemAttempts > 4400)
		{
			rarity[2] = 1;
		}
		if (rarity[1] > 1 && slotItemAttempts > 4300)
		{
			rarity[1] = 1;
		}
		if (rarity[0] > 1 && slotItemAttempts > 4200)
		{
			rarity[0] = 1;
		}
	}

	public static void SetupTravelShop_GetItem(Player playerWithHighestLuck, int[] rarity, ref int it, int minimumRarity = 0)
	{
		if (minimumRarity <= 4 && playerWithHighestLuck.RollLuck(rarity[4]) == 0)
		{
			it = 3309;
		}
		if (minimumRarity <= 3 && playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 3314;
		}
		if (playerWithHighestLuck.RollLuck(rarity[5]) == 0)
		{
			it = 1987;
		}
		if (minimumRarity > 4)
		{
			return;
		}
		if (playerWithHighestLuck.RollLuck(rarity[4]) == 0 && Main.hardMode)
		{
			it = 2270;
		}
		if (playerWithHighestLuck.RollLuck(rarity[4]) == 0 && Main.hardMode)
		{
			it = 4760;
		}
		if (playerWithHighestLuck.RollLuck(rarity[4]) == 0)
		{
			it = 2278;
		}
		if (playerWithHighestLuck.RollLuck(rarity[4]) == 0)
		{
			it = 2271;
		}
		if (minimumRarity > 3)
		{
			return;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
		{
			it = 2223;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2272;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2276;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2284;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2285;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2286;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 2287;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 4744;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && NPC.downedBoss3)
		{
			it = 2296;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 3628;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0 && Main.hardMode)
		{
			it = 4091;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 4603;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 4604;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 5297;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 4605;
		}
		if (playerWithHighestLuck.RollLuck(rarity[3]) == 0)
		{
			it = 4550;
		}
		if (minimumRarity > 2)
		{
			return;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 2268;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && WorldGen.shadowOrbSmashed)
		{
			it = 2269;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 1988;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 2275;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 2279;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 2277;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4555;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4321;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4323;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 5390;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4549;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4561;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4774;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 5136;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 5305;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4562;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4558;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4559;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4563;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0)
		{
			it = 4666;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedQueenBee || Main.hardMode))
		{
			it = 4347;
			if (Main.hardMode)
			{
				it = 4348;
			}
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedBoss1)
		{
			it = 3262;
		}
		if (playerWithHighestLuck.RollLuck(rarity[2]) == 0 && NPC.downedMechBossAny)
		{
			it = 3284;
		}
		if (minimumRarity > 1)
		{
			return;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2267;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2214;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2215;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2216;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2217;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 3624;
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			if (Main.remixWorld)
			{
				it = 671;
			}
			else
			{
				it = 2273;
			}
		}
		if (playerWithHighestLuck.RollLuck(rarity[1]) == 0)
		{
			it = 2274;
		}
		if (minimumRarity <= 0)
		{
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2266;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2281 + Main.rand.Next(3);
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2258;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2242;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 2260;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3637;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 4420;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3119;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3118;
			}
			if (playerWithHighestLuck.RollLuck(rarity[0]) == 0)
			{
				it = 3099;
			}
		}
	}

	/// <summary>
	/// Sets up the <see cref="F:Terraria.ID.NPCID.TravellingMerchant" />'s shop.
	/// </summary>
	public static void SetupTravelShop()
	{
		for (int i = 0; i < 40; i++)
		{
			Main.travelShop[i] = 0;
		}
		Player player = null;
		for (int j = 0; j < 255; j++)
		{
			Player player2 = Main.player[j];
			if (player2.active && (player == null || player.luck < player2.luck))
			{
				player = player2;
			}
		}
		if (player == null)
		{
			player = new Player();
		}
		int num = Main.rand.Next(4, 7);
		if (player.RollLuck(4) == 0)
		{
			num++;
		}
		if (player.RollLuck(8) == 0)
		{
			num++;
		}
		if (player.RollLuck(16) == 0)
		{
			num++;
		}
		if (player.RollLuck(32) == 0)
		{
			num++;
		}
		if (Main.expertMode && player.RollLuck(2) == 0)
		{
			num++;
		}
		if (NPC.peddlersSatchelWasUsed)
		{
			num++;
		}
		if (Main.tenthAnniversaryWorld)
		{
			if (!Main.getGoodWorld)
			{
				num++;
			}
			num++;
		}
		int count = 0;
		int added = 0;
		int[] array = new int[6] { 100, 200, 300, 400, 500, 600 };
		int[] rarity = array;
		int num2 = 0;
		if (Main.hardMode)
		{
			int it = 0;
			while (num2 < 5000)
			{
				num2++;
				SetupTravelShop_AdjustSlotRarities(num2, ref rarity);
				SetupTravelShop_GetItem(player, rarity, ref it, 2);
				if (SetupTravelShop_CanAddItemToShop(it))
				{
					SetupTravelShop_AddToShop(it, ref added, ref count);
					break;
				}
			}
		}
		while (added < num)
		{
			int it2 = 0;
			SetupTravelShop_GetItem(player, array, ref it2);
			if (SetupTravelShop_CanAddItemToShop(it2))
			{
				SetupTravelShop_AddToShop(it2, ref added, ref count);
			}
		}
		rarity = array;
		num2 = 0;
		int it3 = 0;
		while (num2 < 5000)
		{
			num2++;
			SetupTravelShop_AdjustSlotRarities(num2, ref rarity);
			SetupTravelShop_GetPainting(player, rarity, ref it3);
			if (SetupTravelShop_CanAddItemToShop(it3))
			{
				SetupTravelShop_AddToShop(it3, ref added, ref count);
				break;
			}
		}
		NPCLoader.SetupTravelShop(Main.travelShop, ref count);
	}

	public void SetupShop(int type)
	{
		SetupShop(NPCShopDatabase.GetShopNameFromVanillaIndex(type), Main.LocalPlayer.TalkNPC);
	}

	internal void VanillaSetupShop(int type)
	{
		//IL_3df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f38: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ed4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ef7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4034: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_4274: Unknown result type (might be due to invalid IL or missing references)
		bool flag = Main.LocalPlayer.currentShoppingSettings.PriceAdjustment <= 0.8999999761581421;
		Item[] array = item;
		for (int i = 0; i < 40; i++)
		{
			array[i] = new Item();
		}
		int num = 0;
		switch (type)
		{
		case 1:
		{
			array[num].SetDefaults(88);
			num++;
			array[num].SetDefaults(87);
			num++;
			array[num].SetDefaults(35);
			num++;
			array[num].SetDefaults(1991);
			num++;
			array[num].SetDefaults(3509);
			num++;
			array[num].SetDefaults(3506);
			num++;
			array[num].SetDefaults(8);
			num++;
			array[num].SetDefaults(28);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(188);
				num++;
			}
			array[num].SetDefaults(110);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(189);
				num++;
			}
			array[num].SetDefaults(40);
			num++;
			array[num].SetDefaults(42);
			num++;
			array[num].SetDefaults(965);
			num++;
			if (Main.player[Main.myPlayer].ZoneSnow)
			{
				array[num].SetDefaults(967);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneJungle)
			{
				array[num].SetDefaults(33);
				num++;
			}
			if (Main.dayTime && Main.IsItAHappyWindyDay)
			{
				array[num++].SetDefaults(4074);
			}
			if (Main.bloodMoon)
			{
				array[num].SetDefaults(279);
				num++;
			}
			if (!Main.dayTime)
			{
				array[num].SetDefaults(282);
				num++;
			}
			if (NPC.downedBoss3)
			{
				array[num].SetDefaults(346);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(488);
				num++;
			}
			for (int n = 0; n < 58; n++)
			{
				if (Main.player[Main.myPlayer].inventory[n].type == 930)
				{
					array[num].SetDefaults(931);
					num++;
					array[num].SetDefaults(1614);
					num++;
					break;
				}
			}
			array[num].SetDefaults(1786);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(1348);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(3198);
				num++;
			}
			if (NPC.downedBoss2 || NPC.downedBoss3 || Main.hardMode)
			{
				array[num++].SetDefaults(4063);
				array[num++].SetDefaults(4673);
			}
			if (Main.player[Main.myPlayer].HasItem(3107))
			{
				array[num].SetDefaults(3108);
				num++;
			}
			break;
		}
		case 2:
			array[num].SetDefaults(97);
			num++;
			if (Main.bloodMoon || Main.hardMode)
			{
				if (WorldGen.SavedOreTiers.Silver == 168)
				{
					array[num].SetDefaults(4915);
					num++;
				}
				else
				{
					array[num].SetDefaults(278);
					num++;
				}
			}
			if ((NPC.downedBoss2 && !Main.dayTime) || Main.hardMode)
			{
				array[num].SetDefaults(47);
				num++;
			}
			array[num].SetDefaults(95);
			num++;
			array[num].SetDefaults(98);
			num++;
			if (Main.player[Main.myPlayer].ZoneGraveyard && NPC.downedBoss3)
			{
				array[num++].SetDefaults(4703);
			}
			if (!Main.dayTime)
			{
				array[num].SetDefaults(324);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(534);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(1432);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(2177);
				num++;
			}
			if (Main.player[Main.myPlayer].HasItem(1258))
			{
				array[num].SetDefaults(1261);
				num++;
			}
			if (Main.player[Main.myPlayer].HasItem(1835))
			{
				array[num].SetDefaults(1836);
				num++;
			}
			if (Main.player[Main.myPlayer].HasItem(3107))
			{
				array[num].SetDefaults(3108);
				num++;
			}
			if (Main.player[Main.myPlayer].HasItem(1782))
			{
				array[num].SetDefaults(1783);
				num++;
			}
			if (Main.player[Main.myPlayer].HasItem(1784))
			{
				array[num].SetDefaults(1785);
				num++;
			}
			if (Main.halloween)
			{
				array[num].SetDefaults(1736);
				num++;
				array[num].SetDefaults(1737);
				num++;
				array[num].SetDefaults(1738);
				num++;
			}
			break;
		case 3:
			if (Main.bloodMoon)
			{
				if (WorldGen.crimson)
				{
					if (!Main.remixWorld)
					{
						array[num].SetDefaults(2886);
						num++;
					}
					array[num].SetDefaults(2171);
					num++;
					array[num].SetDefaults(4508);
					num++;
				}
				else
				{
					if (!Main.remixWorld)
					{
						array[num].SetDefaults(67);
						num++;
					}
					array[num].SetDefaults(59);
					num++;
					array[num].SetDefaults(4504);
					num++;
				}
			}
			else
			{
				if (!Main.remixWorld)
				{
					array[num].SetDefaults(66);
					num++;
				}
				array[num].SetDefaults(62);
				num++;
				array[num].SetDefaults(63);
				num++;
				array[num].SetDefaults(745);
				num++;
			}
			if (Main.hardMode && Main.player[Main.myPlayer].ZoneGraveyard)
			{
				if (WorldGen.crimson)
				{
					array[num].SetDefaults(59);
				}
				else
				{
					array[num].SetDefaults(2171);
				}
				num++;
			}
			array[num].SetDefaults(27);
			num++;
			array[num].SetDefaults(5309);
			num++;
			array[num].SetDefaults(114);
			num++;
			array[num].SetDefaults(1828);
			num++;
			array[num].SetDefaults(747);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(746);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(369);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(4505);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneUnderworldHeight)
			{
				array[num++].SetDefaults(5214);
			}
			else if (Main.player[Main.myPlayer].ZoneGlowshroom)
			{
				array[num++].SetDefaults(194);
			}
			if (Main.halloween)
			{
				array[num].SetDefaults(1853);
				num++;
				array[num].SetDefaults(1854);
				num++;
			}
			if (NPC.downedSlimeKing)
			{
				array[num].SetDefaults(3215);
				num++;
			}
			if (NPC.downedQueenBee)
			{
				array[num].SetDefaults(3216);
				num++;
			}
			if (NPC.downedBoss1)
			{
				array[num].SetDefaults(3219);
				num++;
			}
			if (NPC.downedBoss2)
			{
				if (WorldGen.crimson)
				{
					array[num].SetDefaults(3218);
					num++;
				}
				else
				{
					array[num].SetDefaults(3217);
					num++;
				}
			}
			if (NPC.downedBoss3)
			{
				array[num].SetDefaults(3220);
				num++;
				array[num].SetDefaults(3221);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(3222);
				num++;
			}
			array[num++].SetDefaults(4047);
			array[num++].SetDefaults(4045);
			array[num++].SetDefaults(4044);
			array[num++].SetDefaults(4043);
			array[num++].SetDefaults(4042);
			array[num++].SetDefaults(4046);
			array[num++].SetDefaults(4041);
			array[num++].SetDefaults(4241);
			array[num++].SetDefaults(4048);
			if (Main.hardMode)
			{
				switch (Main.moonPhase / 2)
				{
				case 0:
					array[num++].SetDefaults(4430);
					array[num++].SetDefaults(4431);
					array[num++].SetDefaults(4432);
					break;
				case 1:
					array[num++].SetDefaults(4433);
					array[num++].SetDefaults(4434);
					array[num++].SetDefaults(4435);
					break;
				case 2:
					array[num++].SetDefaults(4436);
					array[num++].SetDefaults(4437);
					array[num++].SetDefaults(4438);
					break;
				default:
					array[num++].SetDefaults(4439);
					array[num++].SetDefaults(4440);
					array[num++].SetDefaults(4441);
					break;
				}
			}
			break;
		case 4:
		{
			array[num].SetDefaults(168);
			num++;
			array[num].SetDefaults(166);
			num++;
			array[num].SetDefaults(167);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(265);
				num++;
			}
			if (Main.hardMode && NPC.downedPlantBoss && NPC.downedPirates)
			{
				array[num].SetDefaults(937);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(1347);
				num++;
			}
			for (int j = 0; j < 58; j++)
			{
				if (Main.player[Main.myPlayer].inventory[j].type == 4827)
				{
					array[num].SetDefaults(4827);
					num++;
					break;
				}
			}
			for (int k = 0; k < 58; k++)
			{
				if (Main.player[Main.myPlayer].inventory[k].type == 4824)
				{
					array[num].SetDefaults(4824);
					num++;
					break;
				}
			}
			for (int l = 0; l < 58; l++)
			{
				if (Main.player[Main.myPlayer].inventory[l].type == 4825)
				{
					array[num].SetDefaults(4825);
					num++;
					break;
				}
			}
			for (int m = 0; m < 58; m++)
			{
				if (Main.player[Main.myPlayer].inventory[m].type == 4826)
				{
					array[num].SetDefaults(4826);
					num++;
					break;
				}
			}
			break;
		}
		case 5:
		{
			array[num].SetDefaults(254);
			num++;
			array[num].SetDefaults(981);
			num++;
			if (Main.dayTime)
			{
				array[num].SetDefaults(242);
				num++;
			}
			if (Main.moonPhase == 0)
			{
				array[num].SetDefaults(245);
				num++;
				array[num].SetDefaults(246);
				num++;
				if (!Main.dayTime)
				{
					array[num++].SetDefaults(1288);
					array[num++].SetDefaults(1289);
				}
			}
			else if (Main.moonPhase == 1)
			{
				array[num].SetDefaults(325);
				num++;
				array[num].SetDefaults(326);
				num++;
			}
			array[num].SetDefaults(269);
			num++;
			array[num].SetDefaults(270);
			num++;
			array[num].SetDefaults(271);
			num++;
			if (NPC.downedClown)
			{
				array[num].SetDefaults(503);
				num++;
				array[num].SetDefaults(504);
				num++;
				array[num].SetDefaults(505);
				num++;
			}
			if (Main.bloodMoon)
			{
				array[num].SetDefaults(322);
				num++;
				if (!Main.dayTime)
				{
					array[num++].SetDefaults(3362);
					array[num++].SetDefaults(3363);
				}
			}
			if (NPC.downedAncientCultist)
			{
				if (Main.dayTime)
				{
					array[num++].SetDefaults(2856);
					array[num++].SetDefaults(2858);
				}
				else
				{
					array[num++].SetDefaults(2857);
					array[num++].SetDefaults(2859);
				}
			}
			if (NPC.AnyNPCs(441))
			{
				array[num++].SetDefaults(3242);
				array[num++].SetDefaults(3243);
				array[num++].SetDefaults(3244);
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num++].SetDefaults(4685);
				array[num++].SetDefaults(4686);
				array[num++].SetDefaults(4704);
				array[num++].SetDefaults(4705);
				array[num++].SetDefaults(4706);
				array[num++].SetDefaults(4707);
				array[num++].SetDefaults(4708);
				array[num++].SetDefaults(4709);
			}
			if (Main.player[Main.myPlayer].ZoneSnow)
			{
				array[num].SetDefaults(1429);
				num++;
			}
			if (Main.halloween)
			{
				array[num].SetDefaults(1740);
				num++;
			}
			if (Main.hardMode)
			{
				if (Main.moonPhase == 2)
				{
					array[num].SetDefaults(869);
					num++;
				}
				if (Main.moonPhase == 3)
				{
					array[num].SetDefaults(4994);
					num++;
					array[num].SetDefaults(4997);
					num++;
				}
				if (Main.moonPhase == 4)
				{
					array[num].SetDefaults(864);
					num++;
					array[num].SetDefaults(865);
					num++;
				}
				if (Main.moonPhase == 5)
				{
					array[num].SetDefaults(4995);
					num++;
					array[num].SetDefaults(4998);
					num++;
				}
				if (Main.moonPhase == 6)
				{
					array[num].SetDefaults(873);
					num++;
					array[num].SetDefaults(874);
					num++;
					array[num].SetDefaults(875);
					num++;
				}
				if (Main.moonPhase == 7)
				{
					array[num].SetDefaults(4996);
					num++;
					array[num].SetDefaults(4999);
					num++;
				}
			}
			if (NPC.downedFrost)
			{
				if (Main.dayTime)
				{
					array[num].SetDefaults(1275);
					num++;
				}
				else
				{
					array[num].SetDefaults(1276);
					num++;
				}
			}
			if (Main.halloween)
			{
				array[num++].SetDefaults(3246);
				array[num++].SetDefaults(3247);
			}
			if (BirthdayParty.PartyIsUp)
			{
				array[num++].SetDefaults(3730);
				array[num++].SetDefaults(3731);
				array[num++].SetDefaults(3733);
				array[num++].SetDefaults(3734);
				array[num++].SetDefaults(3735);
			}
			int golferScoreAccumulated = Main.LocalPlayer.golferScoreAccumulated;
			if (num < 38 && golferScoreAccumulated >= 2000)
			{
				array[num].SetDefaults(4744);
				num++;
			}
			array[num].SetDefaults(5308);
			num++;
			break;
		}
		case 6:
			array[num].SetDefaults(128);
			num++;
			array[num].SetDefaults(486);
			num++;
			array[num].SetDefaults(398);
			num++;
			array[num].SetDefaults(84);
			num++;
			array[num].SetDefaults(407);
			num++;
			array[num].SetDefaults(161);
			num++;
			if (Main.hardMode)
			{
				array[num++].SetDefaults(5324);
			}
			break;
		case 7:
			array[num].SetDefaults(487);
			num++;
			array[num].SetDefaults(496);
			num++;
			array[num].SetDefaults(500);
			num++;
			array[num].SetDefaults(507);
			num++;
			array[num].SetDefaults(508);
			num++;
			array[num].SetDefaults(531);
			num++;
			array[num].SetDefaults(149);
			num++;
			array[num].SetDefaults(576);
			num++;
			array[num].SetDefaults(3186);
			num++;
			if (Main.halloween)
			{
				array[num].SetDefaults(1739);
				num++;
			}
			break;
		case 8:
			array[num].SetDefaults(509);
			num++;
			array[num].SetDefaults(850);
			num++;
			array[num].SetDefaults(851);
			num++;
			array[num].SetDefaults(3612);
			num++;
			array[num].SetDefaults(510);
			num++;
			array[num].SetDefaults(530);
			num++;
			array[num].SetDefaults(513);
			num++;
			array[num].SetDefaults(538);
			num++;
			array[num].SetDefaults(529);
			num++;
			array[num].SetDefaults(541);
			num++;
			array[num].SetDefaults(542);
			num++;
			array[num].SetDefaults(543);
			num++;
			array[num].SetDefaults(852);
			num++;
			array[num].SetDefaults(853);
			num++;
			array[num++].SetDefaults(4261);
			array[num++].SetDefaults(3707);
			array[num].SetDefaults(2739);
			num++;
			array[num].SetDefaults(849);
			num++;
			array[num++].SetDefaults(1263);
			array[num++].SetDefaults(3616);
			array[num++].SetDefaults(3725);
			array[num++].SetDefaults(2799);
			array[num++].SetDefaults(3619);
			array[num++].SetDefaults(3627);
			array[num++].SetDefaults(3629);
			array[num++].SetDefaults(585);
			array[num++].SetDefaults(584);
			array[num++].SetDefaults(583);
			array[num++].SetDefaults(4484);
			array[num++].SetDefaults(4485);
			if (NPC.AnyNPCs(369) && (Main.moonPhase == 1 || Main.moonPhase == 3 || Main.moonPhase == 5 || Main.moonPhase == 7))
			{
				array[num].SetDefaults(2295);
				num++;
			}
			break;
		case 9:
		{
			array[num].SetDefaults(588);
			num++;
			array[num].SetDefaults(589);
			num++;
			array[num].SetDefaults(590);
			num++;
			array[num].SetDefaults(597);
			num++;
			array[num].SetDefaults(598);
			num++;
			array[num].SetDefaults(596);
			num++;
			for (int num2 = 1873; num2 < 1906; num2++)
			{
				array[num].SetDefaults(num2);
				num++;
			}
			break;
		}
		case 10:
			if (NPC.downedMechBossAny)
			{
				array[num].SetDefaults(756);
				num++;
				array[num].SetDefaults(787);
				num++;
			}
			array[num].SetDefaults(868);
			num++;
			if (NPC.downedPlantBoss)
			{
				array[num].SetDefaults(1551);
				num++;
			}
			array[num].SetDefaults(1181);
			num++;
			array[num++].SetDefaults(5231);
			if (!Main.remixWorld)
			{
				array[num++].SetDefaults(783);
			}
			break;
		case 11:
			if (!Main.remixWorld)
			{
				array[num++].SetDefaults(779);
			}
			if (Main.moonPhase >= 4 && Main.hardMode)
			{
				array[num++].SetDefaults(748);
			}
			else
			{
				array[num++].SetDefaults(839);
				array[num++].SetDefaults(840);
				array[num++].SetDefaults(841);
			}
			if (NPC.downedGolemBoss)
			{
				array[num++].SetDefaults(948);
			}
			if (Main.hardMode)
			{
				array[num++].SetDefaults(3623);
			}
			array[num++].SetDefaults(3603);
			array[num++].SetDefaults(3604);
			array[num++].SetDefaults(3607);
			array[num++].SetDefaults(3605);
			array[num++].SetDefaults(3606);
			array[num++].SetDefaults(3608);
			array[num++].SetDefaults(3618);
			array[num++].SetDefaults(3602);
			array[num++].SetDefaults(3663);
			array[num++].SetDefaults(3609);
			array[num++].SetDefaults(3610);
			if (Main.hardMode || !Main.getGoodWorld)
			{
				array[num++].SetDefaults(995);
			}
			if (NPC.downedBoss1 && NPC.downedBoss2 && NPC.downedBoss3)
			{
				array[num++].SetDefaults(2203);
			}
			if (WorldGen.crimson)
			{
				array[num++].SetDefaults(2193);
			}
			else
			{
				array[num++].SetDefaults(4142);
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num++].SetDefaults(2192);
			}
			if (Main.player[Main.myPlayer].ZoneJungle)
			{
				array[num++].SetDefaults(2204);
			}
			if (Main.player[Main.myPlayer].ZoneSnow)
			{
				array[num++].SetDefaults(2198);
			}
			if ((double)(Main.player[Main.myPlayer].position.Y / 16f) < Main.worldSurface * 0.3499999940395355)
			{
				array[num++].SetDefaults(2197);
			}
			if (Main.player[Main.myPlayer].HasItem(832))
			{
				array[num++].SetDefaults(2196);
			}
			if (!Main.remixWorld)
			{
				if (Main.eclipse || Main.bloodMoon)
				{
					if (WorldGen.crimson)
					{
						array[num++].SetDefaults(784);
					}
					else
					{
						array[num++].SetDefaults(782);
					}
				}
				else if (Main.player[Main.myPlayer].ZoneHallow)
				{
					array[num++].SetDefaults(781);
				}
				else
				{
					array[num++].SetDefaults(780);
				}
				if (NPC.downedMoonlord)
				{
					array[num++].SetDefaults(5392);
					array[num++].SetDefaults(5393);
					array[num++].SetDefaults(5394);
				}
			}
			if (Main.hardMode)
			{
				array[num++].SetDefaults(1344);
				array[num++].SetDefaults(4472);
			}
			if (Main.halloween)
			{
				array[num++].SetDefaults(1742);
			}
			break;
		case 12:
			array[num].SetDefaults(1037);
			num++;
			array[num].SetDefaults(2874);
			num++;
			array[num].SetDefaults(1120);
			num++;
			if (Main.netMode == 1)
			{
				array[num].SetDefaults(1969);
				num++;
			}
			if (Main.halloween)
			{
				array[num].SetDefaults(3248);
				num++;
				array[num].SetDefaults(1741);
				num++;
			}
			if (Main.moonPhase == 0)
			{
				array[num].SetDefaults(2871);
				num++;
				array[num].SetDefaults(2872);
				num++;
			}
			if (!Main.dayTime && Main.bloodMoon)
			{
				array[num].SetDefaults(4663);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(4662);
				num++;
			}
			break;
		case 13:
			array[num].SetDefaults(859);
			num++;
			if (Main.LocalPlayer.golferScoreAccumulated > 500)
			{
				array[num++].SetDefaults(4743);
			}
			array[num].SetDefaults(1000);
			num++;
			array[num].SetDefaults(1168);
			num++;
			if (Main.dayTime)
			{
				array[num].SetDefaults(1449);
				num++;
			}
			else
			{
				array[num].SetDefaults(4552);
				num++;
			}
			array[num].SetDefaults(1345);
			num++;
			array[num].SetDefaults(1450);
			num++;
			array[num++].SetDefaults(3253);
			array[num++].SetDefaults(4553);
			array[num++].SetDefaults(2700);
			array[num++].SetDefaults(2738);
			array[num++].SetDefaults(4470);
			array[num++].SetDefaults(4681);
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num++].SetDefaults(4682);
			}
			if (LanternNight.LanternsUp)
			{
				array[num++].SetDefaults(4702);
			}
			if (Main.player[Main.myPlayer].HasItem(3548))
			{
				array[num].SetDefaults(3548);
				num++;
			}
			if (NPC.AnyNPCs(229))
			{
				array[num++].SetDefaults(3369);
			}
			if (NPC.downedGolemBoss)
			{
				array[num++].SetDefaults(3546);
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(3214);
				num++;
				array[num].SetDefaults(2868);
				num++;
				array[num].SetDefaults(970);
				num++;
				array[num].SetDefaults(971);
				num++;
				array[num].SetDefaults(972);
				num++;
				array[num].SetDefaults(973);
				num++;
			}
			array[num++].SetDefaults(4791);
			array[num++].SetDefaults(3747);
			array[num++].SetDefaults(3732);
			array[num++].SetDefaults(3742);
			if (BirthdayParty.PartyIsUp)
			{
				array[num++].SetDefaults(3749);
				array[num++].SetDefaults(3746);
				array[num++].SetDefaults(3739);
				array[num++].SetDefaults(3740);
				array[num++].SetDefaults(3741);
				array[num++].SetDefaults(3737);
				array[num++].SetDefaults(3738);
				array[num++].SetDefaults(3736);
				array[num++].SetDefaults(3745);
				array[num++].SetDefaults(3744);
				array[num++].SetDefaults(3743);
			}
			break;
		case 14:
			array[num].SetDefaults(771);
			num++;
			if (Main.bloodMoon)
			{
				array[num].SetDefaults(772);
				num++;
			}
			if (!Main.dayTime || Main.eclipse)
			{
				array[num].SetDefaults(773);
				num++;
			}
			if (Main.eclipse)
			{
				array[num].SetDefaults(774);
				num++;
			}
			if (NPC.downedMartians)
			{
				array[num++].SetDefaults(4445);
				if (Main.bloodMoon || Main.eclipse)
				{
					array[num++].SetDefaults(4446);
				}
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(4459);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(760);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(1346);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(5451);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(5452);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(4409);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(4392);
				num++;
			}
			if (Main.halloween)
			{
				array[num].SetDefaults(1743);
				num++;
				array[num].SetDefaults(1744);
				num++;
				array[num].SetDefaults(1745);
				num++;
			}
			if (NPC.downedMartians)
			{
				array[num++].SetDefaults(2862);
				array[num++].SetDefaults(3109);
			}
			if (Main.player[Main.myPlayer].HasItem(3384) || Main.player[Main.myPlayer].HasItem(3664))
			{
				array[num].SetDefaults(3664);
				num++;
			}
			break;
		case 15:
		{
			array[num].SetDefaults(1071);
			num++;
			array[num].SetDefaults(1072);
			num++;
			array[num].SetDefaults(1100);
			num++;
			for (int num7 = 1073; num7 <= 1084; num7++)
			{
				array[num].SetDefaults(num7);
				num++;
			}
			array[num].SetDefaults(1097);
			num++;
			array[num].SetDefaults(1099);
			num++;
			array[num].SetDefaults(1098);
			num++;
			array[num].SetDefaults(1966);
			num++;
			if (Main.hardMode)
			{
				array[num].SetDefaults(1967);
				num++;
				array[num].SetDefaults(1968);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(4668);
				num++;
				if (NPC.downedPlantBoss)
				{
					array[num].SetDefaults(5344);
					num++;
				}
			}
			break;
		}
		case 25:
		{
			if (Main.xMas)
			{
				int num8 = 1948;
				while (num8 <= 1957 && num < 39)
				{
					array[num].SetDefaults(num8);
					num8++;
					num++;
				}
			}
			int num9 = 2158;
			while (num9 <= 2160 && num < 39)
			{
				array[num].SetDefaults(num9);
				num9++;
				num++;
			}
			int num10 = 2008;
			while (num10 <= 2014 && num < 39)
			{
				array[num].SetDefaults(num10);
				num10++;
				num++;
			}
			if (!Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(1490);
				num++;
				if (Main.moonPhase <= 1)
				{
					array[num].SetDefaults(1481);
					num++;
				}
				else if (Main.moonPhase <= 3)
				{
					array[num].SetDefaults(1482);
					num++;
				}
				else if (Main.moonPhase <= 5)
				{
					array[num].SetDefaults(1483);
					num++;
				}
				else
				{
					array[num].SetDefaults(1484);
					num++;
				}
			}
			if (Main.player[Main.myPlayer].ShoppingZone_Forest)
			{
				array[num].SetDefaults(5245);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneCrimson)
			{
				array[num].SetDefaults(1492);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneCorrupt)
			{
				array[num].SetDefaults(1488);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneHallow)
			{
				array[num].SetDefaults(1489);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneJungle)
			{
				array[num].SetDefaults(1486);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneSnow)
			{
				array[num].SetDefaults(1487);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneDesert)
			{
				array[num].SetDefaults(1491);
				num++;
			}
			if (Main.bloodMoon)
			{
				array[num].SetDefaults(1493);
				num++;
			}
			if (!Main.player[Main.myPlayer].ZoneGraveyard)
			{
				if ((double)(Main.player[Main.myPlayer].position.Y / 16f) < Main.worldSurface * 0.3499999940395355)
				{
					array[num].SetDefaults(1485);
					num++;
				}
				if ((double)(Main.player[Main.myPlayer].position.Y / 16f) < Main.worldSurface * 0.3499999940395355 && Main.hardMode)
				{
					array[num].SetDefaults(1494);
					num++;
				}
			}
			if (Main.IsItStorming)
			{
				array[num].SetDefaults(5251);
				num++;
			}
			if (Main.player[Main.myPlayer].ZoneGraveyard)
			{
				array[num].SetDefaults(4723);
				num++;
				array[num].SetDefaults(4724);
				num++;
				array[num].SetDefaults(4725);
				num++;
				array[num].SetDefaults(4726);
				num++;
				array[num].SetDefaults(4727);
				num++;
				array[num].SetDefaults(5257);
				num++;
				array[num].SetDefaults(4728);
				num++;
				array[num].SetDefaults(4729);
				num++;
			}
			break;
		}
		case 16:
			array[num++].SetDefaults(1430);
			array[num++].SetDefaults(986);
			if (NPC.AnyNPCs(108))
			{
				array[num++].SetDefaults(2999);
			}
			if (!Main.dayTime)
			{
				array[num++].SetDefaults(1158);
			}
			if (Main.hardMode && NPC.downedPlantBoss)
			{
				array[num++].SetDefaults(1159);
				array[num++].SetDefaults(1160);
				array[num++].SetDefaults(1161);
				if (Main.player[Main.myPlayer].ZoneJungle)
				{
					array[num++].SetDefaults(1167);
				}
				array[num++].SetDefaults(1339);
			}
			if (Main.hardMode && Main.player[Main.myPlayer].ZoneJungle)
			{
				array[num++].SetDefaults(1171);
				if (!Main.dayTime && NPC.downedPlantBoss)
				{
					array[num++].SetDefaults(1162);
				}
			}
			array[num++].SetDefaults(909);
			array[num++].SetDefaults(910);
			array[num++].SetDefaults(940);
			array[num++].SetDefaults(941);
			array[num++].SetDefaults(942);
			array[num++].SetDefaults(943);
			array[num++].SetDefaults(944);
			array[num++].SetDefaults(945);
			array[num++].SetDefaults(4922);
			array[num++].SetDefaults(4417);
			if (Main.player[Main.myPlayer].HasItem(1835))
			{
				array[num++].SetDefaults(1836);
			}
			if (Main.player[Main.myPlayer].HasItem(1258))
			{
				array[num++].SetDefaults(1261);
			}
			if (Main.halloween)
			{
				array[num++].SetDefaults(1791);
			}
			break;
		case 17:
		{
			array[num].SetDefaults(928);
			num++;
			array[num].SetDefaults(929);
			num++;
			array[num].SetDefaults(876);
			num++;
			array[num].SetDefaults(877);
			num++;
			array[num].SetDefaults(878);
			num++;
			array[num].SetDefaults(2434);
			num++;
			int num11 = (int)((Main.screenPosition.X + (float)(Main.screenWidth / 2)) / 16f);
			if ((double)(Main.screenPosition.Y / 16f) < Main.worldSurface + 10.0 && (num11 < 380 || num11 > Main.maxTilesX - 380))
			{
				array[num].SetDefaults(1180);
				num++;
			}
			if (Main.hardMode && NPC.downedMechBossAny && NPC.AnyNPCs(208))
			{
				array[num].SetDefaults(1337);
				num++;
			}
			break;
		}
		case 18:
		{
			array[num].SetDefaults(1990);
			num++;
			array[num].SetDefaults(1979);
			num++;
			if (Main.player[Main.myPlayer].ConsumedLifeCrystals == 15)
			{
				array[num].SetDefaults(1977);
				num++;
			}
			if (Main.player[Main.myPlayer].ConsumedManaCrystals == 9)
			{
				array[num].SetDefaults(1978);
				num++;
			}
			long num12 = 0L;
			for (int num13 = 0; num13 < 54; num13++)
			{
				if (Main.player[Main.myPlayer].inventory[num13].type == 71)
				{
					num12 += Main.player[Main.myPlayer].inventory[num13].stack;
				}
				if (Main.player[Main.myPlayer].inventory[num13].type == 72)
				{
					num12 += Main.player[Main.myPlayer].inventory[num13].stack * 100;
				}
				if (Main.player[Main.myPlayer].inventory[num13].type == 73)
				{
					num12 += Main.player[Main.myPlayer].inventory[num13].stack * 10000;
				}
				if (Main.player[Main.myPlayer].inventory[num13].type == 74)
				{
					num12 += Main.player[Main.myPlayer].inventory[num13].stack * 1000000;
				}
			}
			if (num12 >= 1000000)
			{
				array[num].SetDefaults(1980);
				num++;
			}
			if ((Main.moonPhase % 2 == 0 && Main.dayTime) || (Main.moonPhase % 2 == 1 && !Main.dayTime))
			{
				array[num].SetDefaults(1981);
				num++;
			}
			if (Main.player[Main.myPlayer].team != 0)
			{
				array[num].SetDefaults(1982);
				num++;
			}
			if (Main.hardMode)
			{
				array[num].SetDefaults(1983);
				num++;
			}
			if (NPC.AnyNPCs(208))
			{
				array[num].SetDefaults(1984);
				num++;
			}
			if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
			{
				array[num].SetDefaults(1985);
				num++;
			}
			if (Main.hardMode && NPC.downedMechBossAny)
			{
				array[num].SetDefaults(1986);
				num++;
			}
			if (Main.hardMode && NPC.downedMartians)
			{
				array[num].SetDefaults(2863);
				num++;
				array[num].SetDefaults(3259);
				num++;
			}
			array[num++].SetDefaults(5104);
			break;
		}
		case 19:
		{
			for (int num3 = 0; num3 < 40; num3++)
			{
				if (Main.travelShop[num3] != 0)
				{
					array[num].netDefaults(Main.travelShop[num3]);
					num++;
				}
			}
			break;
		}
		case 20:
			if (Main.moonPhase == 0)
			{
				array[num].SetDefaults(284);
				num++;
			}
			if (Main.moonPhase == 1)
			{
				array[num].SetDefaults(946);
				num++;
			}
			if (Main.moonPhase == 2 && !Main.remixWorld)
			{
				array[num].SetDefaults(3069);
				num++;
			}
			if (Main.moonPhase == 2 && Main.remixWorld)
			{
				array[num].SetDefaults(517);
				num++;
			}
			if (Main.moonPhase == 3)
			{
				array[num].SetDefaults(4341);
				num++;
			}
			if (Main.moonPhase == 4)
			{
				array[num].SetDefaults(285);
				num++;
			}
			if (Main.moonPhase == 5)
			{
				array[num].SetDefaults(953);
				num++;
			}
			if (Main.moonPhase == 6)
			{
				array[num].SetDefaults(3068);
				num++;
			}
			if (Main.moonPhase == 7)
			{
				array[num].SetDefaults(3084);
				num++;
			}
			if (Main.moonPhase % 2 == 0)
			{
				array[num].SetDefaults(3001);
				num++;
			}
			if (Main.moonPhase % 2 != 0)
			{
				array[num].SetDefaults(28);
				num++;
			}
			if (Main.moonPhase % 2 != 0 && Main.hardMode)
			{
				array[num].SetDefaults(188);
				num++;
			}
			if (!Main.dayTime || Main.moonPhase == 0)
			{
				array[num].SetDefaults(3002);
				num++;
				if (Main.player[Main.myPlayer].HasItem(930))
				{
					array[num].SetDefaults(5377);
					num++;
				}
			}
			else if (Main.dayTime && Main.moonPhase != 0)
			{
				array[num].SetDefaults(282);
				num++;
			}
			if (Main.time % 60.0 * 60.0 * 6.0 <= 10800.0)
			{
				array[num].SetDefaults(3004);
			}
			else
			{
				array[num].SetDefaults(8);
			}
			num++;
			if (Main.moonPhase == 0 || Main.moonPhase == 1 || Main.moonPhase == 4 || Main.moonPhase == 5)
			{
				array[num].SetDefaults(3003);
			}
			else
			{
				array[num].SetDefaults(40);
			}
			num++;
			if (Main.moonPhase % 4 == 0)
			{
				array[num].SetDefaults(3310);
			}
			else if (Main.moonPhase % 4 == 1)
			{
				array[num].SetDefaults(3313);
			}
			else if (Main.moonPhase % 4 == 2)
			{
				array[num].SetDefaults(3312);
			}
			else
			{
				array[num].SetDefaults(3311);
			}
			num++;
			array[num].SetDefaults(166);
			num++;
			array[num].SetDefaults(965);
			num++;
			if (Main.hardMode)
			{
				if (Main.moonPhase < 4)
				{
					array[num].SetDefaults(3316);
				}
				else
				{
					array[num].SetDefaults(3315);
				}
				num++;
				array[num].SetDefaults(3334);
				num++;
				if (Main.bloodMoon)
				{
					array[num].SetDefaults(3258);
					num++;
				}
			}
			if (Main.moonPhase == 0 && !Main.dayTime)
			{
				array[num].SetDefaults(3043);
				num++;
			}
			if (!Main.player[Main.myPlayer].ateArtisanBread && Main.moonPhase >= 3 && Main.moonPhase <= 5)
			{
				array[num].SetDefaults(5326);
				num++;
			}
			break;
		case 21:
		{
			bool flag2 = Main.hardMode && NPC.downedMechBossAny;
			bool num14 = Main.hardMode && NPC.downedGolemBoss;
			array[num].SetDefaults(353);
			num++;
			array[num].SetDefaults(3828);
			if (num14)
			{
				array[num].shopCustomPrice = Item.buyPrice(0, 4);
			}
			else if (flag2)
			{
				array[num].shopCustomPrice = Item.buyPrice(0, 1);
			}
			else
			{
				array[num].shopCustomPrice = Item.buyPrice(0, 0, 25);
			}
			num++;
			array[num].SetDefaults(3816);
			num++;
			array[num].SetDefaults(3813);
			array[num].shopCustomPrice = 50;
			array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			num++;
			num = 10;
			array[num].SetDefaults(3818);
			array[num].shopCustomPrice = 5;
			array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			num++;
			array[num].SetDefaults(3824);
			array[num].shopCustomPrice = 5;
			array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			num++;
			array[num].SetDefaults(3832);
			array[num].shopCustomPrice = 5;
			array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			num++;
			array[num].SetDefaults(3829);
			array[num].shopCustomPrice = 5;
			array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			if (flag2)
			{
				num = 20;
				array[num].SetDefaults(3819);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3825);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3833);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3830);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			}
			if (num14)
			{
				num = 30;
				array[num].SetDefaults(3820);
				array[num].shopCustomPrice = 60;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3826);
				array[num].shopCustomPrice = 60;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3834);
				array[num].shopCustomPrice = 60;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3831);
				array[num].shopCustomPrice = 60;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
			}
			if (flag2)
			{
				num = 4;
				array[num].SetDefaults(3800);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3801);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3802);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 14;
				array[num].SetDefaults(3797);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3798);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3799);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 24;
				array[num].SetDefaults(3803);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3804);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3805);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 34;
				array[num].SetDefaults(3806);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3807);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3808);
				array[num].shopCustomPrice = 15;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
			}
			if (num14)
			{
				num = 7;
				array[num].SetDefaults(3871);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3872);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3873);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 17;
				array[num].SetDefaults(3874);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3875);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3876);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 27;
				array[num].SetDefaults(3877);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3878);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3879);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				num = 37;
				array[num].SetDefaults(3880);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3881);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
				array[num].SetDefaults(3882);
				array[num].shopCustomPrice = 50;
				array[num].shopSpecialCurrency = CustomCurrencyID.DefenderMedals;
				num++;
			}
			num = (num14 ? 39 : ((!flag2) ? 4 : 30));
			break;
		}
		case 22:
		{
			array[num++].SetDefaults(4587);
			array[num++].SetDefaults(4590);
			array[num++].SetDefaults(4589);
			array[num++].SetDefaults(4588);
			array[num++].SetDefaults(4083);
			array[num++].SetDefaults(4084);
			array[num++].SetDefaults(4085);
			array[num++].SetDefaults(4086);
			array[num++].SetDefaults(4087);
			array[num++].SetDefaults(4088);
			int golferScoreAccumulated2 = Main.LocalPlayer.golferScoreAccumulated;
			if (golferScoreAccumulated2 > 500)
			{
				array[num].SetDefaults(4039);
				num++;
				array[num].SetDefaults(4094);
				num++;
				array[num].SetDefaults(4093);
				num++;
				array[num].SetDefaults(4092);
				num++;
			}
			array[num++].SetDefaults(4089);
			array[num++].SetDefaults(3989);
			array[num++].SetDefaults(4095);
			array[num++].SetDefaults(4040);
			array[num++].SetDefaults(4319);
			array[num++].SetDefaults(4320);
			if (golferScoreAccumulated2 > 1000)
			{
				array[num].SetDefaults(4591);
				num++;
				array[num].SetDefaults(4594);
				num++;
				array[num].SetDefaults(4593);
				num++;
				array[num].SetDefaults(4592);
				num++;
			}
			array[num++].SetDefaults(4135);
			array[num++].SetDefaults(4138);
			array[num++].SetDefaults(4136);
			array[num++].SetDefaults(4137);
			array[num++].SetDefaults(4049);
			if (golferScoreAccumulated2 > 500)
			{
				array[num].SetDefaults(4265);
				num++;
			}
			if (golferScoreAccumulated2 > 2000)
			{
				array[num].SetDefaults(4595);
				num++;
				array[num].SetDefaults(4598);
				num++;
				array[num].SetDefaults(4597);
				num++;
				array[num].SetDefaults(4596);
				num++;
				if (NPC.downedBoss3)
				{
					array[num].SetDefaults(4264);
					num++;
				}
			}
			if (golferScoreAccumulated2 > 500)
			{
				array[num].SetDefaults(4599);
				num++;
			}
			if (golferScoreAccumulated2 >= 1000)
			{
				array[num].SetDefaults(4600);
				num++;
			}
			if (golferScoreAccumulated2 >= 2000)
			{
				array[num].SetDefaults(4601);
				num++;
			}
			if (golferScoreAccumulated2 >= 2000)
			{
				if (Main.moonPhase == 0 || Main.moonPhase == 1)
				{
					array[num].SetDefaults(4658);
					num++;
				}
				else if (Main.moonPhase == 2 || Main.moonPhase == 3)
				{
					array[num].SetDefaults(4659);
					num++;
				}
				else if (Main.moonPhase == 4 || Main.moonPhase == 5)
				{
					array[num].SetDefaults(4660);
					num++;
				}
				else if (Main.moonPhase == 6 || Main.moonPhase == 7)
				{
					array[num].SetDefaults(4661);
					num++;
				}
			}
			break;
		}
		case 23:
		{
			BestiaryUnlockProgressReport bestiaryProgressReport = Main.GetBestiaryProgressReport();
			if (BestiaryGirl_IsFairyTorchAvailable())
			{
				array[num++].SetDefaults(4776);
			}
			array[num++].SetDefaults(4767);
			array[num++].SetDefaults(4759);
			if (Main.moonPhase == 0 && !Main.dayTime)
			{
				array[num++].SetDefaults(5253);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.1f)
			{
				array[num++].SetDefaults(4672);
			}
			array[num++].SetDefaults(4829);
			if (bestiaryProgressReport.CompletionPercent >= 0.25f)
			{
				array[num++].SetDefaults(4830);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.45f)
			{
				array[num++].SetDefaults(4910);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4871);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4907);
			}
			if (NPC.downedTowerSolar)
			{
				array[num++].SetDefaults(4677);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.1f)
			{
				array[num++].SetDefaults(4676);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4762);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.25f)
			{
				array[num++].SetDefaults(4716);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4785);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4786);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f)
			{
				array[num++].SetDefaults(4787);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.3f && Main.hardMode)
			{
				array[num++].SetDefaults(4788);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.35f)
			{
				array[num++].SetDefaults(4763);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.4f)
			{
				array[num++].SetDefaults(4955);
			}
			if (Main.hardMode && Main.bloodMoon)
			{
				array[num++].SetDefaults(4736);
			}
			if (NPC.downedPlantBoss)
			{
				array[num++].SetDefaults(4701);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.5f)
			{
				array[num++].SetDefaults(4765);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.5f)
			{
				array[num++].SetDefaults(4766);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.5f)
			{
				array[num++].SetDefaults(5285);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.5f)
			{
				array[num++].SetDefaults(4777);
			}
			if (bestiaryProgressReport.CompletionPercent >= 0.7f)
			{
				array[num++].SetDefaults(4735);
			}
			if (bestiaryProgressReport.CompletionPercent >= 1f)
			{
				array[num++].SetDefaults(4951);
			}
			switch (Main.moonPhase)
			{
			case 0:
			case 1:
				array[num++].SetDefaults(4768);
				array[num++].SetDefaults(4769);
				break;
			case 2:
			case 3:
				array[num++].SetDefaults(4770);
				array[num++].SetDefaults(4771);
				break;
			case 4:
			case 5:
				array[num++].SetDefaults(4772);
				array[num++].SetDefaults(4773);
				break;
			case 6:
			case 7:
				array[num++].SetDefaults(4560);
				array[num++].SetDefaults(4775);
				break;
			}
			break;
		}
		case 24:
			array[num++].SetDefaults(5071);
			array[num++].SetDefaults(5072);
			array[num++].SetDefaults(5073);
			array[num++].SetDefaults(5076);
			array[num++].SetDefaults(5077);
			array[num++].SetDefaults(5078);
			array[num++].SetDefaults(5079);
			array[num++].SetDefaults(5080);
			array[num++].SetDefaults(5081);
			array[num++].SetDefaults(5082);
			array[num++].SetDefaults(5083);
			array[num++].SetDefaults(5084);
			array[num++].SetDefaults(5085);
			array[num++].SetDefaults(5086);
			array[num++].SetDefaults(5087);
			array[num++].SetDefaults(5310);
			array[num++].SetDefaults(5222);
			array[num++].SetDefaults(5228);
			if (NPC.downedSlimeKing && NPC.downedQueenSlime)
			{
				array[num++].SetDefaults(5266);
			}
			if (Main.hardMode && NPC.downedMoonlord)
			{
				array[num++].SetDefaults(5044);
			}
			if (Main.tenthAnniversaryWorld)
			{
				array[num++].SetDefaults(1309);
				array[num++].SetDefaults(1859);
				array[num++].SetDefaults(1358);
				if (Main.player[Main.myPlayer].ZoneDesert)
				{
					array[num++].SetDefaults(857);
				}
				if (Main.bloodMoon)
				{
					array[num++].SetDefaults(4144);
				}
				if (Main.hardMode && NPC.downedPirates)
				{
					if (Main.moonPhase == 0 || Main.moonPhase == 1)
					{
						array[num++].SetDefaults(2584);
					}
					if (Main.moonPhase == 2 || Main.moonPhase == 3)
					{
						array[num++].SetDefaults(854);
					}
					if (Main.moonPhase == 4 || Main.moonPhase == 5)
					{
						array[num++].SetDefaults(855);
					}
					if (Main.moonPhase == 6 || Main.moonPhase == 7)
					{
						array[num++].SetDefaults(905);
					}
				}
			}
			array[num++].SetDefaults(5088);
			break;
		}
		bool num15 = type != 19 && type != 20;
		bool flag3 = TeleportPylonsSystem.DoesPositionHaveEnoughNPCs(2, Main.LocalPlayer.Center.ToTileCoordinates16());
		if (num15 && (flag || Main.remixWorld) && flag3 && !Main.player[Main.myPlayer].ZoneCorrupt && !Main.player[Main.myPlayer].ZoneCrimson)
		{
			if (!Main.player[Main.myPlayer].ZoneSnow && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneBeach && !Main.player[Main.myPlayer].ZoneJungle && !Main.player[Main.myPlayer].ZoneHallow && !Main.player[Main.myPlayer].ZoneGlowshroom)
			{
				if (Main.remixWorld)
				{
					if ((double)(Main.player[Main.myPlayer].Center.Y / 16f) > Main.rockLayer && Main.player[Main.myPlayer].Center.Y / 16f < (float)(Main.maxTilesY - 350) && num < 39)
					{
						array[num++].SetDefaults(4876);
					}
				}
				else if ((double)(Main.player[Main.myPlayer].Center.Y / 16f) < Main.worldSurface && num < 39)
				{
					array[num++].SetDefaults(4876);
				}
			}
			if (Main.player[Main.myPlayer].ZoneSnow && num < 39)
			{
				array[num++].SetDefaults(4920);
			}
			if (Main.player[Main.myPlayer].ZoneDesert && num < 39)
			{
				array[num++].SetDefaults(4919);
			}
			if (Main.remixWorld)
			{
				if (!Main.player[Main.myPlayer].ZoneSnow && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneBeach && !Main.player[Main.myPlayer].ZoneJungle && !Main.player[Main.myPlayer].ZoneHallow && (double)(Main.player[Main.myPlayer].Center.Y / 16f) >= Main.worldSurface && num < 39)
				{
					array[num++].SetDefaults(4917);
				}
			}
			else if (!Main.player[Main.myPlayer].ZoneSnow && !Main.player[Main.myPlayer].ZoneDesert && !Main.player[Main.myPlayer].ZoneBeach && !Main.player[Main.myPlayer].ZoneJungle && !Main.player[Main.myPlayer].ZoneHallow && !Main.player[Main.myPlayer].ZoneGlowshroom && (double)(Main.player[Main.myPlayer].Center.Y / 16f) >= Main.worldSurface && num < 39)
			{
				array[num++].SetDefaults(4917);
			}
			bool flag4 = Main.player[Main.myPlayer].ZoneBeach && (double)Main.player[Main.myPlayer].position.Y < Main.worldSurface * 16.0;
			if (Main.remixWorld)
			{
				float num4 = Main.player[Main.myPlayer].position.X / 16f;
				float num5 = Main.player[Main.myPlayer].position.Y / 16f;
				flag4 |= ((double)num4 < (double)Main.maxTilesX * 0.43 || (double)num4 > (double)Main.maxTilesX * 0.57) && (double)num5 > Main.rockLayer && num5 < (float)(Main.maxTilesY - 350);
			}
			if (flag4 && num < 39)
			{
				array[num++].SetDefaults(4918);
			}
			if (Main.player[Main.myPlayer].ZoneJungle && num < 39)
			{
				array[num++].SetDefaults(4875);
			}
			if (Main.player[Main.myPlayer].ZoneHallow && num < 39)
			{
				array[num++].SetDefaults(4916);
			}
			if (Main.player[Main.myPlayer].ZoneGlowshroom && (!Main.remixWorld || Main.player[Main.myPlayer].Center.Y / 16f < (float)(Main.maxTilesY - 200)) && num < 39)
			{
				array[num++].SetDefaults(4921);
			}
		}
		for (int num6 = 0; num6 < num; num6++)
		{
			array[num6].isAShopItem = true;
		}
	}

	public void SetupShop(string shopName, NPC npc)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Array.Fill(this.item, null);
		if (NPCShopDatabase.TryGetNPCShop(shopName, out var shop))
		{
			shop.FillShop(this.item, npc, out var overflow);
			if (overflow)
			{
				Main.NewText(Language.GetTextValue("tModLoader.ShopOverflow"), Color.Orange);
				Logging.tML.Warn((object)("Unable to fit all item in the shop " + shopName));
			}
		}
		if (npc != null)
		{
			NPCLoader.ModifyActiveShop(npc, shopName, this.item);
		}
		Span<Item> span = this.item.AsSpan();
		for (int i = 0; i < span.Length; i++)
		{
			ref Item item = ref span[i];
			if (item == null)
			{
				item = new Item();
			}
			item.isAShopItem = true;
		}
	}

	internal static bool BestiaryGirl_IsFairyTorchAvailable()
	{
		if (!DidDiscoverBestiaryEntry(585))
		{
			return false;
		}
		if (!DidDiscoverBestiaryEntry(584))
		{
			return false;
		}
		if (!DidDiscoverBestiaryEntry(583))
		{
			return false;
		}
		return true;
	}

	private static bool DidDiscoverBestiaryEntry(int npcId)
	{
		return Main.BestiaryDB.FindEntryByNPCID(npcId).UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
	}

	/// <summary>
	/// Visually opens the chest at <paramref name="worldPosition" /> for <paramref name="duration" /> ticks.
	/// </summary>
	/// <param name="worldPosition">The position of the chest to ask, in tile coordinates.</param>
	/// <param name="duration">The duration of the eating animation in ticks.</param>
	public static void AskForChestToEatItem(Vector2 worldPosition, int duration)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Point point = worldPosition.ToTileCoordinates();
		int num = FindChest(point.X, point.Y);
		if (num != -1)
		{
			Main.chest[num].eatingAnimationTime = duration;
		}
	}

	/// <summary>
	/// Visually updates all chests' frames.
	/// <br /> Chests that are being used (<see cref="M:Terraria.Chest.GetCurrentlyOpenChests" />) or eating an item (<see cref="F:Terraria.Chest.eatingAnimationTime" />) will not be opened.
	/// </summary>
	public static void UpdateChestFrames()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		int num = 8000;
		_chestInUse.Clear();
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].active && Main.player[i].chest >= 0 && Main.player[i].chest < num)
			{
				_chestInUse.Add(Main.player[i].chest);
			}
		}
		Chest chest = null;
		for (int j = 0; j < num; j++)
		{
			chest = Main.chest[j];
			if (chest != null)
			{
				if (_chestInUse.Contains(j))
				{
					chest.frameCounter++;
				}
				else
				{
					chest.frameCounter--;
				}
				if (chest.eatingAnimationTime == 9 && chest.frame == 1)
				{
					SoundEngine.PlaySound(7, new Vector2((float)(chest.x * 16 + 16), (float)(chest.y * 16 + 16)));
				}
				if (chest.eatingAnimationTime > 0)
				{
					chest.eatingAnimationTime--;
				}
				if (chest.frameCounter < chest.eatingAnimationTime)
				{
					chest.frameCounter = chest.eatingAnimationTime;
				}
				if (chest.frameCounter < 0)
				{
					chest.frameCounter = 0;
				}
				if (chest.frameCounter > 10)
				{
					chest.frameCounter = 10;
				}
				if (chest.frameCounter == 0)
				{
					chest.frame = 0;
				}
				else if (chest.frameCounter == 10)
				{
					chest.frame = 2;
				}
				else
				{
					chest.frame = 1;
				}
			}
		}
	}

	/// <summary>
	/// Calls <see cref="M:Terraria.Item.FixAgainstExploit" /> on all of this chest's items.
	/// </summary>
	public void FixLoadedData()
	{
		Item[] array = item;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].FixAgainstExploit();
		}
	}
}
