using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Chat;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.UI;

public class ItemSlot
{
	public class Options
	{
		public static bool DisableLeftShiftTrashCan = true;

		public static bool DisableQuickTrash = false;

		public static bool HighlightNewItems = true;
	}

	public class Context
	{
		public const int ModdedAccessorySlot = -10;

		public const int ModdedVanityAccessorySlot = -11;

		public const int ModdedDyeSlot = -12;

		public const int InventoryItem = 0;

		public const int InventoryCoin = 1;

		public const int InventoryAmmo = 2;

		public const int ChestItem = 3;

		public const int BankItem = 4;

		public const int PrefixItem = 5;

		public const int TrashItem = 6;

		public const int GuideItem = 7;

		public const int EquipArmor = 8;

		public const int EquipArmorVanity = 9;

		public const int EquipAccessory = 10;

		public const int EquipAccessoryVanity = 11;

		public const int EquipDye = 12;

		public const int HotbarItem = 13;

		public const int ChatItem = 14;

		public const int ShopItem = 15;

		public const int EquipGrapple = 16;

		public const int EquipMount = 17;

		public const int EquipMinecart = 18;

		public const int EquipPet = 19;

		public const int EquipLight = 20;

		public const int MouseItem = 21;

		public const int CraftingMaterial = 22;

		public const int DisplayDollArmor = 23;

		public const int DisplayDollAccessory = 24;

		public const int DisplayDollDye = 25;

		public const int HatRackHat = 26;

		public const int HatRackDye = 27;

		public const int GoldDebug = 28;

		public const int CreativeInfinite = 29;

		public const int CreativeSacrifice = 30;

		public const int InWorld = 31;

		public const int VoidItem = 32;

		public const int EquipMiscDye = 33;

		public static readonly int Count = 34;
	}

	public struct ItemTransferInfo
	{
		public int ItemType;

		public int TransferAmount;

		public int FromContenxt;

		public int ToContext;

		public ItemTransferInfo(Item itemAfter, int fromContext, int toContext, int transferAmount = 0)
		{
			ItemType = itemAfter.type;
			TransferAmount = itemAfter.stack;
			if (transferAmount != 0)
			{
				TransferAmount = transferAmount;
			}
			FromContenxt = fromContext;
			ToContext = toContext;
		}
	}

	public delegate void ItemTransferEvent(ItemTransferInfo info);

	public static bool DrawGoldBGForCraftingMaterial;

	public static bool ShiftForcedOn;

	internal static Item[] singleSlotArray;

	private static bool[] canFavoriteAt;

	private static bool[] canShareAt;

	private static float[] inventoryGlowHue;

	private static int[] inventoryGlowTime;

	private static float[] inventoryGlowHueChest;

	private static int[] inventoryGlowTimeChest;

	private static int _customCurrencyForSavings;

	public static bool forceClearGlowsOnChest;

	private static double _lastTimeForVisualEffectsThatLoadoutWasChanged;

	private static Color[,] LoadoutSlotColors;

	private static int dyeSlotCount;

	private static int accSlotToSwapTo;

	public static float CircularRadialOpacity;

	public static float QuicksRadialOpacity;

