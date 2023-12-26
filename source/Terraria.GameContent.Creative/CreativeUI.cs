using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Net;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.Creative;

public class CreativeUI
{
	public enum ItemSacrificeResult
	{
		CannotSacrifice,
		SacrificedButNotDone,
		SacrificedAndDone
	}

	public const int ItemSlotIndexes_SacrificeItem = 0;

	public const int ItemSlotIndexes_Count = 1;

	private bool _initialized;

	private Asset<Texture2D> _buttonTexture;

	private Asset<Texture2D> _buttonBorderTexture;

	private Item[] _itemSlotsForUI = new Item[1];

	private List<int> _itemIdsAvailableInfinitely = new List<int>();

	private UserInterface _powersUI = new UserInterface();

	public int GamepadPointIdForInfiniteItemSearchHack = -1;

	public bool GamepadMoveToSearchButtonHack;

	private UICreativePowersMenu _uiState;

	public bool Enabled { get; private set; }

	public bool Blocked
	{
		get
		{
			if (Main.LocalPlayer.talkNPC == -1)
			{
				return Main.LocalPlayer.chest != -1;
			}
			return true;
		}
	}

	public CreativeUI()
	{
		for (int i = 0; i < _itemSlotsForUI.Length; i++)
		{
			_itemSlotsForUI[i] = new Item();
		}
	}

	public void Initialize()
	{
		_buttonTexture = Main.Assets.Request<Texture2D>("Images/UI/Creative/Journey_Toggle");
		_buttonBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Creative/Journey_Toggle_MouseOver");
		_itemIdsAvailableInfinitely.Clear();
		_uiState = new UICreativePowersMenu();
		_powersUI.SetState(_uiState);
		_initialized = true;
	}

