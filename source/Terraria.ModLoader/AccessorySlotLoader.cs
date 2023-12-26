using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Default;
using Terraria.UI;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as a central place to store equipment slots and their corresponding textures. You will use this to obtain the IDs for your equipment textures.
/// </summary>
public class AccessorySlotLoader : Loader<ModAccessorySlot>
{
	public const int MaxVanillaSlotCount = 7;

	public static string[] scrollStackLang = new string[2]
	{
		Language.GetTextValue("tModLoader.slotStack"),
		Language.GetTextValue("tModLoader.slotScroll")
	};

	private int slotDrawLoopCounter;

	private static Player Player => Main.LocalPlayer;

	/// <summary>
	/// The variable known as num20 used to align all equipment slot drawing in Main.
	/// Represents the y position where equipment slots start to be drawn from.
	/// </summary>
	public static int DrawVerticalAlignment { get; private set; }

	/// <summary>
	/// The variable that determines where the DefenseIcon will be drawn, after considering all slot information.
	/// </summary>
	public static Vector2 DefenseIconPosition { get; private set; }

	internal static ModAccessorySlotPlayer ModSlotPlayer(Player player)
	{
		return player.GetModPlayer<ModAccessorySlotPlayer>();
	}

	public AccessorySlotLoader()
	{
		Initialize(0);
	}

	private ModAccessorySlot GetIdCorrected(int id)
	{
		if (id < list.Count)
		{
			return list[id];
		}
		return new UnloadedAccessorySlot(id, "TEMP'd");
	}

	public ModAccessorySlot Get(int id, Player player)
	{
		return GetIdCorrected(id % ModSlotPlayer(player).SlotCount);
	}

	public new ModAccessorySlot Get(int id)
	{
		return Get(id, Player);
	}

	internal int GetAccessorySlotPerColumn()
	{
		float minimumClearance = (float)DrawVerticalAlignment + 112f * Main.inventoryScale + 4f;
		return (int)(((float)Main.screenHeight - minimumClearance) / (56f * Main.inventoryScale) - 1.8f);
	}