	public static bool ShiftInUse
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			if (!Main.keyState.PressingShift())
			{
				return ShiftForcedOn;
			}
			return true;
		}
	}

	public static bool ControlInUse => Main.keyState.PressingControl();

	public static bool NotUsingGamepad => !PlayerInput.UsingGamepad;

	public static event ItemTransferEvent OnItemTransferred;

	static ItemSlot()
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		DrawGoldBGForCraftingMaterial = false;
		singleSlotArray = new Item[1];
		canFavoriteAt = new bool[Context.Count];
		canShareAt = new bool[Context.Count];
		inventoryGlowHue = new float[58];
		inventoryGlowTime = new int[58];
		inventoryGlowHueChest = new float[58];
		inventoryGlowTimeChest = new int[58];
		_customCurrencyForSavings = -1;
		forceClearGlowsOnChest = false;
		LoadoutSlotColors = new Color[3, 3]
		{
			{
				new Color(50, 106, 64),
				new Color(46, 106, 98),
				new Color(45, 85, 105)
			},
			{
				new Color(35, 106, 126),
				new Color(50, 89, 140),
				new Color(57, 70, 128)
			},
			{
				new Color(122, 63, 83),
				new Color(104, 46, 85),
				new Color(84, 37, 87)
			}
		};
		canFavoriteAt[0] = true;
		canFavoriteAt[1] = true;
		canFavoriteAt[2] = true;
		canFavoriteAt[32] = true;
		canShareAt[15] = true;
		canShareAt[4] = true;
		canShareAt[32] = true;
		canShareAt[5] = true;
		canShareAt[6] = true;
		canShareAt[7] = true;
		canShareAt[27] = true;
		canShareAt[26] = true;
		canShareAt[23] = true;
		canShareAt[24] = true;
		canShareAt[25] = true;
		canShareAt[22] = true;
		canShareAt[3] = true;
		canShareAt[8] = true;
		canShareAt[9] = true;
		canShareAt[10] = true;
		canShareAt[11] = true;
		canShareAt[12] = true;
		canShareAt[33] = true;
		canShareAt[16] = true;
		canShareAt[20] = true;
		canShareAt[18] = true;
		canShareAt[19] = true;
		canShareAt[17] = true;
		canShareAt[29] = true;
		canShareAt[30] = true;
	}

	public static void AnnounceTransfer(ItemTransferInfo info)
	{
		if (ItemSlot.OnItemTransferred != null)
		{
			ItemSlot.OnItemTransferred(info);
		}
	}

	public static void SetGlow(int index, float hue, bool chest)
	{
		if (chest)
		{
			if (hue < 0f)
			{
				inventoryGlowTimeChest[index] = 0;
				inventoryGlowHueChest[index] = 0f;
			}
			else
			{
				inventoryGlowTimeChest[index] = 300;
				inventoryGlowHueChest[index] = hue;
			}
		}
		else
		{
			inventoryGlowTime[index] = 300;
			inventoryGlowHue[index] = hue;
		}
	}

	public static void UpdateInterface()
	{
		if (!Main.playerInventory || Main.player[Main.myPlayer].talkNPC == -1)
		{
			_customCurrencyForSavings = -1;
		}
		for (int i = 0; i < inventoryGlowTime.Length; i++)
		{
			if (inventoryGlowTime[i] > 0)
			{
				inventoryGlowTime[i]--;
				if (inventoryGlowTime[i] == 0)
				{
					inventoryGlowHue[i] = 0f;
				}
			}
		}
		for (int j = 0; j < inventoryGlowTimeChest.Length; j++)
		{
			if (inventoryGlowTimeChest[j] > 0)
			{
				inventoryGlowTimeChest[j]--;
				if (inventoryGlowTimeChest[j] == 0 || forceClearGlowsOnChest)
				{
					inventoryGlowHueChest[j] = 0f;
				}
			}
		}
		forceClearGlowsOnChest = false;
	}

	public static void Handle(ref Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		Handle(singleSlotArray, context);
		inv = singleSlotArray[0];
		Recipe.FindRecipes();
	}

	public static void Handle(Item[] inv, int context = 0, int slot = 0)
	{
		OverrideHover(inv, context, slot);
		LeftClick(inv, context, slot);
		RightClick(inv, context, slot);
		if (Main.mouseLeftRelease && Main.mouseLeft)
		{
			Recipe.FindRecipes();
		}
		MouseHover(inv, context, slot);
	}

	public static void OverrideHover(ref Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		OverrideHover(singleSlotArray, context);
		inv = singleSlotArray[0];
	}

	public static bool isEquipLocked(int type)
	{
		return false;
	}

	public static void OverrideHover(Item[] inv, int context = 0, int slot = 0)
	{
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		Item item = inv[slot];
		if (!PlayerInput.UsingGamepad)
		{
			UILinkPointNavigator.SuggestUsage(GetGamepadPointForSlot(inv, context, slot));
		}
		if (PlayerLoader.HoverSlot(Main.player[Main.myPlayer], inv, context, slot))
		{
			return;
		}
		bool shiftForcedOn = ShiftForcedOn;
		if (NotUsingGamepad && Options.DisableLeftShiftTrashCan && !shiftForcedOn)
		{
			if (ControlInUse && !Options.DisableQuickTrash)
			{
				if (item.type > 0 && item.stack > 0 && !inv[slot].favorited)
				{
					switch (context)
					{
					case 0:
					case 1:
					case 2:
						if (Main.npcShop > 0 && !item.favorited)
						{
							Main.cursorOverride = 10;
						}
						else
						{
							Main.cursorOverride = 6;
						}
						break;
					case 3:
					case 4:
					case 7:
					case 32:
						if (Main.player[Main.myPlayer].ItemSpace(item).CanTakeItemToPersonalInventory)
						{
							Main.cursorOverride = 6;
						}
						break;
					}
				}
			}
			else if (ShiftInUse)
			{
				bool flag = false;
				if (Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity())
				{
					flag = Main.LocalPlayer.tileEntityAnchor.GetTileEntity().OverrideItemSlotHover(inv, context, slot);
				}
				if (item.type > 0 && item.stack > 0 && !inv[slot].favorited && !flag)
				{
					switch (context)
					{
					case 0:
						if (Main.CreativeMenu.IsShowingResearchMenu())
						{
							Main.cursorOverride = 9;
							break;
						}
						goto case 1;
					case 1:
					case 2:
						if (context == 0 && Main.InReforgeMenu)
						{
							if (item.Prefix(-3))
							{
								Main.cursorOverride = 9;
							}
						}
						else if (context == 0 && Main.InGuideCraftMenu)
						{
							if (item.material)
							{
								Main.cursorOverride = 9;
							}
						}
						else if (Main.player[Main.myPlayer].chest != -1 && ChestUI.TryPlacingInChest(item, justCheck: true, context))
						{
							Main.cursorOverride = 9;
						}
						break;
					case 3:
					case 4:
					case 32:
						if (Main.player[Main.myPlayer].ItemSpace(item).CanTakeItemToPersonalInventory)
						{
							Main.cursorOverride = 8;
						}
						break;
					case 5:
					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 18:
					case 19:
					case 20:
					case 23:
					case 24:
					case 25:
					case 26:
					case 27:
					case 29:
					case 33:
						if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
						{
							Main.cursorOverride = 7;
						}
						break;
					}
				}
			}
		}
		else if (ShiftInUse)
		{
			bool flag2 = false;
			if (Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity())
			{
				flag2 = Main.LocalPlayer.tileEntityAnchor.GetTileEntity().OverrideItemSlotHover(inv, context, slot);
			}
			if (item.type > 0 && item.stack > 0 && !inv[slot].favorited && !flag2)
			{
				switch (context)
				{
				case 0:
				case 1:
				case 2:
					if (Main.npcShop > 0 && !item.favorited)
					{
						if (!Options.DisableQuickTrash)
						{
							Main.cursorOverride = 10;
						}
					}
					else if (context == 0 && Main.CreativeMenu.IsShowingResearchMenu())
					{
						Main.cursorOverride = 9;
					}
					else if (context == 0 && Main.InReforgeMenu)
					{
						if (item.Prefix(-3))
						{
							Main.cursorOverride = 9;
						}
					}
					else if (context == 0 && Main.InGuideCraftMenu)
					{
						if (item.material)
						{
							Main.cursorOverride = 9;
						}
					}
					else if (Main.player[Main.myPlayer].chest != -1)
					{
						if (ChestUI.TryPlacingInChest(item, justCheck: true, context))
						{
							Main.cursorOverride = 9;
						}
					}
					else if (!Options.DisableQuickTrash)
					{
						Main.cursorOverride = 6;
					}
					break;
				case 3:
				case 4:
				case 32:
					if (Main.player[Main.myPlayer].ItemSpace(item).CanTakeItemToPersonalInventory)
					{
						Main.cursorOverride = 8;
					}
					break;
				case 5:
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 23:
				case 24:
				case 25:
				case 26:
				case 27:
				case 29:
				case 33:
					if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
					{
						Main.cursorOverride = 7;
					}
					break;
				}
			}
		}
		if (((KeyboardState)(ref Main.keyState)).IsKeyDown(Main.FavoriteKey) && (canFavoriteAt[context] || (Main.drawingPlayerChat && canShareAt[context])))
		{
			if (item.type > 0 && item.stack > 0 && Main.drawingPlayerChat)
			{
				Main.cursorOverride = 2;
			}
			else if (item.type > 0 && item.stack > 0)
			{
				Main.cursorOverride = 3;
			}
		}
	}

	private static bool OverrideLeftClick(Item[] inv, int context = 0, int slot = 0)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		if (Math.Abs(context) == 10 && isEquipLocked(inv[slot].type))
		{
			return true;
		}
		if (Main.LocalPlayer.tileEntityAnchor.IsInValidUseTileEntity() && Main.LocalPlayer.tileEntityAnchor.GetTileEntity().OverrideItemSlotLeftClick(inv, context, slot))
		{
			return true;
		}
		Item item = inv[slot];
		if (ShiftInUse && PlayerLoader.ShiftClickSlot(Main.player[Main.myPlayer], inv, context, slot))
		{
			return true;
		}
		if (Main.cursorOverride == 2)
		{
			if (ChatManager.AddChatText(FontAssets.MouseText.Value, ItemTagHandler.GenerateTag(item), Vector2.One))
			{
				SoundEngine.PlaySound(12);
			}
			return true;
		}
		if (Main.cursorOverride == 3)
		{
			if (!canFavoriteAt[Math.Abs(context)])
			{
				return false;
			}
			item.favorited = !item.favorited;
			SoundEngine.PlaySound(12);
			return true;
		}
		if (Main.cursorOverride == 7)
		{
			if (context == 29)
			{
				Item item2 = inv[slot].Clone();
				item2.stack = item2.maxStack;
				item2.OnCreated(new JourneyDuplicationItemCreationContext());
				item2 = Main.player[Main.myPlayer].GetItem(Main.myPlayer, item2, GetItemSettings.InventoryEntityToPlayerInventorySettings);
				SoundEngine.PlaySound(12);
				return true;
			}
			inv[slot] = Main.player[Main.myPlayer].GetItem(Main.myPlayer, inv[slot], GetItemSettings.InventoryEntityToPlayerInventorySettings);
			SoundEngine.PlaySound(12);
			return true;
		}
		if (Main.cursorOverride == 8)
		{
			inv[slot] = Main.player[Main.myPlayer].GetItem(Main.myPlayer, inv[slot], GetItemSettings.InventoryEntityToPlayerInventorySettings);
			if (Main.player[Main.myPlayer].chest > -1)
			{
				NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, slot);
			}
			return true;
		}
		if (Main.cursorOverride == 9)
		{
			if (Main.CreativeMenu.IsShowingResearchMenu())
			{
				Main.CreativeMenu.SwapItem(ref inv[slot]);
				SoundEngine.PlaySound(7);
				Main.CreativeMenu.SacrificeItemInSacrificeSlot();
			}
			else if (Main.InReforgeMenu)
			{
				Utils.Swap(ref inv[slot], ref Main.reforgeItem);
				SoundEngine.PlaySound(7);
			}
			else if (Main.InGuideCraftMenu)
			{
				Utils.Swap(ref inv[slot], ref Main.guideItem);
				Recipe.FindRecipes();
				SoundEngine.PlaySound(7);
			}
			else
			{
				ChestUI.TryPlacingInChest(inv[slot], justCheck: false, context);
			}
			return true;
		}
		return false;
	}

	public static void LeftClick(ref Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		LeftClick(singleSlotArray, context);
		inv = singleSlotArray[0];
	}

	private static bool IsAccessoryContext(int context)
	{
		int num = Math.Abs(context);
		return num == 10 || num == 11;
	}

	public static void LeftClick(Item[] inv, int context = 0, int slot = 0)
	{
		Player player = Main.player[Main.myPlayer];
		bool flag = Main.mouseLeftRelease && Main.mouseLeft;
		if (flag)
		{
			if (OverrideLeftClick(inv, context, slot))
			{
				return;
			}
			inv[slot].newAndShiny = false;
			if (LeftClick_SellOrTrash(inv, context, slot) || player.itemAnimation != 0 || player.itemTime != 0)
			{
				return;
			}
		}
		int num = PickItemMovementAction(inv, context, slot, Main.mouseItem);
		if (num != 3 && !flag)
		{
			return;
		}
		switch (num)
		{
		case 0:
		{
			if (context == 6 && Main.mouseItem.type != 0)
			{
				inv[slot].SetDefaults();
			}
			if ((IsAccessoryContext(context) && !ItemLoader.CanEquipAccessory(inv[slot], slot, context < 0)) || (context == 11 && !inv[slot].FitsAccessoryVanitySlot) || (context < 0 && !LoaderManager.Get<AccessorySlotLoader>().CanAcceptItem(slot, inv[slot], context)))
			{
				break;
			}
			if (Main.mouseItem.maxStack <= 1 || inv[slot].type != Main.mouseItem.type || inv[slot].stack == inv[slot].maxStack || Main.mouseItem.stack == Main.mouseItem.maxStack)
			{
				Utils.Swap(ref inv[slot], ref Main.mouseItem);
			}
			if (inv[slot].stack > 0)
			{
				AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, context, inv[slot].stack));
			}
			else
			{
				AnnounceTransfer(new ItemTransferInfo(Main.mouseItem, context, 21, Main.mouseItem.stack));
			}
			if (inv[slot].stack > 0)
			{
				switch (Math.Abs(context))
				{
				case 0:
					AchievementsHelper.NotifyItemPickup(player, inv[slot]);
					break;
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 16:
				case 17:
				case 25:
				case 27:
				case 33:
					AchievementsHelper.HandleOnEquip(player, inv[slot], context);
					break;
				}
			}
			if (inv[slot].type == 0 || inv[slot].stack < 1)
			{
				inv[slot] = new Item();
			}
			if (Main.mouseItem.IsTheSameAs(inv[slot]) && inv[slot].stack != inv[slot].maxStack && Main.mouseItem.stack != Main.mouseItem.maxStack && ItemLoader.TryStackItems(inv[slot], Main.mouseItem, out var numTransfered))
			{
				AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, context, numTransfered));
			}
			if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
			{
				Main.mouseItem = new Item();
			}
			if (Main.mouseItem.type > 0 || inv[slot].type > 0)
			{
				Recipe.FindRecipes();
				SoundEngine.PlaySound(7);
			}
			if (context == 3 && Main.netMode == 1)
			{
				NetMessage.SendData(32, -1, -1, null, player.chest, slot);
			}
			break;
		}
		case 1:
			if (Main.mouseItem.stack == 1 && Main.mouseItem.type > 0 && inv[slot].type > 0 && inv[slot].IsNotTheSameAs(Main.mouseItem) && (context != 11 || Main.mouseItem.FitsAccessoryVanitySlot))
			{
				if ((IsAccessoryContext(context) && !ItemLoader.CanEquipAccessory(Main.mouseItem, slot, context < 0)) || (Math.Abs(context) == 11 && !Main.mouseItem.FitsAccessoryVanitySlot) || (context < 0 && !LoaderManager.Get<AccessorySlotLoader>().CanAcceptItem(slot, Main.mouseItem, context)))
				{
					break;
				}
				Utils.Swap(ref inv[slot], ref Main.mouseItem);
				SoundEngine.PlaySound(7);
				if (inv[slot].stack > 0)
				{
					switch (Math.Abs(context))
					{
					case 0:
						AchievementsHelper.NotifyItemPickup(player, inv[slot]);
						break;
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 25:
					case 27:
					case 33:
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
						break;
					}
				}
			}
			else if (Main.mouseItem.type == 0 && inv[slot].type > 0)
			{
				Utils.Swap(ref inv[slot], ref Main.mouseItem);
				if (inv[slot].type == 0 || inv[slot].stack < 1)
				{
					inv[slot] = new Item();
				}
				if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
				{
					Main.mouseItem = new Item();
				}
				if (Main.mouseItem.type > 0 || inv[slot].type > 0)
				{
					Recipe.FindRecipes();
					SoundEngine.PlaySound(7);
				}
			}
			else if (Main.mouseItem.type > 0 && inv[slot].type == 0 && (context != 11 || Main.mouseItem.FitsAccessoryVanitySlot))
			{
				if ((IsAccessoryContext(context) && !ItemLoader.CanEquipAccessory(Main.mouseItem, slot, context < 0)) || (Math.Abs(context) == 11 && !Main.mouseItem.FitsAccessoryVanitySlot) || (context < 0 && !LoaderManager.Get<AccessorySlotLoader>().CanAcceptItem(slot, Main.mouseItem, context)))
				{
					break;
				}
				inv[slot] = ItemLoader.TransferWithLimit(Main.mouseItem, 1);
				Recipe.FindRecipes();
				SoundEngine.PlaySound(7);
				if (inv[slot].stack > 0)
				{
					switch (Math.Abs(context))
					{
					case 0:
						AchievementsHelper.NotifyItemPickup(player, inv[slot]);
						break;
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 25:
					case 27:
					case 33:
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
						break;
					}
				}
			}
			if ((context == 23 || context == 24) && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
			}
			if (context == 26 && Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
			}
			break;
		case 2:
			if (Main.mouseItem.stack == 1 && Main.mouseItem.dye > 0 && inv[slot].type > 0 && inv[slot].type != Main.mouseItem.type)
			{
				Utils.Swap(ref inv[slot], ref Main.mouseItem);
				SoundEngine.PlaySound(7);
				if (inv[slot].stack > 0)
				{
					switch (Math.Abs(context))
					{
					case 0:
						AchievementsHelper.NotifyItemPickup(player, inv[slot]);
						break;
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 25:
					case 27:
					case 33:
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
						break;
					}
				}
			}
			else if (Main.mouseItem.type == 0 && inv[slot].type > 0)
			{
				Utils.Swap(ref inv[slot], ref Main.mouseItem);
				if (inv[slot].type == 0 || inv[slot].stack < 1)
				{
					inv[slot] = new Item();
				}
				if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
				{
					Main.mouseItem = new Item();
				}
				if (Main.mouseItem.type > 0 || inv[slot].type > 0)
				{
					Recipe.FindRecipes();
					SoundEngine.PlaySound(7);
				}
			}
			else if (Main.mouseItem.dye > 0 && inv[slot].type == 0)
			{
				inv[slot] = ItemLoader.TransferWithLimit(Main.mouseItem, 1);
				Recipe.FindRecipes();
				SoundEngine.PlaySound(7);
				if (inv[slot].stack > 0)
				{
					switch (Math.Abs(context))
					{
					case 0:
						AchievementsHelper.NotifyItemPickup(player, inv[slot]);
						break;
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 16:
					case 17:
					case 25:
					case 27:
					case 33:
						AchievementsHelper.HandleOnEquip(player, inv[slot], context);
						break;
					}
				}
			}
			if (context == 25 && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
			}
			if (context == 27 && Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
			}
			break;
		case 3:
			HandleShopSlot(inv, slot, rightClickIsValid: false, leftClickIsValid: true);
			break;
		case 4:
			if (PlayerLoader.CanSellItem(player, player.TalkNPC, inv, Main.mouseItem))
			{
				Chest chest = Main.instance.shop[Main.npcShop];
				if (player.SellItem(Main.mouseItem))
				{
					int soldItemIndex = chest.AddItemToShop(Main.mouseItem);
					Main.mouseItem.SetDefaults();
					SoundEngine.PlaySound(18);
					AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, 15));
					PlayerLoader.PostSellItem(player, player.TalkNPC, chest.item, chest.item[soldItemIndex]);
				}
				else if (Main.mouseItem.value == 0)
				{
					int soldItemIndex2 = chest.AddItemToShop(Main.mouseItem);
					Main.mouseItem.SetDefaults();
					SoundEngine.PlaySound(7);
					AnnounceTransfer(new ItemTransferInfo(inv[slot], 21, 15));
					PlayerLoader.PostSellItem(player, player.TalkNPC, chest.item, chest.item[soldItemIndex2]);
				}
				Recipe.FindRecipes();
				Main.stackSplit = 9999;
			}
			break;
		case 5:
			if (Main.mouseItem.IsAir)
			{
				SoundEngine.PlaySound(7);
				Main.mouseItem = inv[slot].Clone();
				Main.mouseItem.stack = Main.mouseItem.maxStack;
				Main.mouseItem.OnCreated(new JourneyDuplicationItemCreationContext());
				AnnounceTransfer(new ItemTransferInfo(inv[slot], 29, 21));
			}
			break;
		}
		if ((uint)context > 2u && context != 5 && context != 32)
		{
			inv[slot].favorited = false;
		}
	}

	private static bool DisableTrashing()
	{
		if (Options.DisableLeftShiftTrashCan)
		{
			return !PlayerInput.SteamDeckIsUsed;
		}
		return false;
	}

	private static bool LeftClick_SellOrTrash(Item[] inv, int context, int slot)
	{
		bool flag = false;
		bool result = false;
		if (NotUsingGamepad && Options.DisableLeftShiftTrashCan)
		{
			if (!Options.DisableQuickTrash)
			{
				if (((uint)context <= 4u && context >= 0) || context == 7 || context == 32)
				{
					flag = true;
				}
				if (ControlInUse && flag)
				{
					SellOrTrash(inv, context, slot);
					result = true;
				}
			}
		}
		else
		{
			if (((uint)context <= 4u && context >= 0) || context == 32)
			{
				flag = Main.player[Main.myPlayer].chest == -1;
			}
			if (ShiftInUse && flag && (!NotUsingGamepad || !Options.DisableQuickTrash))
			{
				SellOrTrash(inv, context, slot);
				result = true;
			}
		}
		return result;
	}

	public static void SellOrTrash(Item[] inv, int context, int slot)
	{
		Player player = Main.player[Main.myPlayer];
		if (inv[slot].type <= 0)
		{
			return;
		}
		if (Main.npcShop > 0 && !inv[slot].favorited)
		{
			Chest chest = Main.instance.shop[Main.npcShop];
			if ((inv[slot].type < 71 || inv[slot].type > 74) && PlayerLoader.CanSellItem(player, player.TalkNPC, chest.item, inv[slot]))
			{
				if (player.SellItem(inv[slot]))
				{
					AnnounceTransfer(new ItemTransferInfo(inv[slot], context, 15));
					int soldItemIndex = chest.AddItemToShop(inv[slot]);
					inv[slot].TurnToAir();
					SoundEngine.PlaySound(18);
					Recipe.FindRecipes();
					PlayerLoader.PostSellItem(player, player.TalkNPC, chest.item, chest.item[soldItemIndex]);
				}
				else if (inv[slot].value == 0)
				{
					AnnounceTransfer(new ItemTransferInfo(inv[slot], context, 15));
					int soldItemIndex2 = chest.AddItemToShop(inv[slot]);
					inv[slot].TurnToAir();
					SoundEngine.PlaySound(7);
					Recipe.FindRecipes();
					PlayerLoader.PostSellItem(player, player.TalkNPC, chest.item, chest.item[soldItemIndex2]);
				}
			}
		}
		else if (!inv[slot].favorited)
		{
			SoundEngine.PlaySound(7);
			player.trashItem = inv[slot].Clone();
			AnnounceTransfer(new ItemTransferInfo(player.trashItem, context, 6));
			inv[slot].TurnToAir();
			if (context == 3 && Main.netMode == 1)
			{
				NetMessage.SendData(32, -1, -1, null, player.chest, slot);
			}
			Recipe.FindRecipes();
		}
	}

	private static string GetOverrideInstructions(Item[] inv, int context, int slot)
	{
		Player player = Main.player[Main.myPlayer];
		TileEntity tileEntity = player.tileEntityAnchor.GetTileEntity();
		if (tileEntity != null && tileEntity.TryGetItemGamepadOverrideInstructions(inv, context, slot, out var instruction))
		{
			return instruction;
		}
		if (inv[slot].type > 0 && inv[slot].stack > 0)
		{
			if (!inv[slot].favorited)
			{
				switch (context)
				{
				case 0:
				case 1:
				case 2:
					if (Main.npcShop > 0 && !inv[slot].favorited)
					{
						return Lang.misc[75].Value;
					}
					if (Main.player[Main.myPlayer].chest != -1)
					{
						if (ChestUI.TryPlacingInChest(inv[slot], justCheck: true, context))
						{
							return Lang.misc[76].Value;
						}
					}
					else if (Main.InGuideCraftMenu && inv[slot].material)
					{
						return Lang.misc[76].Value;
					}
					if (Main.mouseItem.type > 0 && (context == 0 || context == 1 || context == 2 || context == 6 || context == 15 || context == 7 || context == 4 || context == 32 || context == 3))
					{
						return null;
					}
					return Lang.misc[74].Value;
				case 3:
				case 4:
				case 32:
					if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
					{
						return Lang.misc[76].Value;
					}
					break;
				case 5:
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 16:
				case 17:
				case 18:
				case 19:
				case 20:
				case 25:
				case 27:
				case 33:
					if (Main.player[Main.myPlayer].ItemSpace(inv[slot]).CanTakeItemToPersonalInventory)
					{
						return Lang.misc[68].Value;
					}
					break;
				}
			}
			bool flag = false;
			if ((uint)context <= 4u || context == 32)
			{
				flag = player.chest == -1;
			}
			if (flag)
			{
				if (Main.npcShop > 0 && !inv[slot].favorited)
				{
					_ = Main.instance.shop[Main.npcShop];
					if (inv[slot].type >= 71 && inv[slot].type <= 74)
					{
						return "";
					}
					return Lang.misc[75].Value;
				}
				if (!inv[slot].favorited)
				{
					return Lang.misc[74].Value;
				}
			}
		}
		return "";
	}

	public static int PickItemMovementAction(Item[] inv, int context, int slot, Item checkItem)
	{
		_ = Main.player[Main.myPlayer];
		int result = -1;
		switch (context)
		{
		case 0:
			result = 0;
			break;
		case 1:
			if (checkItem.type == 0 || checkItem.type == 71 || checkItem.type == 72 || checkItem.type == 73 || checkItem.type == 74)
			{
				result = 0;
			}
			break;
		case 2:
			if (checkItem.FitsAmmoSlot())
			{
				result = 0;
			}
			break;
		case 3:
			result = 0;
			break;
		case 4:
		case 32:
		{
			ChestUI.GetContainerUsageInfo(out var _, out var chestinv);
			if (!ChestUI.IsBlockedFromTransferIntoChest(checkItem, chestinv))
			{
				result = 0;
			}
			break;
		}
		case 5:
			if (checkItem.Prefix(-3) || checkItem.type == 0)
			{
				result = 0;
			}
			break;
		case 6:
			result = 0;
			break;
		case 7:
			if (checkItem.material || checkItem.type == 0)
			{
				result = 0;
			}
			break;
		case 8:
			if (checkItem.type == 0 || (checkItem.headSlot > -1 && slot == 0) || (checkItem.bodySlot > -1 && slot == 1) || (checkItem.legSlot > -1 && slot == 2))
			{
				result = 1;
			}
			break;
		case 23:
			if (checkItem.type == 0 || (checkItem.headSlot > 0 && slot == 0) || (checkItem.bodySlot > 0 && slot == 1) || (checkItem.legSlot > 0 && slot == 2))
			{
				result = 1;
			}
			break;
		case 26:
			if (checkItem.type == 0 || checkItem.headSlot > 0)
			{
				result = 1;
			}
			break;
		case 9:
			if (checkItem.type == 0 || (checkItem.headSlot > -1 && slot == 10) || (checkItem.bodySlot > -1 && slot == 11) || (checkItem.legSlot > -1 && slot == 12))
			{
				result = 1;
			}
			break;
		case 10:
			if (checkItem.type == 0 || (checkItem.accessory && !AccCheck_ForLocalPlayer(Main.LocalPlayer.armor.Concat(AccessorySlotLoader.ModSlotPlayer(Main.LocalPlayer).exAccessorySlot).ToArray(), checkItem, slot)))
			{
				result = 1;
			}
			break;
		case -11:
		case -10:
			if (checkItem.type == 0 || (checkItem.accessory && LoaderManager.Get<AccessorySlotLoader>().ModSlotCheck(checkItem, slot, context)))
			{
				result = 1;
			}
			break;
		case 24:
			if (checkItem.type == 0 || (checkItem.accessory && !AccCheck(inv, checkItem, slot)))
			{
				result = 1;
			}
			break;
		case 11:
			if (checkItem.type == 0 || (checkItem.accessory && !AccCheck_ForLocalPlayer(Main.LocalPlayer.armor.Concat(AccessorySlotLoader.ModSlotPlayer(Main.LocalPlayer).exAccessorySlot).ToArray(), checkItem, slot)))
			{
				result = 1;
			}
			break;
		case -12:
		case 12:
		case 25:
		case 27:
		case 33:
			result = 2;
			break;
		case 15:
			if (checkItem.type == 0 && inv[slot].type > 0)
			{
				result = 3;
			}
			else if (checkItem.type == inv[slot].type && checkItem.type > 0 && checkItem.stack < checkItem.maxStack && inv[slot].stack > 0)
			{
				result = 3;
			}
			else if (inv[slot].type == 0 && checkItem.type > 0 && (checkItem.type < 71 || checkItem.type > 74))
			{
				result = 4;
			}
			break;
		case 16:
			if (checkItem.type == 0 || Main.projHook[checkItem.shoot])
			{
				result = 1;
			}
			break;
		case 17:
			if (checkItem.type == 0 || (checkItem.mountType != -1 && !MountID.Sets.Cart[checkItem.mountType]))
			{
				result = 1;
			}
			break;
		case 19:
			if (checkItem.type == 0 || (checkItem.buffType > 0 && Main.vanityPet[checkItem.buffType] && !Main.lightPet[checkItem.buffType]))
			{
				result = 1;
			}
			break;
		case 18:
			if (checkItem.type == 0 || (checkItem.mountType != -1 && MountID.Sets.Cart[checkItem.mountType]))
			{
				result = 1;
			}
			break;
		case 20:
			if (checkItem.type == 0 || (checkItem.buffType > 0 && Main.lightPet[checkItem.buffType]))
			{
				result = 1;
			}
			break;
		case 29:
			if (checkItem.type == 0 && inv[slot].type > 0)
			{
				result = 5;
			}
			break;
		}
		if (context == 30)
		{
			result = 0;
		}
		return result;
	}

	public static void RightClick(ref Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		RightClick(singleSlotArray, context);
		inv = singleSlotArray[0];
	}

	public static void RightClick(Item[] inv, int context = 0, int slot = 0)
	{
		Player player = Main.player[Main.myPlayer];
		inv[slot].newAndShiny = false;
		if (player.itemAnimation > 0)
		{
			return;
		}
		if (context == 15)
		{
			HandleShopSlot(inv, slot, rightClickIsValid: true, leftClickIsValid: false);
		}
		else
		{
			if (!Main.mouseRight)
			{
				return;
			}
			if (context == 0 && Main.mouseRightRelease)
			{
				TryItemSwap(inv[slot]);
			}
			if (context == 0 && ItemLoader.CanRightClick(inv[slot]))
			{
				if (Main.mouseRightRelease)
				{
					if (Main.ItemDropsDB.GetRulesForItemID(inv[slot].type).Any())
					{
						TryOpenContainer(inv[slot], player);
					}
					else
					{
						ItemLoader.RightClick(inv[slot], player);
					}
				}
				return;
			}
			switch (Math.Abs(context))
			{
			case 9:
			case 11:
				if (Main.mouseRightRelease)
				{
					SwapVanityEquip(inv, context, slot, player);
				}
				break;
			case 12:
			case 25:
			case 27:
			case 33:
				if (Main.mouseRightRelease)
				{
					TryPickupDyeToCursor(context, inv, slot, player);
				}
				break;
			case 0:
			case 3:
			case 4:
			case 32:
				if (inv[slot].maxStack == 1)
				{
					if (Main.mouseRightRelease)
					{
						SwapEquip(inv, context, slot);
					}
					break;
				}
				goto default;
			default:
			{
				if (Main.stackSplit > 1)
				{
					break;
				}
				bool flag = true;
				bool flag2 = inv[slot].maxStack <= 1 && inv[slot].stack <= 1;
				if (context == 0 && flag2)
				{
					flag = false;
				}
				if (context == 3 && flag2)
				{
					flag = false;
				}
				if (context == 4 && flag2)
				{
					flag = false;
				}
				if (context == 32 && flag2)
				{
					flag = false;
				}
				if (!flag)
				{
					break;
				}
				int num = Main.superFastStack + 1;
				for (int i = 0; i < num; i++)
				{
					if (((Main.mouseItem.IsTheSameAs(inv[slot]) && ItemLoader.CanStack(Main.mouseItem, inv[slot])) || Main.mouseItem.type == 0) && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
					{
						PickupItemIntoMouse(inv, context, slot, player);
						SoundEngine.PlaySound(12);
						RefreshStackSplitCooldown();
					}
				}
				break;
			}
			}
		}
	}

	public static void PickupItemIntoMouse(Item[] inv, int context, int slot, Player player)
	{
		if (Main.mouseItem.type == 0)
		{
			if (context == 29)
			{
				Main.mouseItem = inv[slot].Clone();
				Main.mouseItem.OnCreated(new JourneyDuplicationItemCreationContext());
			}
			else
			{
				Main.mouseItem = ItemLoader.TransferWithLimit(inv[slot], 1);
			}
			AnnounceTransfer(new ItemTransferInfo(inv[slot], context, 21));
		}
		else
		{
			ItemLoader.StackItems(Main.mouseItem, inv[slot], out var _, context == 29, 1);
		}
		if (inv[slot].stack <= 0)
		{
			inv[slot] = new Item();
		}
		Recipe.FindRecipes();
		if (context == 3 && Main.netMode == 1)
		{
			NetMessage.SendData(32, -1, -1, null, player.chest, slot);
		}
		if ((context == 23 || context == 24) && Main.netMode == 1)
		{
			NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
		}
		if (context == 25 && Main.netMode == 1)
		{
			NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
		}
		if (context == 26 && Main.netMode == 1)
		{
			NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot);
		}
		if (context == 27 && Main.netMode == 1)
		{
			NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
		}
	}

	public static void RefreshStackSplitCooldown()
	{
		if (Main.stackSplit == 0)
		{
			Main.stackSplit = 30;
		}
		else
		{
			Main.stackSplit = Main.stackDelay;
		}
	}

	private static void TryOpenContainer(Item item, Player player)
	{
		if (ItemID.Sets.BossBag[item.type])
		{
			player.OpenBossBag(item.type);
		}
		else if (ItemID.Sets.IsFishingCrate[item.type])
		{
			player.OpenFishingCrate(item.type);
		}
		else if (item.type == 3093)
		{
			player.OpenHerbBag(3093);
		}
		else if (item.type == 4345)
		{
			player.OpenCanofWorms(item.type);
		}
		else if (item.type == 4410)
		{
			player.OpenOyster(item.type);
		}
		else if (item.type == 1774)
		{
			player.OpenGoodieBag(1774);
		}
		else if (item.type == 3085)
		{
			if (!player.ConsumeItem(327, reverseOrder: false, includeVoidBag: true))
			{
				return;
			}
			player.OpenLockBox(3085);
		}
		else if (item.type == 4879)
		{
			if (!player.HasItemInInventoryOrOpenVoidBag(329))
			{
				return;
			}
			player.OpenShadowLockbox(4879);
		}
		else if (item.type == 1869)
		{
			player.OpenPresent(1869);
		}
		else if (item.type == 599 && item.type == 600 && item.type == 601)
		{
			player.OpenLegacyPresent(item.type);
		}
		else
		{
			player.DropFromItem(item.type);
		}
		ItemLoader.RightClickCallHooks(item, player);
		if (ItemLoader.ConsumeItem(item, player))
		{
			item.stack--;
		}
		if (item.stack == 0)
		{
			item.SetDefaults();
		}
		SoundEngine.PlaySound(7);
		Main.stackSplit = 30;
		Main.mouseRightRelease = false;
		Recipe.FindRecipes();
	}

	private static void SwapVanityEquip(Item[] inv, int context, int slot, Player player)
	{
		int tSlot = slot - inv.Length / 2;
		if (Main.npcShop > 0 || ((inv[slot].type <= 0 || inv[slot].stack <= 0) && (inv[tSlot].type <= 0 || inv[tSlot].stack <= 0)))
		{
			return;
		}
		Item item = inv[tSlot];
		bool flag = context != 11 || item.FitsAccessoryVanitySlot || item.IsAir;
		if (flag && Math.Abs(context) == 11)
		{
			Item[] accessories = AccessorySlotLoader.ModSlotPlayer(player).exAccessorySlot;
			for (int invNum = 0; invNum < 2; invNum++)
			{
				Item[] checkInv = player.armor;
				int startIndex = 3;
				if (invNum == 1)
				{
					checkInv = accessories;
					startIndex = 0;
				}
				for (int i = startIndex; i < checkInv.Length / 2; i++)
				{
					if ((context != 11 || invNum != 0 || i != tSlot) && (context != -11 || invNum != 1 || i != tSlot) && ((inv[slot].wingSlot > 0 && checkInv[i].wingSlot > 0) || !ItemLoader.CanAccessoryBeEquippedWith(inv[slot], checkInv[i])))
					{
						flag = false;
					}
				}
			}
		}
		if (!flag || !ItemLoader.CanEquipAccessory(inv[slot], tSlot, context < 0))
		{
			return;
		}
		Utils.Swap(ref inv[slot], ref inv[tSlot]);
		SoundEngine.PlaySound(7);
		Recipe.FindRecipes();
		if (inv[slot].stack > 0)
		{
			switch (context)
			{
			case 0:
				AchievementsHelper.NotifyItemPickup(player, inv[slot]);
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 16:
			case 17:
			case 25:
			case 27:
			case 33:
				AchievementsHelper.HandleOnEquip(player, inv[slot], context);
				break;
			}
		}
	}

	private static void TryPickupDyeToCursor(int context, Item[] inv, int slot, Player player)
	{
		bool flag = false;
		if (!flag && ((Main.mouseItem.stack < Main.mouseItem.maxStack && Main.mouseItem.type > 0 && ItemLoader.CanStack(Main.mouseItem, inv[slot])) || Main.mouseItem.IsAir) && inv[slot].type > 0 && (Main.mouseItem.type == inv[slot].type || Main.mouseItem.IsAir))
		{
			flag = true;
			if (Main.mouseItem.IsAir)
			{
				Main.mouseItem = inv[slot].Clone();
			}
			else
			{
				ItemLoader.StackItems(Main.mouseItem, inv[slot], out var _);
			}
			inv[slot].SetDefaults();
			SoundEngine.PlaySound(7);
		}
		if (flag)
		{
			if (context == 25 && Main.netMode == 1)
			{
				NetMessage.SendData(121, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
			}
			if (context == 27 && Main.netMode == 1)
			{
				NetMessage.SendData(124, -1, -1, null, Main.myPlayer, player.tileEntityAnchor.interactEntityID, slot, 1f);
			}
		}
	}

	private static void TryItemSwap(Item item)
	{
		int type = item.type;
		switch (type)
		{
		case 4131:
		case 5325:
			item.ChangeItemType((item.type == 5325) ? 4131 : 5325);
			AfterItemSwap(type, item.type);
			break;
		case 5059:
		case 5060:
			item.ChangeItemType((item.type == 5059) ? 5060 : 5059);
			AfterItemSwap(type, item.type);
			break;
		case 5324:
			item.ChangeItemType(5329);
			AfterItemSwap(type, item.type);
			break;
		case 5329:
			item.ChangeItemType(5330);
			AfterItemSwap(type, item.type);
			break;
		case 5330:
			item.ChangeItemType(5324);
			AfterItemSwap(type, item.type);
			break;
		case 4346:
		case 5391:
			item.ChangeItemType((item.type == 4346) ? 5391 : 4346);
			AfterItemSwap(type, item.type);
			break;
		case 5323:
		case 5455:
			item.ChangeItemType((item.type == 5323) ? 5455 : 5323);
			AfterItemSwap(type, item.type);
			break;
		case 4767:
		case 5453:
			item.ChangeItemType((item.type == 4767) ? 5453 : 4767);
			AfterItemSwap(type, item.type);
			break;
		case 5309:
		case 5454:
			item.ChangeItemType((item.type == 5309) ? 5454 : 5309);
			AfterItemSwap(type, item.type);
			break;
		case 5358:
			item.ChangeItemType(5360);
			AfterItemSwap(type, item.type);
			break;
		case 5360:
			item.ChangeItemType(5361);
			AfterItemSwap(type, item.type);
			break;
		case 5361:
			item.ChangeItemType(5359);
			AfterItemSwap(type, item.type);
			break;
		case 5359:
			item.ChangeItemType(5358);
			AfterItemSwap(type, item.type);
			break;
		case 5437:
			item.ChangeItemType(5358);
			AfterItemSwap(type, item.type);
			break;
		}
	}

	private static void AfterItemSwap(int oldType, int newType)
	{
		if (newType == 5324 || newType == 5329 || newType == 5330 || newType == 4346 || newType == 5391 || newType == 5358 || newType == 5361 || newType == 5360 || newType == 5359)
		{
			SoundEngine.PlaySound(22);
		}
		else
		{
			SoundEngine.PlaySound(7);
		}
		Main.stackSplit = 30;
		Main.mouseRightRelease = false;
		Recipe.FindRecipes();
	}

	private static void HandleShopSlot(Item[] inv, int slot, bool rightClickIsValid, bool leftClickIsValid)
	{
		if (Main.cursorOverride == 2)
		{
			return;
		}
		_ = Main.instance.shop[Main.npcShop];
		bool flag = (Main.mouseRight && rightClickIsValid) || (Main.mouseLeft && leftClickIsValid);
		if (!(Main.stackSplit <= 1 && flag) || inv[slot].type <= 0 || ((!Main.mouseItem.IsTheSameAs(inv[slot]) || !ItemLoader.CanStack(Main.mouseItem, inv[slot])) && Main.mouseItem.type != 0))
		{
			return;
		}
		int num = Main.superFastStack + 1;
		Player localPlayer = Main.LocalPlayer;
		for (int i = 0; i < num; i++)
		{
			if (!PlayerLoader.CanBuyItem(localPlayer, localPlayer.TalkNPC, inv, inv[slot]) || (Main.mouseItem.stack >= Main.mouseItem.maxStack && Main.mouseItem.type != 0))
			{
				continue;
			}
			localPlayer.GetItemExpectedPrice(inv[slot], out var _, out var calcForBuying);
			if (!localPlayer.BuyItem(calcForBuying, inv[slot].shopSpecialCurrency) || inv[slot].stack <= 0)
			{
				continue;
			}
			if (i == 0)
			{
				if (inv[slot].value > 0)
				{
					SoundEngine.PlaySound(18);
				}
				else
				{
					SoundEngine.PlaySound(7);
				}
			}
			Item boughtItem = inv[slot].Clone();
			boughtItem.buyOnce = false;
			boughtItem.isAShopItem = false;
			boughtItem.stack = 1;
			boughtItem.OnCreated(new BuyItemCreationContext(Main.mouseItem, localPlayer.TalkNPC));
			int numTransferred;
			if (Main.mouseItem.type == 0)
			{
				Main.mouseItem = boughtItem;
			}
			else
			{
				ItemLoader.StackItems(Main.mouseItem, inv[slot], out numTransferred, infiniteSource: true, 1);
			}
			if (!inv[slot].buyOnce)
			{
				Main.shopSellbackHelper.Add(inv[slot]);
			}
			RefreshStackSplitCooldown();
			if (inv[slot].buyOnce)
			{
				numTransferred = --inv[slot].stack;
				if (numTransferred <= 0)
				{
					inv[slot].SetDefaults();
				}
			}
			AnnounceTransfer(new ItemTransferInfo(Main.mouseItem, 15, 21));
			PlayerLoader.PostBuyItem(localPlayer, localPlayer.TalkNPC, inv, Main.mouseItem);
		}
	}

	public static void Draw(SpriteBatch spriteBatch, ref Item inv, int context, Vector2 position, Color lightColor = default(Color))
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		singleSlotArray[0] = inv;
		Draw(spriteBatch, singleSlotArray, context, 0, position, lightColor);
		inv = singleSlotArray[0];
	}

	public static void Draw(SpriteBatch spriteBatch, Item[] inv, int context, int slot, Vector2 position, Color lightColor = default(Color))
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_1043: Unknown result type (might be due to invalid IL or missing references)
		//IL_1047: Unknown result type (might be due to invalid IL or missing references)
		//IL_104d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1057: Unknown result type (might be due to invalid IL or missing references)
		//IL_105c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1063: Unknown result type (might be due to invalid IL or missing references)
		//IL_1069: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Unknown result type (might be due to invalid IL or missing references)
		//IL_1078: Unknown result type (might be due to invalid IL or missing references)
		//IL_107d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1082: Unknown result type (might be due to invalid IL or missing references)
		//IL_1096: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1179: Unknown result type (might be due to invalid IL or missing references)
		//IL_117b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1182: Unknown result type (might be due to invalid IL or missing references)
		//IL_1187: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_10eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1138: Unknown result type (might be due to invalid IL or missing references)
		//IL_1144: Unknown result type (might be due to invalid IL or missing references)
		//IL_114a: Unknown result type (might be due to invalid IL or missing references)
		//IL_114f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1154: Unknown result type (might be due to invalid IL or missing references)
		//IL_115b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1161: Unknown result type (might be due to invalid IL or missing references)
		//IL_116c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1108: Unknown result type (might be due to invalid IL or missing references)
		//IL_110d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0feb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1004: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Unknown result type (might be due to invalid IL or missing references)
		//IL_1019: Unknown result type (might be due to invalid IL or missing references)
		//IL_101f: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		Item item = inv[slot];
		float inventoryScale = Main.inventoryScale;
		Color color = Color.White;
		if (lightColor != Color.Transparent)
		{
			color = lightColor;
		}
		bool flag = false;
		int num = 0;
		int gamepadPointForSlot = GetGamepadPointForSlot(inv, context, slot);
		if (PlayerInput.UsingGamepadUI)
		{
			flag = UILinkPointNavigator.CurrentPoint == gamepadPointForSlot;
			if (PlayerInput.SettingsForUI.PreventHighlightsForGamepad)
			{
				flag = false;
			}
			if (context == 0)
			{
				num = player.DpadRadial.GetDrawMode(slot);
				if (num > 0 && !PlayerInput.CurrentProfile.UsingDpadHotbar())
				{
					num = 0;
				}
			}
		}
		Texture2D value = TextureAssets.InventoryBack.Value;
		Color color2 = Main.inventoryBack;
		bool flag2 = false;
		bool highlightThingsForMouse = PlayerInput.SettingsForUI.HighlightThingsForMouse;
		if (item.type > 0 && item.stack > 0 && item.favorited && context != 13 && context != 21 && context != 22 && context != 14)
		{
			value = TextureAssets.InventoryBack10.Value;
			if (context == 32)
			{
				value = TextureAssets.InventoryBack19.Value;
			}
		}
		else if (item.type > 0 && item.stack > 0 && Options.HighlightNewItems && item.newAndShiny && context != 13 && context != 21 && context != 14 && context != 22)
		{
			value = TextureAssets.InventoryBack15.Value;
			float num5 = (float)(int)Main.mouseTextColor / 255f;
			num5 = num5 * 0.2f + 0.8f;
			color2 = color2.MultiplyRGBA(new Color(num5, num5, num5));
		}
		else if (!highlightThingsForMouse && item.type > 0 && item.stack > 0 && num != 0 && context != 13 && context != 21 && context != 22)
		{
			value = TextureAssets.InventoryBack15.Value;
			float num6 = (float)(int)Main.mouseTextColor / 255f;
			num6 = num6 * 0.2f + 0.8f;
			color2 = ((num != 1) ? color2.MultiplyRGBA(new Color(num6 / 2f, num6, num6 / 2f)) : color2.MultiplyRGBA(new Color(num6, num6 / 2f, num6 / 2f)));
		}
		else if (context == 0 && slot < 10)
		{
			value = TextureAssets.InventoryBack9.Value;
		}
		else
		{
			switch (context)
			{
			case 28:
				value = TextureAssets.InventoryBack7.Value;
				color2 = Color.White;
				break;
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
				value = TextureAssets.InventoryBack3.Value;
				break;
			case 8:
			case 10:
				value = TextureAssets.InventoryBack13.Value;
				color2 = GetColorByLoadout(slot, context);
				break;
			case 23:
			case 24:
			case 26:
				value = TextureAssets.InventoryBack8.Value;
				break;
			case 9:
			case 11:
				value = TextureAssets.InventoryBack13.Value;
				color2 = GetColorByLoadout(slot, context);
				break;
			case 25:
			case 27:
			case 33:
				value = TextureAssets.InventoryBack12.Value;
				break;
			case 12:
				value = TextureAssets.InventoryBack13.Value;
				color2 = GetColorByLoadout(slot, context);
				break;
			case -12:
			case -11:
			case -10:
				value = LoaderManager.Get<AccessorySlotLoader>().GetBackgroundTexture(slot, context);
				break;
			case 3:
				value = TextureAssets.InventoryBack5.Value;
				break;
			case 4:
			case 32:
				value = TextureAssets.InventoryBack2.Value;
				break;
			case 5:
			case 7:
				value = TextureAssets.InventoryBack4.Value;
				break;
			case 6:
				value = TextureAssets.InventoryBack7.Value;
				break;
			case 13:
			{
				byte b = 200;
				if (slot == Main.player[Main.myPlayer].selectedItem)
				{
					value = TextureAssets.InventoryBack14.Value;
					b = byte.MaxValue;
				}
				((Color)(ref color2))._002Ector((int)b, (int)b, (int)b, (int)b);
				break;
			}
			case 14:
			case 21:
				flag2 = true;
				break;
			case 15:
				value = TextureAssets.InventoryBack6.Value;
				break;
			case 29:
				((Color)(ref color2))._002Ector(53, 69, 127, 255);
				value = TextureAssets.InventoryBack18.Value;
				break;
			case 30:
				flag2 = !flag;
				break;
			case 22:
				value = TextureAssets.InventoryBack4.Value;
				if (DrawGoldBGForCraftingMaterial)
				{
					DrawGoldBGForCraftingMaterial = false;
					value = TextureAssets.InventoryBack14.Value;
					float num7 = (float)(int)((Color)(ref color2)).A / 255f;
					num7 = ((!(num7 < 0.7f)) ? 1f : Utils.GetLerpValue(0f, 0.7f, num7, clamped: true));
					color2 = Color.White * num7;
				}
				break;
			}
		}
		if ((context == 0 || context == 2) && inventoryGlowTime[slot] > 0 && !inv[slot].favorited && !inv[slot].IsAir)
		{
			float num8 = Main.invAlpha / 255f;
			Color val = new Color(63, 65, 151, 255) * num8;
			Color value4 = Main.hslToRgb(inventoryGlowHue[slot], 1f, 0.5f) * num8;
			float num9 = (float)inventoryGlowTime[slot] / 300f;
			num9 *= num9;
			color2 = Color.Lerp(val, value4, num9 / 2f);
			value = TextureAssets.InventoryBack13.Value;
		}
		if ((context == 4 || context == 32 || context == 3) && inventoryGlowTimeChest[slot] > 0 && !inv[slot].favorited && !inv[slot].IsAir)
		{
			float num10 = Main.invAlpha / 255f;
			Color value5 = new Color(130, 62, 102, 255) * num10;
			if (context == 3)
			{
				value5 = new Color(104, 52, 52, 255) * num10;
			}
			Color value6 = Main.hslToRgb(inventoryGlowHueChest[slot], 1f, 0.5f) * num10;
			float num11 = (float)inventoryGlowTimeChest[slot] / 300f;
			num11 *= num11;
			color2 = Color.Lerp(value5, value6, num11 / 2f);
			value = TextureAssets.InventoryBack13.Value;
		}
		if (flag)
		{
			value = TextureAssets.InventoryBack14.Value;
			color2 = Color.White;
			if (item.favorited)
			{
				value = TextureAssets.InventoryBack17.Value;
			}
		}
		if (context == 28 && Main.MouseScreen.Between(position, position + value.Size() * inventoryScale) && !player.mouseInterface)
		{
			value = TextureAssets.InventoryBack14.Value;
			color2 = Color.White;
		}
		if (!flag2)
		{
			spriteBatch.Draw(value, position, (Rectangle?)null, color2, 0f, default(Vector2), inventoryScale, (SpriteEffects)0, 0f);
		}
		int num12 = -1;
		switch (context)
		{
		case 8:
		case 23:
			if (slot == 0)
			{
				num12 = 0;
			}
			if (slot == 1)
			{
				num12 = 6;
			}
			if (slot == 2)
			{
				num12 = 12;
			}
			break;
		case 26:
			num12 = 0;
			break;
		case 9:
			if (slot == 10)
			{
				num12 = 3;
			}
			if (slot == 11)
			{
				num12 = 9;
			}
			if (slot == 12)
			{
				num12 = 15;
			}
			break;
		case 10:
		case 24:
			num12 = 11;
			break;
		case 11:
			num12 = 2;
			break;
		case 12:
		case 25:
		case 27:
		case 33:
			num12 = 1;
			break;
		case -10:
			num12 = 11;
			break;
		case -11:
			num12 = 2;
			break;
		case -12:
			num12 = 1;
			break;
		case 16:
			num12 = 4;
			break;
		case 17:
			num12 = 13;
			break;
		case 19:
			num12 = 10;
			break;
		case 18:
			num12 = 7;
			break;
		case 20:
			num12 = 17;
			break;
		}
		if ((item.type <= 0 || item.stack <= 0) && num12 != -1)
		{
			Texture2D value7 = TextureAssets.Extra[54].Value;
			Rectangle rectangle = value7.Frame(3, 6, num12 % 3, num12 / 3);
			rectangle.Width -= 2;
			rectangle.Height -= 2;
			if (context == -10 || context == -11 || context == -12)
			{
				LoaderManager.Get<AccessorySlotLoader>().DrawSlotTexture(value7, position + value.Size() / 2f * inventoryScale, rectangle, Color.White * 0.35f, 0f, rectangle.Size() / 2f, inventoryScale, (SpriteEffects)0, 0f, slot, context);
			}
			else
			{
				spriteBatch.Draw(value7, position + value.Size() / 2f * inventoryScale, (Rectangle?)rectangle, Color.White * 0.35f, 0f, rectangle.Size() / 2f, inventoryScale, (SpriteEffects)0, 0f);
			}
		}
		Vector2 vector = value.Size() * inventoryScale;
		if (item.type > 0 && item.stack > 0)
		{
			float scale = DrawItemIcon(item, context, spriteBatch, position + vector / 2f, inventoryScale, 32f, color);
			if (ItemID.Sets.TrapSigned[item.type])
			{
				spriteBatch.Draw(TextureAssets.Wire.Value, position + new Vector2(40f, 40f) * inventoryScale, (Rectangle?)new Rectangle(4, 58, 8, 8), color, 0f, new Vector2(4f), 1f, (SpriteEffects)0, 0f);
			}
			if (ItemID.Sets.DrawUnsafeIndicator[item.type])
			{
				Vector2 vector2 = new Vector2(-4f, -4f) * inventoryScale;
				Texture2D value8 = TextureAssets.Extra[258].Value;
				Rectangle rectangle2 = value8.Frame();
				spriteBatch.Draw(value8, position + vector2 + new Vector2(40f, 40f) * inventoryScale, (Rectangle?)rectangle2, color, 0f, rectangle2.Size() / 2f, 1f, (SpriteEffects)0, 0f);
			}
			if (item.type == 5324 || item.type == 5329 || item.type == 5330)
			{
				Vector2 vector3 = new Vector2(2f, -6f) * inventoryScale;
				switch (item.type)
				{
				case 5324:
				{
					Texture2D value2 = TextureAssets.Extra[257].Value;
					Rectangle rectangle5 = value2.Frame(3, 1, 2);
					spriteBatch.Draw(value2, position + vector3 + new Vector2(40f, 40f) * inventoryScale, (Rectangle?)rectangle5, color, 0f, rectangle5.Size() / 2f, 1f, (SpriteEffects)0, 0f);
					break;
				}
				case 5329:
				{
					Texture2D value10 = TextureAssets.Extra[257].Value;
					Rectangle rectangle4 = value10.Frame(3, 1, 1);
					spriteBatch.Draw(value10, position + vector3 + new Vector2(40f, 40f) * inventoryScale, (Rectangle?)rectangle4, color, 0f, rectangle4.Size() / 2f, 1f, (SpriteEffects)0, 0f);
					break;
				}
				case 5330:
				{
					Texture2D value9 = TextureAssets.Extra[257].Value;
					Rectangle rectangle3 = value9.Frame(3);
					spriteBatch.Draw(value9, position + vector3 + new Vector2(40f, 40f) * inventoryScale, (Rectangle?)rectangle3, color, 0f, rectangle3.Size() / 2f, 1f, (SpriteEffects)0, 0f);
					break;
				}
				}
			}
			if (item.stack > 1)
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, item.stack.ToString(), position + new Vector2(10f, 26f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
			}
			int num2 = -1;
			if (context == 13)
			{
				if (item.DD2Summon)
				{
					for (int i = 0; i < 58; i++)
					{
						if (inv[i].type == 3822)
						{
							num2 += inv[i].stack;
						}
					}
					if (num2 >= 0)
					{
						num2++;
					}
				}
				if (item.useAmmo > 0)
				{
					_ = item.useAmmo;
					num2 = 0;
					for (int j = 0; j < 58; j++)
					{
						if (inv[j].stack > 0 && ItemLoader.CanChooseAmmo(item, inv[j], player))
						{
							num2 += inv[j].stack;
						}
					}
				}
				if (item.fishingPole > 0)
				{
					num2 = 0;
					for (int k = 0; k < 58; k++)
					{
						if (inv[k].bait > 0)
						{
							num2 += inv[k].stack;
						}
					}
				}
				if (item.tileWand > 0)
				{
					int tileWand = item.tileWand;
					num2 = 0;
					for (int l = 0; l < 58; l++)
					{
						if (inv[l].type == tileWand)
						{
							num2 += inv[l].stack;
						}
					}
				}
				if (item.type == 509 || item.type == 851 || item.type == 850 || item.type == 3612 || item.type == 3625 || item.type == 3611)
				{
					num2 = 0;
					for (int m = 0; m < 58; m++)
					{
						if (inv[m].type == 530)
						{
							num2 += inv[m].stack;
						}
					}
				}
			}
			if (num2 != -1)
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, num2.ToString(), position + new Vector2(8f, 30f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale * 0.8f), -1f, inventoryScale);
			}
			if (context == 13)
			{
				string text = string.Concat(slot + 1);
				if (text == "10")
				{
					text = "0";
				}
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, position + new Vector2(8f, 4f) * inventoryScale, color, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
			}
			if (context == 13 && item.potion)
			{
				Vector2 position2 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.Value.Size() * inventoryScale / 2f;
				Color color3 = item.GetAlpha(color) * ((float)player.potionDelay / (float)player.potionDelayTime);
				spriteBatch.Draw(TextureAssets.Cd.Value, position2, (Rectangle?)null, color3, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
			}
			if ((Math.Abs(context) == 10 || context == 18) && ((item.expertOnly && !Main.expertMode) || (item.masterOnly && !Main.masterMode)))
			{
				Vector2 position3 = position + value.Size() * inventoryScale / 2f - TextureAssets.Cd.Value.Size() * inventoryScale / 2f;
				Color white = Color.White;
				spriteBatch.Draw(TextureAssets.Cd.Value, position3, (Rectangle?)null, white, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
			}
		}
		else if (context == 6)
		{
			Texture2D value3 = TextureAssets.Trash.Value;
			Vector2 position4 = position + value.Size() * inventoryScale / 2f - value3.Size() * inventoryScale / 2f;
			spriteBatch.Draw(value3, position4, (Rectangle?)null, new Color(100, 100, 100, 100), 0f, default(Vector2), inventoryScale, (SpriteEffects)0, 0f);
		}
		if (context == 0 && slot < 10)
		{
			float num3 = inventoryScale;
			string text2 = string.Concat(slot + 1);
			if (text2 == "10")
			{
				text2 = "0";
			}
			Color baseColor = Main.inventoryBack;
			int num4 = 0;
			if (Main.player[Main.myPlayer].selectedItem == slot)
			{
				baseColor = Color.White;
				((Color)(ref baseColor)).A = 200;
				num4 -= 2;
				num3 *= 1.4f;
			}
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text2, position + new Vector2(6f, (float)(4 + num4)) * inventoryScale, baseColor, 0f, Vector2.Zero, new Vector2(inventoryScale), -1f, inventoryScale);
		}
		if (gamepadPointForSlot != -1)
		{
			UILinkPointNavigator.SetPosition(gamepadPointForSlot, position + vector * 0.75f);
		}
	}

	public static Color GetColorByLoadout(int slot, int context)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Color color = Color.White;
		if (TryGetSlotColor(Main.LocalPlayer.CurrentLoadoutIndex, context, out var color2))
		{
			color = color2;
		}
		Color val = new Color(((Color)(ref color)).ToVector4() * ((Color)(ref Main.inventoryBack)).ToVector4());
		float num = Utils.Remap((float)(Main.timeForVisualEffects - _lastTimeForVisualEffectsThatLoadoutWasChanged), 0f, 30f, 0.5f, 0f);
		return Color.Lerp(val, Color.White, num * num * num);
	}

	public static void RecordLoadoutChange()
	{
		_lastTimeForVisualEffectsThatLoadoutWasChanged = Main.timeForVisualEffects;
	}

	public static bool TryGetSlotColor(int loadoutIndex, int context, out Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		color = default(Color);
		if (loadoutIndex < 0 || loadoutIndex >= 3)
		{
			return false;
		}
		int num = -1;
		switch (context)
		{
		case 8:
		case 10:
			num = 0;
			break;
		case 9:
		case 11:
			num = 1;
			break;
		case 12:
			num = 2;
			break;
		}
		if (num == -1)
		{
			return false;
		}
		color = LoadoutSlotColors[loadoutIndex, num];
		return true;
	}

	public static float ShiftHueByLoadout(float hue, int loadoutIndex)
	{
		return (hue + (float)loadoutIndex / 8f) % 1f;
	}

	public static Color GetLoadoutColor(int loadoutIndex)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return Main.hslToRgb(ShiftHueByLoadout(0.41f, loadoutIndex), 0.7f, 0.5f);
	}

	public static float DrawItemIcon(Item item, int context, SpriteBatch spriteBatch, Vector2 screenPositionForItemCenter, float scale, float sizeLimit, Color environmentColor)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		int num = item.type;
		if ((uint)(num - 5358) <= 3u && context == 31)
		{
			num = 5437;
		}
		Main.instance.LoadItem(num);
		Texture2D value = TextureAssets.Item[num].Value;
		Rectangle frame = ((Main.itemAnimations[num] == null) ? value.Frame() : Main.itemAnimations[num].GetFrame(value));
		DrawItem_GetColorAndScale(item, scale, ref environmentColor, sizeLimit, ref frame, out var itemLight, out var finalDrawScale);
		SpriteEffects effects = (SpriteEffects)0;
		Vector2 origin = frame.Size() / 2f;
		if (ItemLoader.PreDrawInInventory(item, spriteBatch, screenPositionForItemCenter, frame, item.GetAlpha(itemLight), item.GetColor(environmentColor), origin, finalDrawScale))
		{
			spriteBatch.Draw(value, screenPositionForItemCenter, (Rectangle?)frame, item.GetAlpha(itemLight), 0f, origin, finalDrawScale, effects, 0f);
			if (item.color != Color.Transparent)
			{
				Color newColor = environmentColor;
				if (context == 13)
				{
					((Color)(ref newColor)).A = byte.MaxValue;
				}
				spriteBatch.Draw(value, screenPositionForItemCenter, (Rectangle?)frame, item.GetColor(newColor), 0f, origin, finalDrawScale, effects, 0f);
			}
			switch (num)
			{
			case 5140:
			case 5141:
			case 5142:
			case 5143:
			case 5144:
			case 5145:
			{
				Texture2D value3 = TextureAssets.GlowMask[item.glowMask].Value;
				Color white = Color.White;
				spriteBatch.Draw(value3, screenPositionForItemCenter, (Rectangle?)frame, white, 0f, origin, finalDrawScale, effects, 0f);
				break;
			}
			case 5146:
			{
				Texture2D value2 = TextureAssets.GlowMask[324].Value;
				Color val = default(Color);
				((Color)(ref val))._002Ector(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				spriteBatch.Draw(value2, screenPositionForItemCenter, (Rectangle?)frame, val, 0f, origin, finalDrawScale, effects, 0f);
				break;
			}
			}
		}
		ItemLoader.PostDrawInInventory(item, spriteBatch, screenPositionForItemCenter, frame, item.GetAlpha(itemLight), item.GetColor(environmentColor), origin, finalDrawScale);
		return finalDrawScale;
	}

	public static void DrawItem_GetColorAndScale(Item item, float scale, ref Color currentWhite, float sizeLimit, ref Rectangle frame, out Color itemLight, out float finalDrawScale)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		itemLight = currentWhite;
		float scale2 = 1f;
		GetItemLight(ref itemLight, ref scale2, item);
		float num = 1f;
		if ((float)frame.Width > sizeLimit || (float)frame.Height > sizeLimit)
		{
			num = ((frame.Width <= frame.Height) ? (sizeLimit / (float)frame.Height) : (sizeLimit / (float)frame.Width));
		}
		finalDrawScale = scale * num * scale2;
	}

	private static int GetGamepadPointForSlot(Item[] inv, int context, int slot)
	{
		Player localPlayer = Main.LocalPlayer;
		int result = -1;
		switch (context)
		{
		case 0:
		case 1:
		case 2:
			result = slot;
			break;
		case 8:
		case 9:
		case 10:
		case 11:
		{
			int num2 = slot;
			if (num2 % 10 == 9 && !localPlayer.CanDemonHeartAccessoryBeShown())
			{
				num2--;
			}
			result = 100 + num2;
			break;
		}
		case 12:
			if (inv == localPlayer.dye)
			{
				int num = slot;
				if (num % 10 == 9 && !localPlayer.CanDemonHeartAccessoryBeShown())
				{
					num--;
				}
				result = 120 + num;
			}
			break;
		case 33:
			if (inv == localPlayer.miscDyes)
			{
				result = 185 + slot;
			}
			break;
		case 19:
			result = 180;
			break;
		case 20:
			result = 181;
			break;
		case 18:
			result = 182;
			break;
		case 17:
			result = 183;
			break;
		case 16:
			result = 184;
			break;
		case 3:
		case 4:
		case 32:
			result = 400 + slot;
			break;
		case 15:
			result = 2700 + slot;
			break;
		case 6:
			result = 300;
			break;
		case 22:
			if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig != -1)
			{
				result = 700 + UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeBig;
			}
			if (UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall != -1)
			{
				result = 1500 + UILinkPointNavigator.Shortcuts.CRAFT_CurrentRecipeSmall + 1;
			}
			break;
		case 7:
			result = 1500;
			break;
		case 5:
			result = 303;
			break;
		case 23:
			result = 5100 + slot;
			break;
		case 24:
			result = 5100 + slot;
			break;
		case 25:
			result = 5108 + slot;
			break;
		case 26:
			result = 5000 + slot;
			break;
		case 27:
			result = 5002 + slot;
			break;
		case 29:
			result = 3000 + slot;
			if (UILinkPointNavigator.Shortcuts.CREATIVE_ItemSlotShouldHighlightAsSelected)
			{
				result = UILinkPointNavigator.CurrentPoint;
			}
			break;
		case 30:
			result = 15000 + slot;
			break;
		}
		return result;
	}

	public static void MouseHover(int context = 0)
	{
		singleSlotArray[0] = Main.HoverItem;
		MouseHover(singleSlotArray, context);
	}

	public static void MouseHover(ref Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		MouseHover(singleSlotArray, context);
		inv = singleSlotArray[0];
	}

	public static void MouseHover(Item[] inv, int context = 0, int slot = 0)
	{
		if (context == 6 && Main.hoverItemName == null)
		{
			Main.hoverItemName = Lang.inter[3].Value;
		}
		if (inv[slot].type > 0 && inv[slot].stack > 0)
		{
			_customCurrencyForSavings = inv[slot].shopSpecialCurrency;
			Main.hoverItemName = inv[slot].Name;
			if (inv[slot].stack > 1)
			{
				Main.hoverItemName = Main.hoverItemName + " (" + inv[slot].stack + ")";
			}
			Main.HoverItem = inv[slot].Clone();
			Main.HoverItem.tooltipContext = context;
			if (context == 8 && slot <= 2)
			{
				Main.HoverItem.wornArmor = true;
				return;
			}
			switch (context)
			{
			case 9:
			case 11:
				Main.HoverItem.social = true;
				break;
			case 15:
				Main.HoverItem.buy = true;
				break;
			}
			return;
		}
		if (context == 10 || context == 11 || context == 24)
		{
			Main.hoverItemName = Lang.inter[9].Value;
		}
		if (context == 11)
		{
			Main.hoverItemName = Lang.inter[11].Value + " " + Main.hoverItemName;
		}
		if (context == 8 || context == 9 || context == 23 || context == 26)
		{
			if (slot == 0 || slot == 10 || context == 26)
			{
				Main.hoverItemName = Lang.inter[12].Value;
			}
			else if (slot == 1 || slot == 11)
			{
				Main.hoverItemName = Lang.inter[13].Value;
			}
			else if (slot == 2 || slot == 12)
			{
				Main.hoverItemName = Lang.inter[14].Value;
			}
			else if (slot >= 10)
			{
				Main.hoverItemName = Lang.inter[11].Value + " " + Main.hoverItemName;
			}
		}
		if (context == 12 || context == 25 || context == 27 || context == 33)
		{
			Main.hoverItemName = Lang.inter[57].Value;
		}
		if (context == 16)
		{
			Main.hoverItemName = Lang.inter[90].Value;
		}
		if (context == 17)
		{
			Main.hoverItemName = Lang.inter[91].Value;
		}
		if (context == 19)
		{
			Main.hoverItemName = Lang.inter[92].Value;
		}
		if (context == 18)
		{
			Main.hoverItemName = Lang.inter[93].Value;
		}
		if (context == 20)
		{
			Main.hoverItemName = Lang.inter[94].Value;
		}
	}

	public static void SwapEquip(ref Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		SwapEquip(singleSlotArray, context, 0);
		inv = singleSlotArray[0];
	}

	public static void SwapEquip(Item[] inv, int context, int slot)
	{
		Player player = Main.player[Main.myPlayer];
		if (isEquipLocked(inv[slot].type) || inv[slot].IsAir)
		{
			return;
		}
		bool success;
		if (inv[slot].dye > 0)
		{
			inv[slot] = DyeSwap(inv[slot], out success);
			if (success)
			{
				Main.EquipPageSelected = 0;
				AchievementsHelper.HandleOnEquip(player, inv[slot], 12);
			}
		}
		else if (Main.projHook[inv[slot].shoot])
		{
			inv[slot] = EquipSwap(inv[slot], player.miscEquips, 4, out success);
			if (success)
			{
				Main.EquipPageSelected = 2;
				AchievementsHelper.HandleOnEquip(player, inv[slot], 16);
			}
		}
		else if (inv[slot].mountType != -1 && !MountID.Sets.Cart[inv[slot].mountType])
		{
			inv[slot] = EquipSwap(inv[slot], player.miscEquips, 3, out success);
			if (success)
			{
				Main.EquipPageSelected = 2;
				AchievementsHelper.HandleOnEquip(player, inv[slot], 17);
			}
		}
		else if (inv[slot].mountType != -1 && MountID.Sets.Cart[inv[slot].mountType])
		{
			inv[slot] = EquipSwap(inv[slot], player.miscEquips, 2, out success);
			if (success)
			{
				Main.EquipPageSelected = 2;
			}
		}
		else if (inv[slot].buffType > 0 && Main.lightPet[inv[slot].buffType])
		{
			inv[slot] = EquipSwap(inv[slot], player.miscEquips, 1, out success);
			if (success)
			{
				Main.EquipPageSelected = 2;
			}
		}
		else if (inv[slot].buffType > 0 && Main.vanityPet[inv[slot].buffType])
		{
			inv[slot] = EquipSwap(inv[slot], player.miscEquips, 0, out success);
			if (success)
			{
				Main.EquipPageSelected = 2;
			}
		}
		else
		{
			Item item = inv[slot];
			inv[slot] = ArmorSwap(inv[slot], out success);
			if (success)
			{
				Main.EquipPageSelected = 0;
				AchievementsHelper.HandleOnEquip(player, item, (item.accessory ? 10 : 8) * Math.Sign(context));
			}
		}
		Recipe.FindRecipes();
		if (context == 3 && Main.netMode == 1)
		{
			NetMessage.SendData(32, -1, -1, null, player.chest, slot);
		}
	}

	public static bool Equippable(ref Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		bool result = Equippable(singleSlotArray, context, 0);
		inv = singleSlotArray[0];
		return result;
	}

	public static bool Equippable(Item[] inv, int context, int slot)
	{
		_ = Main.player[Main.myPlayer];
		if (inv[slot].dye > 0 || Main.projHook[inv[slot].shoot] || inv[slot].mountType != -1 || (inv[slot].buffType > 0 && Main.lightPet[inv[slot].buffType]) || (inv[slot].buffType > 0 && Main.vanityPet[inv[slot].buffType]) || inv[slot].headSlot >= 0 || inv[slot].bodySlot >= 0 || inv[slot].legSlot >= 0 || inv[slot].accessory)
		{
			return true;
		}
		return false;
	}

	public static bool IsMiscEquipment(Item item)
	{
		if (item.mountType == -1 && (item.buffType <= 0 || !Main.lightPet[item.buffType]) && (item.buffType <= 0 || !Main.vanityPet[item.buffType]))
		{
			return Main.projHook[item.shoot];
		}
		return true;
	}

	public static bool AccCheck(Item[] itemCollection, Item item, int slot)
	{
		if (isEquipLocked(item.type))
		{
			return true;
		}
		if (slot != -1)
		{
			if (itemCollection[slot].IsTheSameAs(item))
			{
				return false;
			}
			if (itemCollection[slot].wingSlot > 0 && item.wingSlot > 0)
			{
				return false;
			}
		}
		for (int i = 0; i < itemCollection.Length; i++)
		{
			if (slot < 10 && i < 10)
			{
				if (item.wingSlot > 0 && itemCollection[i].wingSlot > 0)
				{
					return true;
				}
				if (slot >= 10 && i >= 10 && item.wingSlot > 0 && itemCollection[i].wingSlot > 0)
				{
					return true;
				}
			}
			if (item.IsTheSameAs(itemCollection[i]))
			{
				return true;
			}
		}
		return false;
	}

	private static Item DyeSwap(Item item, out bool success)
	{
		success = false;
		if (item.dye <= 0)
		{
			return item;
		}
		Player player = Main.player[Main.myPlayer];
		Item item2 = item;
		for (int i = 0; i < 10; i++)
		{
			if (player.dye[i].type == 0)
			{
				dyeSlotCount = i;
				break;
			}
		}
		if (dyeSlotCount >= 10)
		{
			item2 = ModSlotDyeSwap(item, out success);
			if (success)
			{
				return item2;
			}
			dyeSlotCount = 0;
		}
		if (dyeSlotCount < 0)
		{
			dyeSlotCount = 9;
		}
		item2 = player.dye[dyeSlotCount].Clone();
		player.dye[dyeSlotCount] = item.Clone();
		dyeSlotCount++;
		if (dyeSlotCount >= 10)
		{
			accSlotToSwapTo = 0;
		}
		SoundEngine.PlaySound(7);
		Recipe.FindRecipes();
		success = true;
		return item2;
	}

	private static Item ArmorSwap(Item item, out bool success)
	{
		success = false;
		if (item.stack < 1)
		{
			return item;
		}
		if (item.headSlot == -1 && item.bodySlot == -1 && item.legSlot == -1 && !item.accessory)
		{
			return item;
		}
		Player player = Main.player[Main.myPlayer];
		int num = ((item.vanity && !item.accessory) ? 10 : 0);
		item.favorited = false;
		Item result = item;
		if (item.headSlot != -1)
		{
			result = player.armor[num].Clone();
			player.armor[num] = item.Clone();
		}
		else if (item.bodySlot != -1)
		{
			result = player.armor[num + 1].Clone();
			player.armor[num + 1] = item.Clone();
		}
		else if (item.legSlot != -1)
		{
			result = player.armor[num + 2].Clone();
			player.armor[num + 2] = item.Clone();
		}
		else if (item.accessory && !AccessorySwap(player, item, ref result))
		{
			return result;
		}
		SoundEngine.PlaySound(7);
		Recipe.FindRecipes();
		success = true;
		return result;
	}

	private static Item EquipSwap(Item item, Item[] inv, int slot, out bool success)
	{
		success = false;
		_ = Main.player[Main.myPlayer];
		item.favorited = false;
		Item result = inv[slot].Clone();
		inv[slot] = item.Clone();
		SoundEngine.PlaySound(7);
		Recipe.FindRecipes();
		success = true;
		return result;
	}

	public static void DrawMoney(SpriteBatch sb, string text, float shopx, float shopy, int[] coinsArray, bool horizontal = false)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		Utils.DrawBorderStringFourWay(sb, FontAssets.MouseText.Value, text, shopx, shopy + 40f, Color.White * ((float)(int)Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
		if (horizontal)
		{
			Vector2 position = default(Vector2);
			for (int i = 0; i < 4; i++)
			{
				Main.instance.LoadItem(74 - i);
				if (i == 0)
				{
					_ = coinsArray[3 - i];
				}
				((Vector2)(ref position))._002Ector(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).X + (float)(24 * i) + 45f, shopy + 50f);
				sb.Draw(TextureAssets.Item[74 - i].Value, position, (Rectangle?)null, Color.White, 0f, TextureAssets.Item[74 - i].Value.Size() / 2f, 1f, (SpriteEffects)0, 0f);
				Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, coinsArray[3 - i].ToString(), position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
			}
		}
		else
		{
			for (int j = 0; j < 4; j++)
			{
				Main.instance.LoadItem(74 - j);
				int num = ((j == 0 && coinsArray[3 - j] > 99) ? (-6) : 0);
				sb.Draw(TextureAssets.Item[74 - j].Value, new Vector2(shopx + 11f + (float)(24 * j), shopy + 75f), (Rectangle?)null, Color.White, 0f, TextureAssets.Item[74 - j].Value.Size() / 2f, 1f, (SpriteEffects)0, 0f);
				Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, coinsArray[3 - j].ToString(), shopx + (float)(24 * j) + (float)num, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
			}
		}
	}

	public static void DrawSavings(SpriteBatch sb, float shopx, float shopy, bool horizontal = false)
	{
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		if (_customCurrencyForSavings != -1)
		{
			CustomCurrencyManager.DrawSavings(sb, _customCurrencyForSavings, shopx, shopy, horizontal);
			return;
		}
		bool overFlowing;
		long num = Utils.CoinsCount(out overFlowing, player.bank.item);
		long num2 = Utils.CoinsCount(out overFlowing, player.bank2.item);
		long num3 = Utils.CoinsCount(out overFlowing, player.bank3.item);
		long num4 = Utils.CoinsCount(out overFlowing, player.bank4.item);
		long num5 = Utils.CoinsCombineStacks(out overFlowing, num, num2, num3, num4);
		if (num5 > 0)
		{
			Main.GetItemDrawFrame(4076, out var itemTexture, out var itemFrame);
			Main.GetItemDrawFrame(3813, out var itemTexture2, out var itemFrame2);
			Main.GetItemDrawFrame(346, out var itemTexture3, out var itemFrame3);
			Main.GetItemDrawFrame(87, out var itemTexture4, out itemFrame3);
			if (num4 > 0)
			{
				sb.Draw(itemTexture, Utils.CenteredRectangle(new Vector2(shopx + 92f, shopy + 45f), itemFrame.Size() * 0.65f), (Rectangle?)null, Color.White);
			}
			if (num3 > 0)
			{
				sb.Draw(itemTexture2, Utils.CenteredRectangle(new Vector2(shopx + 92f, shopy + 45f), itemFrame2.Size() * 0.65f), (Rectangle?)null, Color.White);
			}
			if (num2 > 0)
			{
				sb.Draw(itemTexture3, Utils.CenteredRectangle(new Vector2(shopx + 80f, shopy + 50f), itemTexture3.Size() * 0.65f), (Rectangle?)null, Color.White);
			}
			if (num > 0)
			{
				sb.Draw(itemTexture4, Utils.CenteredRectangle(new Vector2(shopx + 70f, shopy + 60f), itemTexture4.Size() * 0.65f), (Rectangle?)null, Color.White);
			}
			DrawMoney(sb, Lang.inter[66].Value, shopx, shopy, Utils.CoinsSplit(num5), horizontal);
		}
	}

	public static void GetItemLight(ref Color currentColor, Item item, bool outInTheWorld = false)
	{
		float scale = 1f;
		GetItemLight(ref currentColor, ref scale, item, outInTheWorld);
	}

	public static void GetItemLight(ref Color currentColor, int type, bool outInTheWorld = false)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		float scale = 1f;
		GetItemLight(ref currentColor, ref scale, type, outInTheWorld);
	}

	public static void GetItemLight(ref Color currentColor, ref float scale, Item item, bool outInTheWorld = false)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		GetItemLight(ref currentColor, ref scale, item.type, outInTheWorld);
	}

	public static Color GetItemLight(ref Color currentColor, ref float scale, int type, bool outInTheWorld = false)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		if (type < 0)
		{
			return currentColor;
		}
		if (type == 662 || type == 663 || type == 5444 || type == 5450)
		{
			((Color)(ref currentColor)).R = (byte)Main.DiscoR;
			((Color)(ref currentColor)).G = (byte)Main.DiscoG;
			((Color)(ref currentColor)).B = (byte)Main.DiscoB;
			((Color)(ref currentColor)).A = byte.MaxValue;
		}
		if (type == 5128)
		{
			((Color)(ref currentColor)).R = (byte)Main.DiscoR;
			((Color)(ref currentColor)).G = (byte)Main.DiscoG;
			((Color)(ref currentColor)).B = (byte)Main.DiscoB;
			((Color)(ref currentColor)).A = byte.MaxValue;
		}
		else if (ItemID.Sets.ItemIconPulse[type])
		{
			scale = Main.essScale;
			((Color)(ref currentColor)).R = (byte)((float)(int)((Color)(ref currentColor)).R * scale);
			((Color)(ref currentColor)).G = (byte)((float)(int)((Color)(ref currentColor)).G * scale);
			((Color)(ref currentColor)).B = (byte)((float)(int)((Color)(ref currentColor)).B * scale);
			((Color)(ref currentColor)).A = (byte)((float)(int)((Color)(ref currentColor)).A * scale);
		}
		else if (type == 58 || type == 184 || type == 4143)
		{
			scale = Main.essScale * 0.25f + 0.75f;
			((Color)(ref currentColor)).R = (byte)((float)(int)((Color)(ref currentColor)).R * scale);
			((Color)(ref currentColor)).G = (byte)((float)(int)((Color)(ref currentColor)).G * scale);
			((Color)(ref currentColor)).B = (byte)((float)(int)((Color)(ref currentColor)).B * scale);
			((Color)(ref currentColor)).A = (byte)((float)(int)((Color)(ref currentColor)).A * scale);
		}
		return currentColor;
	}

	public static void DrawRadialCircular(SpriteBatch sb, Vector2 position, Player.SelectionRadial radial, Item[] items)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		CircularRadialOpacity = MathHelper.Clamp(CircularRadialOpacity + ((PlayerInput.UsingGamepad && PlayerInput.Triggers.Current.RadialHotbar) ? 0.25f : (-0.15f)), 0f, 1f);
		if (CircularRadialOpacity == 0f)
		{
			return;
		}
		Texture2D value = TextureAssets.HotbarRadial[2].Value;
		float num = CircularRadialOpacity * 0.9f;
		float num2 = CircularRadialOpacity * 1f;
		float num3 = (float)(int)Main.mouseTextColor / 255f;
		float num4 = 1f - (1f - num3) * (1f - num3);
		num4 *= 0.785f;
		Color color = Color.White * num4 * num;
		value = TextureAssets.HotbarRadial[1].Value;
		float num5 = (float)Math.PI * 2f / (float)radial.RadialCount;
		float num6 = -(float)Math.PI / 2f;
		for (int i = 0; i < radial.RadialCount; i++)
		{
			int num7 = radial.Bindings[i];
			Vector2 vector = Utils.RotatedBy(new Vector2(150f, 0f), num6 + num5 * (float)i) * num2;
			float num8 = 0.85f;
			if (radial.SelectedBinding == i)
			{
				num8 = 1.7f;
			}
			sb.Draw(value, position + vector, (Rectangle?)null, color * num8, 0f, value.Size() / 2f, num2 * num8, (SpriteEffects)0, 0f);
			if (num7 != -1)
			{
				float inventoryScale = Main.inventoryScale;
				Main.inventoryScale = num2 * num8;
				Draw(sb, items, 14, num7, position + vector + new Vector2(-26f * num2 * num8), Color.White);
				Main.inventoryScale = inventoryScale;
			}
		}
	}

	public static void DrawRadialQuicks(SpriteBatch sb, Vector2 position)
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		QuicksRadialOpacity = MathHelper.Clamp(QuicksRadialOpacity + ((PlayerInput.UsingGamepad && PlayerInput.Triggers.Current.RadialQuickbar) ? 0.25f : (-0.15f)), 0f, 1f);
		if (QuicksRadialOpacity == 0f)
		{
			return;
		}
		Player player = Main.player[Main.myPlayer];
		Texture2D value = TextureAssets.HotbarRadial[2].Value;
		Texture2D value2 = TextureAssets.QuicksIcon.Value;
		float num = QuicksRadialOpacity * 0.9f;
		float num2 = QuicksRadialOpacity * 1f;
		float num3 = (float)(int)Main.mouseTextColor / 255f;
		float num4 = 1f - (1f - num3) * (1f - num3);
		num4 *= 0.785f;
		Color color = Color.White * num4 * num;
		float num5 = (float)Math.PI * 2f / (float)player.QuicksRadial.RadialCount;
		float num6 = -(float)Math.PI / 2f;
		Item item = player.QuickHeal_GetItemToUse();
		Item item2 = player.QuickMana_GetItemToUse();
		Item item3 = null;
		Item item4 = null;
		if (item == null)
		{
			item = new Item();
			item.SetDefaults(28);
		}
		if (item2 == null)
		{
			item2 = new Item();
			item2.SetDefaults(110);
		}
		if (item3 == null)
		{
			item3 = new Item();
			item3.SetDefaults(292);
		}
		if (item4 == null)
		{
			item4 = new Item();
			item4.SetDefaults(2428);
		}
		for (int i = 0; i < player.QuicksRadial.RadialCount; i++)
		{
			Item inv = item4;
			if (i == 1)
			{
				inv = item;
			}
			if (i == 2)
			{
				inv = item3;
			}
			if (i == 3)
			{
				inv = item2;
			}
			_ = player.QuicksRadial.Bindings[i];
			Vector2 vector = Utils.RotatedBy(new Vector2(120f, 0f), num6 + num5 * (float)i) * num2;
			float num7 = 0.85f;
			if (player.QuicksRadial.SelectedBinding == i)
			{
				num7 = 1.7f;
			}
			sb.Draw(value, position + vector, (Rectangle?)null, color * num7, 0f, value.Size() / 2f, num2 * num7 * 1.3f, (SpriteEffects)0, 0f);
			float inventoryScale = Main.inventoryScale;
			Main.inventoryScale = num2 * num7;
			Draw(sb, ref inv, 14, position + vector + new Vector2(-26f * num2 * num7), Color.White);
			Main.inventoryScale = inventoryScale;
			sb.Draw(value2, position + vector + new Vector2(34f, 20f) * 0.85f * num2 * num7, (Rectangle?)null, color * num7, 0f, value.Size() / 2f, num2 * num7 * 1.3f, (SpriteEffects)0, 0f);
		}
	}

	public static void DrawRadialDpad(SpriteBatch sb, Vector2 position)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		if (!PlayerInput.UsingGamepad || !PlayerInput.CurrentProfile.UsingDpadHotbar())
		{
			return;
		}
		Player player = Main.player[Main.myPlayer];
		if (player.chest != -1)
		{
			return;
		}
		Texture2D value = TextureAssets.HotbarRadial[0].Value;
		float num = (float)(int)Main.mouseTextColor / 255f;
		float num2 = 1f - (1f - num) * (1f - num);
		num2 *= 0.785f;
		Color color = Color.White * num2;
		sb.Draw(value, position, (Rectangle?)null, color, 0f, value.Size() / 2f, Main.inventoryScale, (SpriteEffects)0, 0f);
		for (int i = 0; i < 4; i++)
		{
			int num3 = player.DpadRadial.Bindings[i];
			if (num3 != -1)
			{
				Draw(sb, player.inventory, 14, num3, position + Utils.RotatedBy(new Vector2((float)(value.Width / 3), 0f), -(float)Math.PI / 2f + (float)Math.PI / 2f * (float)i) + new Vector2(-26f * Main.inventoryScale), Color.White);
			}
		}
	}

	public static string GetGamepadInstructions(ref Item inv, int context = 0)
	{
		singleSlotArray[0] = inv;
		string gamepadInstructions = GetGamepadInstructions(singleSlotArray, context);
		inv = singleSlotArray[0];
		return gamepadInstructions;
	}

	public static bool CanExecuteCommand()
	{
		return PlayerInput.AllowExecutionOfGamepadInstructions;
	}

	public static string GetGamepadInstructions(Item[] inv, int context = 0, int slot = 0)
	{
		Player player = Main.player[Main.myPlayer];
		string s = "";
		if (inv == null || inv[slot] == null || Main.mouseItem == null)
		{
			return s;
		}
		if (context == 0 || context == 1 || context == 2)
		{
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
					{
						s += PlayerInput.BuildCommand(Lang.misc[55].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				else
				{
					if (context == 0 && player.chest == -1 && PlayerInput.AllowExecutionOfGamepadInstructions)
					{
						player.DpadRadial.ChangeBinding(slot);
					}
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					if (inv[slot].maxStack > 1)
					{
						s += PlayerInput.BuildCommand(Lang.misc[55].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				if (inv[slot].maxStack == 1 && Equippable(inv, context, slot))
				{
					s += PlayerInput.BuildCommand(Lang.misc[67].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						SwapEquip(inv, context, slot);
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				s += PlayerInput.BuildCommand(Lang.misc[83].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
				if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
				{
					inv[slot].favorited = !inv[slot].favorited;
					PlayerInput.LockGamepadButtons("SmartCursor");
					PlayerInput.SettingsForUI.TryRevertingToMouseMode();
				}
			}
			else if (Main.mouseItem.type > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
		}
		if (context == 3 || context == 4 || context == 32)
		{
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
					{
						s += PlayerInput.BuildCommand(Lang.misc[55].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					if (inv[slot].maxStack > 1)
					{
						s += PlayerInput.BuildCommand(Lang.misc[55].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				if (inv[slot].maxStack == 1 && Equippable(inv, context, slot))
				{
					s += PlayerInput.BuildCommand(Lang.misc[67].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						SwapEquip(inv, context, slot);
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				if (context == 32)
				{
					s += PlayerInput.BuildCommand(Lang.misc[83].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartCursor"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartCursor)
					{
						inv[slot].favorited = !inv[slot].favorited;
						PlayerInput.LockGamepadButtons("SmartCursor");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
			}
			else if (Main.mouseItem.type > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
		}
		if (context == 15)
		{
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					if (inv[slot].type == Main.mouseItem.type && Main.mouseItem.stack < inv[slot].maxStack && inv[slot].maxStack > 1)
					{
						s += PlayerInput.BuildCommand(Lang.misc[91].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[90].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"], PlayerInput.ProfileGamepadUI.KeyStatus["MouseRight"]);
				}
			}
			else if (Main.mouseItem.type > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[92].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
		}
		if (context == 8 || context == 9 || context == 16 || context == 17 || context == 18 || context == 19 || context == 20)
		{
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					if (Equippable(ref Main.mouseItem, context))
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
				}
				else if (context != 8 || !isEquipLocked(inv[slot].type))
				{
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
				if (context == 8 && slot >= 3)
				{
					bool flag = player.hideVisibleAccessory[slot];
					s += PlayerInput.BuildCommand(Lang.misc[flag ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						player.hideVisibleAccessory[slot] = !player.hideVisibleAccessory[slot];
						SoundEngine.PlaySound(12);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
						}
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				if ((context == 16 || context == 17 || context == 18 || context == 19 || context == 20) && slot < 2)
				{
					bool flag2 = player.hideMisc[slot];
					s += PlayerInput.BuildCommand(Lang.misc[flag2 ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						if (slot == 0)
						{
							player.TogglePet();
						}
						if (slot == 1)
						{
							player.ToggleLight();
						}
						SoundEngine.PlaySound(12);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
						}
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
			}
			else
			{
				if (Main.mouseItem.type > 0 && Equippable(ref Main.mouseItem, context))
				{
					s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
				if (context == 8 && slot >= 3)
				{
					int num = slot;
					if (num % 10 == 8 && !player.CanDemonHeartAccessoryBeShown())
					{
						num++;
					}
					bool flag3 = player.hideVisibleAccessory[num];
					s += PlayerInput.BuildCommand(Lang.misc[flag3 ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						player.hideVisibleAccessory[num] = !player.hideVisibleAccessory[num];
						SoundEngine.PlaySound(12);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
						}
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				if ((context == 16 || context == 17 || context == 18 || context == 19 || context == 20) && slot < 2)
				{
					bool flag4 = player.hideMisc[slot];
					s += PlayerInput.BuildCommand(Lang.misc[flag4 ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
					{
						if (slot == 0)
						{
							player.TogglePet();
						}
						if (slot == 1)
						{
							player.ToggleLight();
						}
						Main.mouseLeftRelease = false;
						SoundEngine.PlaySound(12);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
						}
						PlayerInput.LockGamepadButtons("Grapple");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
			}
		}
		switch (context)
		{
		case 12:
		case 25:
		case 27:
		case 33:
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					if (Main.mouseItem.dye > 0)
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
				if (context == 12 || context == 25 || context == 27 || context == 33)
				{
					int num2 = -1;
					if (inv == player.dye)
					{
						num2 = slot;
					}
					if (inv == player.miscDyes)
					{
						num2 = 10 + slot;
					}
					if (num2 != -1)
					{
						if (num2 < 10)
						{
							bool flag7 = player.hideVisibleAccessory[slot];
							s += PlayerInput.BuildCommand(Lang.misc[flag7 ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
							if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
							{
								player.hideVisibleAccessory[slot] = !player.hideVisibleAccessory[slot];
								SoundEngine.PlaySound(12);
								if (Main.netMode == 1)
								{
									NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
								}
								PlayerInput.LockGamepadButtons("Grapple");
								PlayerInput.SettingsForUI.TryRevertingToMouseMode();
							}
						}
						else
						{
							bool flag8 = player.hideMisc[slot];
							s += PlayerInput.BuildCommand(Lang.misc[flag8 ? 77 : 78].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
							if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
							{
								player.hideMisc[slot] = !player.hideMisc[slot];
								SoundEngine.PlaySound(12);
								if (Main.netMode == 1)
								{
									NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
								}
								PlayerInput.LockGamepadButtons("Grapple");
								PlayerInput.SettingsForUI.TryRevertingToMouseMode();
							}
						}
					}
				}
			}
			else if (Main.mouseItem.type > 0 && Main.mouseItem.dye > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
			return s;
		case 18:
		{
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					if (Main.mouseItem.dye > 0)
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
			}
			else if (Main.mouseItem.type > 0 && Main.mouseItem.dye > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
			bool enabledSuperCart = player.enabledSuperCart;
			s += PlayerInput.BuildCommand(Language.GetTextValue((!enabledSuperCart) ? "UI.EnableSuperCart" : "UI.DisableSuperCart"), false, PlayerInput.ProfileGamepadUI.KeyStatus["Grapple"]);
			if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.Grapple)
			{
				player.enabledSuperCart = !player.enabledSuperCart;
				SoundEngine.PlaySound(12);
				if (Main.netMode == 1)
				{
					NetMessage.SendData(4, -1, -1, null, Main.myPlayer);
				}
				PlayerInput.LockGamepadButtons("Grapple");
				PlayerInput.SettingsForUI.TryRevertingToMouseMode();
			}
			return s;
		}
		case 6:
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				s = ((Main.mouseItem.type <= 0) ? (s + PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"])) : (s + PlayerInput.BuildCommand(Lang.misc[74].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"])));
			}
			else if (Main.mouseItem.type > 0)
			{
				s += PlayerInput.BuildCommand(Lang.misc[74].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
			return s;
		case 5:
		case 7:
		{
			bool flag6 = false;
			if (context == 5)
			{
				flag6 = Main.mouseItem.Prefix(-3) || Main.mouseItem.type == 0;
			}
			if (context == 7)
			{
				flag6 = Main.mouseItem.material;
			}
			if (inv[slot].type > 0 && inv[slot].stack > 0)
			{
				if (Main.mouseItem.type > 0)
				{
					if (flag6)
					{
						s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
					}
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[54].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
				}
			}
			else if (Main.mouseItem.type > 0 && flag6)
			{
				s += PlayerInput.BuildCommand(Lang.misc[65].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["MouseLeft"]);
			}
			return s;
		}
		default:
		{
			string overrideInstructions = GetOverrideInstructions(inv, context, slot);
			bool flag5 = Main.mouseItem.type > 0 && (context == 0 || context == 1 || context == 2 || context == 6 || context == 15 || context == 7 || context == 4 || context == 32 || context == 3);
			if (context != 8 || !isEquipLocked(inv[slot].type))
			{
				if (flag5 && string.IsNullOrEmpty(overrideInstructions))
				{
					s += PlayerInput.BuildCommand(Lang.inter[121].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]);
					if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
					{
						player.DropSelectedItem();
						PlayerInput.LockGamepadButtons("SmartSelect");
						PlayerInput.SettingsForUI.TryRevertingToMouseMode();
					}
				}
				else if (!string.IsNullOrEmpty(overrideInstructions))
				{
					ShiftForcedOn = true;
					int cursorOverride = Main.cursorOverride;
					OverrideHover(inv, context, slot);
					if (-1 != Main.cursorOverride)
					{
						s += PlayerInput.BuildCommand(overrideInstructions, false, PlayerInput.ProfileGamepadUI.KeyStatus["SmartSelect"]);
						if (CanDoSimulatedClickAction() && CanExecuteCommand() && PlayerInput.Triggers.JustPressed.SmartSelect)
						{
							bool mouseLeft = Main.mouseLeft;
							Main.mouseLeft = true;
							LeftClick(inv, context, slot);
							Main.mouseLeft = mouseLeft;
							PlayerInput.LockGamepadButtons("SmartSelect");
							PlayerInput.SettingsForUI.TryRevertingToMouseMode();
						}
					}
					Main.cursorOverride = cursorOverride;
					ShiftForcedOn = false;
				}
			}
			if (!TryEnteringFastUseMode(inv, context, slot, player, ref s))
			{
				TryEnteringBuildingMode(inv, context, slot, player, ref s);
			}
			return s;
		}
		}
	}

	private static bool CanDoSimulatedClickAction()
	{
		if (PlayerInput.SteamDeckIsUsed)
		{
			return UILinkPointNavigator.InUse;
		}
		return true;
	}

	private static bool TryEnteringFastUseMode(Item[] inv, int context, int slot, Player player, ref string s)
	{
		int num = 0;
		if (Main.mouseItem.CanBeQuickUsed)
		{
			num = 1;
		}
		if (num == 0 && Main.mouseItem.stack <= 0 && context == 0 && inv[slot].CanBeQuickUsed)
		{
			num = 2;
		}
		if (num > 0)
		{
			s += PlayerInput.BuildCommand(Language.GetTextValue("UI.QuickUseItem"), false, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
			if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.QuickMount)
			{
				switch (num)
				{
				case 1:
					PlayerInput.TryEnteringFastUseModeForMouseItem();
					break;
				case 2:
					PlayerInput.TryEnteringFastUseModeForInventorySlot(slot);
					break;
				}
			}
			return true;
		}
		return false;
	}

	private static bool TryEnteringBuildingMode(Item[] inv, int context, int slot, Player player, ref string s)
	{
		int num = 0;
		if (IsABuildingItem(Main.mouseItem))
		{
			num = 1;
		}
		if (num == 0 && Main.mouseItem.stack <= 0 && context == 0 && IsABuildingItem(inv[slot]))
		{
			num = 2;
		}
		if (num > 0)
		{
			Item item = Main.mouseItem;
			if (num == 1)
			{
				item = Main.mouseItem;
			}
			if (num == 2)
			{
				item = inv[slot];
			}
			if (num != 1 || player.ItemSpace(item).CanTakeItemToPersonalInventory)
			{
				if (item.damage > 0 && item.ammo == 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[60].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
				}
				else if (item.createTile >= 0 || item.createWall > 0)
				{
					s += PlayerInput.BuildCommand(Lang.misc[61].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
				}
				else
				{
					s += PlayerInput.BuildCommand(Lang.misc[63].Value, false, PlayerInput.ProfileGamepadUI.KeyStatus["QuickMount"]);
				}
				if (CanExecuteCommand() && PlayerInput.Triggers.JustPressed.QuickMount)
				{
					PlayerInput.EnterBuildingMode();
				}
				return true;
			}
		}
		return false;
	}

	public static bool IsABuildingItem(Item item)
	{
		if (item.type > 0 && item.stack > 0 && item.useStyle != 0)
		{
			return item.useTime > 0;
		}
		return false;
	}

	public static void SelectEquipPage(Item item)
	{
		Main.EquipPage = -1;
		if (!item.IsAir)
		{
			if (Main.projHook[item.shoot])
			{
				Main.EquipPage = 2;
			}
			else if (item.mountType != -1)
			{
				Main.EquipPage = 2;
			}
			else if (item.buffType > 0 && Main.vanityPet[item.buffType])
			{
				Main.EquipPage = 2;
			}
			else if (item.buffType > 0 && Main.lightPet[item.buffType])
			{
				Main.EquipPage = 2;
			}
			else if (item.dye > 0 && Main.EquipPageSelected == 1)
			{
				Main.EquipPage = 0;
			}
			else if (item.legSlot != -1 || item.headSlot != -1 || item.bodySlot != -1 || item.accessory)
			{
				Main.EquipPage = 0;
			}
		}
	}

	private static bool AccessorySwap(Player player, Item item, ref Item result)
	{
		accSlotToSwapTo = -1;
		AccessorySlotLoader accLoader = LoaderManager.Get<AccessorySlotLoader>();
		Item[] accessories = AccessorySlotLoader.ModSlotPlayer(player).exAccessorySlot;
		for (int j = 3; j < 10; j++)
		{
			if (player.IsItemSlotUnlockedAndUsable(j) && player.armor[j].type == 0 && ItemLoader.CanEquipAccessory(item, j, modded: false))
			{
				accSlotToSwapTo = j - 3;
				break;
			}
		}
		if (accSlotToSwapTo < 0)
		{
			for (int i = 0; i < accessories.Length / 2; i++)
			{
				if (accLoader.ModdedIsItemSlotUnlockedAndUsable(i, player) && accessories[i].type == 0 && accLoader.CanAcceptItem(i, item, -10) && ItemLoader.CanEquipAccessory(item, i, modded: true))
				{
					accSlotToSwapTo = i + 20;
					break;
				}
			}
		}
		accLoader.ModifyDefaultSwapSlot(item, ref accSlotToSwapTo);
		for (int l = 0; l < player.armor.Length; l++)
		{
			if (item.IsTheSameAs(player.armor[l]) && ItemLoader.CanEquipAccessory(item, l, modded: false))
			{
				accSlotToSwapTo = l - 3;
			}
			if (l < 10 && ((item.wingSlot > 0 && player.armor[l].wingSlot > 0) || !ItemLoader.CanAccessoryBeEquippedWith(player.armor[l], item)) && ItemLoader.CanEquipAccessory(item, l, modded: false))
			{
				accSlotToSwapTo = l - 3;
			}
		}
		for (int k = 0; k < accessories.Length; k++)
		{
			if (item.IsTheSameAs(accessories[k]) && accLoader.CanAcceptItem(k, item, (k < accessories.Length / 2) ? (-10) : (-11)) && ItemLoader.CanEquipAccessory(item, k, modded: true))
			{
				accSlotToSwapTo = k + 20;
			}
			if (k < accLoader.list.Count && ((item.wingSlot > 0 && accessories[k].wingSlot > 0) || !ItemLoader.CanAccessoryBeEquippedWith(accessories[k], item)) && accLoader.CanAcceptItem(k, item, (k < accessories.Length / 2) ? (-10) : (-11)) && ItemLoader.CanEquipAccessory(item, k, modded: true))
			{
				accSlotToSwapTo = k + 20;
			}
		}
		if (accSlotToSwapTo == -1 && !ItemLoader.CanEquipAccessory(item, 0, modded: false))
		{
			return false;
		}
		accSlotToSwapTo = Math.Max(accSlotToSwapTo, 0);
		if (accSlotToSwapTo >= 20)
		{
			int num3 = accSlotToSwapTo - 20;
			if (isEquipLocked(accessories[num3].type))
			{
				result = item;
				return false;
			}
			result = accessories[num3].Clone();
			accessories[num3] = item.Clone();
		}
		else
		{
			int num4 = 3 + accSlotToSwapTo;
			if (isEquipLocked(player.armor[num4].type))
			{
				result = item;
				return false;
			}
			result = player.armor[num4].Clone();
			player.armor[num4] = item.Clone();
		}
		return true;
	}

	/// <summary>
	/// Alters the ItemSlot.DyeSwap method for modded slots;
	/// Unfortunately, I (Solxan) couldn't ever get ItemSlot.DyeSwap invoked so pretty sure this and its vanilla code is defunct.
	/// Here in case someone proves my statement wrong later.
	/// </summary>
	private static Item ModSlotDyeSwap(Item item, out bool success)
	{
		Item item2 = item;
		ModAccessorySlotPlayer msPlayer = AccessorySlotLoader.ModSlotPlayer(Main.LocalPlayer);
		int dyeSlotCount = 0;
		Item[] dyes = msPlayer.exDyesAccessory;
		for (int i = 0; i < dyeSlotCount; i++)
		{
			if (dyes[i].type == 0)
			{
				dyeSlotCount = i;
				break;
			}
		}
		if (dyeSlotCount >= msPlayer.SlotCount)
		{
			success = false;
			return item2;
		}
		item2 = dyes[dyeSlotCount].Clone();
		dyes[dyeSlotCount] = item.Clone();
		SoundEngine.PlaySound(7);
		Recipe.FindRecipes();
		success = true;
		return item2;
	}

	internal static bool AccCheck_ForLocalPlayer(Item[] itemCollection, Item item, int slot)
	{
		if (isEquipLocked(item.type))
		{
			return true;
		}
		if (slot != -1)
		{
			if (itemCollection[slot].IsTheSameAs(item))
			{
				return false;
			}
			if ((itemCollection[slot].wingSlot > 0 && item.wingSlot > 0) || !ItemLoader.CanAccessoryBeEquippedWith(itemCollection[slot], item))
			{
				return !ItemLoader.CanEquipAccessory(item, slot % 20, slot >= 20);
			}
		}
		int modCount = AccessorySlotLoader.ModSlotPlayer(Main.LocalPlayer).SlotCount;
		bool targetVanity = (slot >= 20 && slot >= modCount + 20) || (slot < 20 && slot >= 10);
		for (int k = (targetVanity ? 13 : 3); k < (targetVanity ? 20 : 10); k++)
		{
			if ((!targetVanity && item.wingSlot > 0 && itemCollection[k].wingSlot > 0) || !ItemLoader.CanAccessoryBeEquippedWith(itemCollection[k], item))
			{
				return true;
			}
		}
		for (int j = (targetVanity ? modCount : 0) + 20; j < (targetVanity ? (modCount * 2) : modCount) + 20; j++)
		{
			if ((!targetVanity && item.wingSlot > 0 && itemCollection[j].wingSlot > 0) || !ItemLoader.CanAccessoryBeEquippedWith(itemCollection[j], item))
			{
				return true;
			}
		}
		for (int i = 0; i < itemCollection.Length; i++)
		{
			if (item.IsTheSameAs(itemCollection[i]))
			{
				return true;
			}
		}
		return !ItemLoader.CanEquipAccessory(item, slot % 20, slot >= 20);
	}
}