	public void Update(GameTime gameTime)
	{
		if (Enabled && Main.playerInventory)
		{
			_powersUI.Update(gameTime);
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		if (!_initialized)
		{
			Initialize();
		}
		if (Main.LocalPlayer.difficulty != 3)
		{
			Enabled = false;
		}
		else if (!Blocked)
		{
			Vector2 location = default(Vector2);
			((Vector2)(ref location))._002Ector(28f, 267f);
			Vector2 val = new Vector2(353f, 258f);
			new Vector2(40f, 267f);
			_ = val + new Vector2(50f, 50f);
			if (Main.screenHeight < 650 && Enabled)
			{
				location.X += 52f * Main.inventoryScale;
			}
			DrawToggleButton(spriteBatch, location);
			if (Enabled)
			{
				_powersUI.Draw(spriteBatch, Main.gameTimeCache);
			}
		}
	}

	public UIElement ProvideItemSlotElement(int itemSlotContext)
	{
		if (itemSlotContext != 0)
		{
			return null;
		}
		return new UIItemSlot(_itemSlotsForUI, itemSlotContext, 30);
	}

	public Item GetItemByIndex(int itemSlotContext)
	{
		if (itemSlotContext != 0)
		{
			return null;
		}
		return _itemSlotsForUI[itemSlotContext];
	}

	public void SetItembyIndex(Item item, int itemSlotContext)
	{
		if (itemSlotContext == 0)
		{
			_itemSlotsForUI[itemSlotContext] = item;
		}
	}

	private void DrawToggleButton(SpriteBatch spritebatch, Vector2 location)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = _buttonTexture.Size();
		Rectangle rectangle = Utils.CenteredRectangle(location + vector / 2f, vector);
		UILinkPointNavigator.SetPosition(311, ((Rectangle)(ref rectangle)).Center.ToVector2());
		spritebatch.Draw(_buttonTexture.Value, location, (Rectangle?)null, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		Main.LocalPlayer.creativeInterface = false;
		if (((Rectangle)(ref rectangle)).Contains(Main.MouseScreen.ToPoint()))
		{
			Main.LocalPlayer.creativeInterface = true;
			Main.LocalPlayer.mouseInterface = true;
			if (Enabled)
			{
				Main.instance.MouseText(Language.GetTextValue("CreativePowers.PowersMenuOpen"), 0, 0);
			}
			else
			{
				Main.instance.MouseText(Language.GetTextValue("CreativePowers.PowersMenuClosed"), 0, 0);
			}
			spritebatch.Draw(_buttonBorderTexture.Value, location, (Rectangle?)null, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				ToggleMenu();
			}
		}
	}

	public void SwapItem(ref Item item)
	{
		Utils.Swap(ref item, ref _itemSlotsForUI[0]);
	}

	public void CloseMenu()
	{
		Enabled = false;
		if (_itemSlotsForUI[0].stack > 0)
		{
			_itemSlotsForUI[0] = Main.LocalPlayer.GetItem(Main.myPlayer, _itemSlotsForUI[0], GetItemSettings.InventoryUIToInventorySettings);
		}
		StopPlayingSacrificeAnimations();
	}

	public void ToggleMenu()
	{
		Enabled = !Enabled;
		_powersUI.EscapeElements();
		UISliderBase.EscapeElements();
		SoundEngine.PlaySound(12);
		if (Enabled)
		{
			Recipe.FindRecipes();
			Main.LocalPlayer.tileEntityAnchor.Clear();
			RefreshAvailableInfiniteItemsList();
		}
		else if (_itemSlotsForUI[0].stack > 0)
		{
			_itemSlotsForUI[0] = Main.LocalPlayer.GetItem(Main.myPlayer, _itemSlotsForUI[0], GetItemSettings.InventoryUIToInventorySettings);
			StopPlayingSacrificeAnimations();
		}
	}

	public bool IsShowingResearchMenu()
	{
		if (Enabled && _uiState != null)
		{
			return _uiState.IsShowingResearchMenu;
		}
		return false;
	}

	public void SacrificeItemInSacrificeSlot()
	{
		if (_uiState != null)
		{
			_uiState.SacrificeWhatsInResearchMenu();
		}
	}

	public void StopPlayingSacrificeAnimations()
	{
		if (_uiState != null)
		{
			_uiState.StopPlayingResearchAnimations();
		}
	}

	public bool ShouldDrawSacrificeArea()
	{
		if (!_itemSlotsForUI[0].IsAir)
		{
			return true;
		}
		Item mouseItem = Main.mouseItem;
		if (mouseItem.IsAir)
		{
			return false;
		}
		if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(mouseItem.type, out var amountNeeded))
		{
			return false;
		}
		if (Main.LocalPlayerCreativeTracker.ItemSacrifices.GetSacrificeCount(mouseItem.type) < amountNeeded)
		{
			return true;
		}
		return false;
	}

	public bool GetSacrificeNumbers(out int itemIdChecked, out int amountWeHave, out int amountNeededTotal)
	{
		amountWeHave = 0;
		amountNeededTotal = 0;
		itemIdChecked = 0;
		Item item = _itemSlotsForUI[0];
		if (!item.IsAir)
		{
			itemIdChecked = item.type;
		}
		if (!Main.LocalPlayerCreativeTracker.ItemSacrifices.TryGetSacrificeNumbers(item.type, out amountWeHave, out amountNeededTotal))
		{
			return false;
		}
		return true;
	}

	public ItemSacrificeResult SacrificeItem(out int amountWeSacrificed)
	{
		return SacrificeItem(ref _itemSlotsForUI[0], out amountWeSacrificed);
	}

	public static ItemSacrificeResult SacrificeItem(Item toSacrifice, out int amountWeSacrificed)
	{
		return SacrificeItem(ref toSacrifice, out amountWeSacrificed);
	}