	public void DrawAccSlots(int num20)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		int skip = 0;
		DrawVerticalAlignment = num20;
		Color color = Main.inventoryBack;
		for (int vanillaSlot = 3; vanillaSlot < Player.dye.Length; vanillaSlot++)
		{
			if (!Draw(skip, modded: false, vanillaSlot, color))
			{
				skip++;
			}
		}
		for (int modSlot = 0; modSlot < ModSlotPlayer(Player).SlotCount; modSlot++)
		{
			if (!Draw(skip, modded: true, modSlot, color))
			{
				skip++;
			}
		}
		if (skip == 7 + ModSlotPlayer(Player).SlotCount)
		{
			ModSlotPlayer(Player).scrollbarSlotPosition = 0;
			return;
		}
		int accessoryPerColumn = GetAccessorySlotPerColumn();
		int slotsToRender = ModSlotPlayer(Player).SlotCount + 7 - skip;
		int scrollIncrement = slotsToRender - accessoryPerColumn;
		if (scrollIncrement < 0)
		{
			accessoryPerColumn = slotsToRender;
			scrollIncrement = 0;
		}
		DefenseIconPosition = new Vector2((float)(Main.screenWidth - 64 - 28), (float)DrawVerticalAlignment + (float)((accessoryPerColumn + 2) * 56) * Main.inventoryScale + 4f);
		if (scrollIncrement > 0)
		{
			DrawScrollSwitch();
			if (ModSlotPlayer(Player).scrollSlots)
			{
				DrawScrollbar(accessoryPerColumn, slotsToRender, scrollIncrement);
			}
		}
		else
		{
			ModSlotPlayer(Player).scrollbarSlotPosition = 0;
		}
	}

	internal void DrawScrollSwitch()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value4 = TextureAssets.InventoryTickOn.Value;
		if (ModSlotPlayer(Player).scrollSlots)
		{
			value4 = TextureAssets.InventoryTickOff.Value;
		}
		int xLoc2 = Main.screenWidth - 64 - 28 + 47 + 9;
		int yLoc2 = (int)((float)DrawVerticalAlignment + 168f * Main.inventoryScale) - 10;
		Main.spriteBatch.Draw(value4, new Vector2((float)xLoc2, (float)yLoc2), Color.White * 0.7f);
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(xLoc2, yLoc2, value4.Width, value4.Height);
		if (((Rectangle)(ref rectangle)).Contains(new Point(Main.mouseX, Main.mouseY)) && !PlayerInput.IgnoreMouseInterface)
		{
			Main.LocalPlayer.mouseInterface = true;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				ModSlotPlayer(Player).scrollSlots = !ModSlotPlayer(Player).scrollSlots;
				SoundEngine.PlaySound(12);
			}
			int num45 = (ModSlotPlayer(Player).scrollSlots ? 1 : 0);
			Main.HoverItem = new Item();
			Main.hoverItemName = scrollStackLang[num45];
		}
	}

	internal void DrawScrollbar(int accessoryPerColumn, int slotsToRender, int scrollIncrement)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		int xLoc = Main.screenWidth - 64 - 28;
		int chkMax = (int)((float)DrawVerticalAlignment + (float)((accessoryPerColumn + 3) * 56) * Main.inventoryScale) + 4;
		int chkMin = (int)((float)DrawVerticalAlignment + 168f * Main.inventoryScale) + 4;
		UIScrollbar uIScrollbar = new UIScrollbar();
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(xLoc + 47 + 6, chkMin, 5, chkMax - chkMin);
		uIScrollbar.DrawBar(Main.spriteBatch, Main.Assets.Request<Texture2D>("Images/UI/Scrollbar").Value, rectangle, Color.White);
		int barSize = (chkMax - chkMin) / (scrollIncrement + 1);
		((Rectangle)(ref rectangle))._002Ector(xLoc + 47 + 5, chkMin + ModSlotPlayer(Player).scrollbarSlotPosition * barSize, 3, barSize);
		uIScrollbar.DrawBar(Main.spriteBatch, Main.Assets.Request<Texture2D>("Images/UI/ScrollbarInner").Value, rectangle, Color.White);
		((Rectangle)(ref rectangle))._002Ector(xLoc - 94, chkMin, 141, chkMax - chkMin);
		if (((Rectangle)(ref rectangle)).Contains(new Point(Main.mouseX, Main.mouseY)) && !PlayerInput.IgnoreMouseInterface)
		{
			PlayerInput.LockVanillaMouseScroll("ModLoader/Acc");
			int scrollDelta = ModSlotPlayer(Player).scrollbarSlotPosition + PlayerInput.ScrollWheelDelta / 120;
			scrollDelta = Math.Min(scrollDelta, scrollIncrement);
			scrollDelta = Math.Max(scrollDelta, 0);
			ModSlotPlayer(Player).scrollbarSlotPosition = scrollDelta;
			PlayerInput.ScrollWheelDelta = 0;
		}
	}

	/// <summary>
	/// Draws Vanilla and Modded Accessory Slots
	/// </summary>
	public bool Draw(int skip, bool modded, int slot, Color color)
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		bool flag4 = false;
		bool flag3;
		if (modded)
		{
			flag3 = !ModdedIsItemSlotUnlockedAndUsable(slot, Player);
			flag4 = !ModdedCanSlotBeShown(slot);
		}
		else
		{
			flag3 = !Player.IsItemSlotUnlockedAndUsable(slot);
			switch (slot)
			{
			case 8:
				flag4 = slot == 8 && !Player.CanDemonHeartAccessoryBeShown();
				break;
			case 9:
				flag4 = !Player.CanMasterModeAccessoryBeShown();
				break;
			}
		}
		if ((flag4 && flag3) || (modded && IsHidden(slot)))
		{
			return false;
		}
		Main.inventoryBack = (Color)(flag3 ? new Color(80, 80, 80, 80) : color);
		slotDrawLoopCounter = 0;
		int yLoc = 0;
		int xLoc = 0;
		bool customLoc = false;
		if (modded)
		{
			ModAccessorySlot mAccSlot = Get(slot);
			customLoc = mAccSlot.CustomLocation.HasValue;
			if (!customLoc && Main.EquipPage != 0)
			{
				Main.inventoryBack = color;
				return false;
			}
			if (customLoc)
			{
				xLoc = (int)(mAccSlot.CustomLocation?.X).Value;
				yLoc = (int)(mAccSlot.CustomLocation?.Y).Value;
			}
			else if (!SetDrawLocation(slot + Player.dye.Length - 3, skip, ref xLoc, ref yLoc))
			{
				Main.inventoryBack = color;
				return true;
			}
			ModAccessorySlot modAccessorySlot = Get(slot);
			if (modAccessorySlot.DrawFunctionalSlot)
			{
				DrawSlot(skipCheck: DrawVisibility(ref ModSlotPlayer(Player).exHideAccessory[slot], -10, xLoc, yLoc, out var xLoc2, out var yLoc2, out var value4), items: ModSlotPlayer(Player).exAccessorySlot, context: -10, slot: slot, flag3: flag3, xLoc: xLoc, yLoc: yLoc);
				Main.spriteBatch.Draw(value4, new Vector2((float)xLoc2, (float)yLoc2), Color.White * 0.7f);
			}
			if (modAccessorySlot.DrawVanitySlot)
			{
				DrawSlot(ModSlotPlayer(Player).exAccessorySlot, -11, slot + ModSlotPlayer(Player).SlotCount, flag3, xLoc, yLoc);
			}
			if (modAccessorySlot.DrawDyeSlot)
			{
				DrawSlot(ModSlotPlayer(Player).exDyesAccessory, -12, slot, flag3, xLoc, yLoc);
			}
		}
		else
		{
			if (!customLoc && Main.EquipPage != 0)
			{
				Main.inventoryBack = color;
				return false;
			}
			if (!SetDrawLocation(slot - 3, skip, ref xLoc, ref yLoc))
			{
				Main.inventoryBack = color;
				return true;
			}
			int xLoc3;
			int yLoc3;
			Texture2D value5;
			bool skipMouse = DrawVisibility(ref Player.hideVisibleAccessory[slot], 10, xLoc, yLoc, out xLoc3, out yLoc3, out value5);
			DrawSlot(Player.armor, 10, slot, flag3, xLoc, yLoc, skipMouse);
			Main.spriteBatch.Draw(value5, new Vector2((float)xLoc3, (float)yLoc3), Color.White * 0.7f);
			DrawSlot(Player.armor, 11, slot + Player.dye.Length, flag3, xLoc, yLoc);
			DrawSlot(Player.dye, 12, slot, flag3, xLoc, yLoc);
		}
		Main.inventoryBack = color;
		return !customLoc;
	}

	/// <summary>
	/// Applies Xloc and Yloc data for the slot, based on ModAccessorySlotPlayer.scrollSlots
	/// </summary>
	internal bool SetDrawLocation(int trueSlot, int skip, ref int xLoc, ref int yLoc)
	{
		int accessoryPerColumn = GetAccessorySlotPerColumn();
		int xColumn = trueSlot / accessoryPerColumn;
		int yRow = trueSlot % accessoryPerColumn;
		if (ModSlotPlayer(Player).scrollSlots)
		{
			int row = yRow + xColumn * accessoryPerColumn - ModSlotPlayer(Player).scrollbarSlotPosition - skip;
			yLoc = (int)((float)DrawVerticalAlignment + (float)((row + 3) * 56) * Main.inventoryScale) + 4;
			int chkMin = (int)((float)DrawVerticalAlignment + 168f * Main.inventoryScale) + 4;
			int chkMax = (int)((float)DrawVerticalAlignment + (float)((accessoryPerColumn - 1 + 3) * 56) * Main.inventoryScale) + 4;
			if (yLoc > chkMax || yLoc < chkMin)
			{
				return false;
			}
			xLoc = Main.screenWidth - 64 - 28;
		}
		else
		{
			int row2 = yRow;
			int tempSlot = trueSlot;
			int col = xColumn;
			if (skip > 0)
			{
				tempSlot -= skip;
				row2 = tempSlot % accessoryPerColumn;
				col = tempSlot / accessoryPerColumn;
			}
			yLoc = (int)((float)DrawVerticalAlignment + (float)((row2 + 3) * 56) * Main.inventoryScale) + 4;
			if (col > 0)
			{
				xLoc = Main.screenWidth - 64 - 28 - 141 * col - 50;
			}
			else
			{
				xLoc = Main.screenWidth - 64 - 28 - 141 * col;
			}
		}
		return true;
	}

	/// <summary>
	/// Is run in AccessorySlotLoader.Draw.
	/// Creates &amp; sets up Hide Visibility Button.
	/// </summary>
	internal bool DrawVisibility(ref bool visbility, int context, int xLoc, int yLoc, out int xLoc2, out int yLoc2, out Texture2D value4)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		yLoc2 = yLoc - 2;
		xLoc2 = xLoc - 58 + 64 + 28;
		value4 = TextureAssets.InventoryTickOn.Value;
		if (visbility)
		{
			value4 = TextureAssets.InventoryTickOff.Value;
		}
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(xLoc2, yLoc2, value4.Width, value4.Height);
		int num45 = 0;
		bool skipCheck = false;
		if (((Rectangle)(ref rectangle)).Contains(new Point(Main.mouseX, Main.mouseY)) && !PlayerInput.IgnoreMouseInterface)
		{
			skipCheck = true;
			Player.mouseInterface = true;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				visbility = !visbility;
				SoundEngine.PlaySound(12);
				if (Main.netMode == 1 && context > 0)
				{
					NetMessage.SendData(4, -1, -1, null, Player.whoAmI);
				}
			}
			num45 = ((!visbility) ? 1 : 2);
		}
		if (num45 > 0)
		{
			Main.HoverItem = new Item();
			Main.hoverItemName = Lang.inter[58 + num45].Value;
		}
		return skipCheck;
	}

	/// <summary>
	/// Is run in AccessorySlotLoader.Draw.
	/// Generates a significant amount of functionality for the slot, despite being named drawing because vanilla.
	/// At the end, calls this.DrawRedirect to enable custom drawing
	/// </summary>
	internal void DrawSlot(Item[] items, int context, int slot, bool flag3, int xLoc, int yLoc, bool skipCheck = false)
	{
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		bool flag4 = flag3 && !Main.mouseItem.IsAir;
		int xLoc2 = xLoc - 47 * slotDrawLoopCounter++;
		bool isHovered = false;
		if (!skipCheck && Main.mouseX >= xLoc2 && (float)Main.mouseX <= (float)xLoc2 + (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale && Main.mouseY >= yLoc && (float)Main.mouseY <= (float)yLoc + (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale && !PlayerInput.IgnoreMouseInterface)
		{
			isHovered = true;
			Player.mouseInterface = true;
			Main.armorHide = true;
			ItemSlot.OverrideHover(items, Math.Abs(context), slot);
			if (!flag4)
			{
				if (Math.Abs(context) == 12)
				{
					if (Main.mouseRightRelease && Main.mouseRight)
					{
						ItemSlot.RightClick(items, context, slot);
					}
					ItemSlot.LeftClick(items, context, slot);
				}
				else if (Math.Abs(context) == 11)
				{
					ItemSlot.LeftClick(items, context, slot);
					ItemSlot.RightClick(items, context, slot);
				}
				else if (Math.Abs(context) == 10)
				{
					ItemSlot.LeftClick(items, context, slot);
				}
			}
			ItemSlot.MouseHover(items, Math.Abs(context), slot);
			if (context < 0)
			{
				OnHover(slot, context);
			}
		}
		DrawRedirect(items, context, slot, new Vector2((float)xLoc2, (float)yLoc), isHovered);
	}

	internal void DrawRedirect(Item[] inv, int context, int slot, Vector2 location, bool isHovered)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		if (context < 0)
		{
			if (Get(slot).PreDraw(ContextToEnum(context), inv[slot], location, isHovered))
			{
				ItemSlot.Draw(Main.spriteBatch, inv, context, slot, location);
			}
			Get(slot).PostDraw(ContextToEnum(context), inv[slot], location, isHovered);
		}
		else
		{
			ItemSlot.Draw(Main.spriteBatch, inv, context, slot, location);
		}
	}

	/// <summary>
	/// Provides the Texture for a Modded Accessory Slot
	/// This probably will need optimization down the road.
	/// </summary>
	internal Texture2D GetBackgroundTexture(int slot, int context)
	{
		ModAccessorySlot thisSlot = Get(slot);
		switch (context)
		{
		case -10:
		{
			if (ModContent.RequestIfExists(thisSlot.FunctionalBackgroundTexture, out Asset<Texture2D> funcTexture, AssetRequestMode.AsyncLoad))
			{
				return funcTexture.Value;
			}
			return TextureAssets.InventoryBack3.Value;
		}
		case -11:
		{
			if (ModContent.RequestIfExists(thisSlot.VanityBackgroundTexture, out Asset<Texture2D> vanityTexture, AssetRequestMode.AsyncLoad))
			{
				return vanityTexture.Value;
			}
			return TextureAssets.InventoryBack8.Value;
		}
		case -12:
		{
			if (ModContent.RequestIfExists(thisSlot.DyeBackgroundTexture, out Asset<Texture2D> dyeTexture, AssetRequestMode.AsyncLoad))
			{
				return dyeTexture.Value;
			}
			return TextureAssets.InventoryBack12.Value;
		}
		default:
			return TextureAssets.InventoryBack3.Value;
		}
	}

	internal void DrawSlotTexture(Texture2D value6, Vector2 position, Rectangle rectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth, int slot, int context)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		ModAccessorySlot thisSlot = Get(slot);
		Texture2D texture = null;
		switch (context)
		{
		case -10:
		{
			if (ModContent.RequestIfExists(thisSlot.FunctionalTexture, out Asset<Texture2D> funcTexture, AssetRequestMode.AsyncLoad))
			{
				texture = funcTexture.Value;
			}
			break;
		}
		case -11:
		{
			if (ModContent.RequestIfExists(thisSlot.VanityTexture, out Asset<Texture2D> vanityTexture, AssetRequestMode.AsyncLoad))
			{
				texture = vanityTexture.Value;
			}
			break;
		}
		case -12:
		{
			if (ModContent.RequestIfExists(thisSlot.DyeTexture, out Asset<Texture2D> dyeTexture, AssetRequestMode.AsyncLoad))
			{
				texture = dyeTexture.Value;
			}
			break;
		}
		}
		if (texture == null)
		{
			texture = value6;
		}
		else
		{
			((Rectangle)(ref rectangle))._002Ector(0, 0, texture.Width, texture.Height);
			origin = rectangle.Size() / 2f;
		}
		Main.spriteBatch.Draw(texture, position, (Rectangle?)rectangle, color, rotation, origin, scale, effects, layerDepth);
	}

	public AccessorySlotType ContextToEnum(int context)
	{
		return (AccessorySlotType)Math.Abs(context);
	}

	public bool ModdedIsItemSlotUnlockedAndUsable(int index, Player player)
	{
		return Get(index, player).IsEnabled();
	}

	public void CustomUpdateEquips(int index, Player player)
	{
		Get(index, player).ApplyEquipEffects();
	}

	public bool ModdedCanSlotBeShown(int index)
	{
		return Get(index).IsVisibleWhenNotEnabled();
	}

	public bool IsHidden(int index)
	{
		return Get(index).IsHidden();
	}

	public bool CanAcceptItem(int index, Item checkItem, int context)
	{
		return Get(index).CanAcceptItem(checkItem, ContextToEnum(context));
	}

	public void OnHover(int index, int context)
	{
		Get(index).OnMouseHover(ContextToEnum(context));
	}

	/// <summary>
	/// Checks if the provided item can go in to the provided slot.
	/// Includes checking if the item already exists in either of Player.Armor or ModSlotPlayer.exAccessorySlot
	/// Invokes directly ItemSlot.AccCheck &amp; ModSlot.CanAcceptItem
	/// </summary>
	public bool ModSlotCheck(Item checkItem, int slot, int context)
	{
		if (CanAcceptItem(slot, checkItem, context))
		{
			return !ItemSlot.AccCheck_ForLocalPlayer(Player.armor.Concat(ModSlotPlayer(Player).exAccessorySlot).ToArray(), checkItem, slot + Player.armor.Length);
		}
		return false;
	}

	/// <summary>
	/// After checking for empty slots in ItemSlot.AccessorySwap, this allows for changing what the target slot will be if the accessory isn't already equipped.
	/// DOES NOT affect vanilla behavior of swapping items like for like where existing in a slot
	/// </summary>
	public void ModifyDefaultSwapSlot(Item item, ref int accSlotToSwapTo)
	{
		for (int num = ModSlotPlayer(Player).SlotCount - 1; num >= 0; num--)
		{
			if (ModdedIsItemSlotUnlockedAndUsable(num, Player) && Get(num).ModifyDefaultSwapSlot(item, accSlotToSwapTo))
			{
				accSlotToSwapTo = num + 20;
			}
		}
	}

	/// <summary>
	/// Mirrors Player.GetPreferredGolfBallToUse.
	/// Provides the golf ball projectile from an accessory slot.
	/// </summary>
	public bool PreferredGolfBall(ref int projType)
	{
		for (int num = ModSlotPlayer(Player).SlotCount * 2 - 1; num >= 0; num--)
		{
			if (ModdedIsItemSlotUnlockedAndUsable(num, Player))
			{
				Item item2 = ModSlotPlayer(Player).exAccessorySlot[num];
				if (!item2.IsAir && item2.shoot > 0 && ProjectileID.Sets.IsAGolfBall[item2.shoot])
				{
					projType = item2.shoot;
					return true;
				}
			}
		}
		return false;
	}
}