	public static ItemSacrificeResult SacrificeItem(ref Item item, out int amountWeSacrificed, bool returnRemainderToPlayer = false)
	{
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		int amountNeededTotal = 0;
		int amountWeHave = 0;
		amountWeSacrificed = 0;
		if (!ItemLoader.CanResearch(item))
		{
			return ItemSacrificeResult.CannotSacrifice;
		}
		if (!Main.LocalPlayerCreativeTracker.ItemSacrifices.TryGetSacrificeNumbers(item.type, out amountWeHave, out amountNeededTotal))
		{
			return ItemSacrificeResult.CannotSacrifice;
		}
		int num = Utils.Clamp(amountNeededTotal - amountWeHave, 0, amountNeededTotal);
		if (num == 0)
		{
			return ItemSacrificeResult.CannotSacrifice;
		}
		int num2 = Math.Min(num, item.stack);
		if (!Main.ServerSideCharacter)
		{
			Main.LocalPlayerCreativeTracker.ItemSacrifices.RegisterItemSacrifice(item.type, num2);
		}
		else
		{
			NetPacket packet = NetCreativeUnlocksPlayerReportModule.SerializeSacrificeRequest(item.type, num2);
			NetManager.Instance.SendToServerOrLoopback(packet);
		}
		bool num3 = num2 == num;
		ItemLoader.OnResearched(item, num3);
		item.stack -= num2;
		if (item.stack <= 0)
		{
			item.TurnToAir();
		}
		amountWeSacrificed = num2;
		Main.CreativeMenu.RefreshAvailableInfiniteItemsList();
		if (item.stack > 0 && returnRemainderToPlayer)
		{
			item.position.X = Main.player[Main.myPlayer].Center.X - (float)(item.width / 2);
			item.position.Y = Main.player[Main.myPlayer].Center.Y - (float)(item.height / 2);
			item = Main.LocalPlayer.GetItem(Main.myPlayer, item, GetItemSettings.InventoryUIToInventorySettings);
		}
		if (!num3)
		{
			return ItemSacrificeResult.SacrificedButNotDone;
		}
		return ItemSacrificeResult.SacrificedAndDone;
	}

	private void RefreshAvailableInfiniteItemsList()
	{
		_itemIdsAvailableInfinitely.Clear();
		Main.LocalPlayerCreativeTracker.ItemSacrifices.FillListOfItemsThatCanBeObtainedInfinitely(_itemIdsAvailableInfinitely);
	}

	public void Reset()
	{
		for (int i = 0; i < _itemSlotsForUI.Length; i++)
		{
			_itemSlotsForUI[i].TurnToAir();
		}
		_initialized = false;
		Enabled = false;
	}

	public static ItemSacrificeResult ResearchItem(int type)
	{
		if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(type, out var amountNeeded))
		{
			return ItemSacrificeResult.CannotSacrifice;
		}
		int amountWeSacrificed;
		return SacrificeItem(new Item(type, amountNeeded), out amountWeSacrificed);
	}

	/// <summary>
	/// Method that allows you to easily get how many items of a type you have researched so far
	/// </summary>
	/// <param name="type">The item type to check.</param>
	/// <param name="fullyResearched">True if it is fully researched.</param>
	/// <returns></returns>
	public static int GetSacrificeCount(int type, out bool fullyResearched)
	{
		fullyResearched = false;
		if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(type, out var amountNeeded))
		{
			return 0;
		}
		Main.LocalPlayerCreativeTracker.ItemSacrifices._sacrificesCountByItemIdCache.TryGetValue(type, out var amountSacrificed);
		fullyResearched = amountSacrificed >= amountNeeded;
		return amountSacrificed;
	}

	/// <summary>
	/// Method that allows you to easily get how many items of a type you need to fully research that item
	/// </summary>
	/// <param name="type">The item type to check.</param>
	/// <returns>The number of sacrifices remaining , or null if the item can never be unlocked.</returns>
	public static int? GetSacrificesRemaining(int type)
	{
		if (!CreativeItemSacrificesCatalog.Instance.TryGetSacrificeCountCapToUnlockInfiniteItems(type, out var amountNeeded))
		{
			return null;
		}
		Main.LocalPlayerCreativeTracker.ItemSacrifices._sacrificesCountByItemIdCache.TryGetValue(type, out var amountSacrificed);
		return Utils.Clamp(amountNeeded - amountSacrificed, 0, amountNeeded);
	}
}
