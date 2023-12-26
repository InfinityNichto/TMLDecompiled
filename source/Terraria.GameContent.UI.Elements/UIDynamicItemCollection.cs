using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.Elements;

public class UIDynamicItemCollection : UIElement
{
	private List<int> _itemIdsAvailableToShow = new List<int>();

	private List<int> _itemIdsToLoadTexturesFor = new List<int>();

	private int _itemsPerLine;

	private const int sizePerEntryX = 44;

	private const int sizePerEntryY = 44;

	private List<SnapPoint> _dummySnapPoints = new List<SnapPoint>();

	private Item _item = new Item();

	public UIDynamicItemCollection()
	{
		Width = new StyleDimension(0f, 1f);
		HAlign = 0.5f;
		UpdateSize();
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		Main.inventoryScale = 0.84615386f;
		GetGridParameters(out var startX, out var startY, out var startItemIndex, out var endItemIndex);
		int num = _itemsPerLine;
		for (int i = startItemIndex; i < endItemIndex; i++)
		{
			int num2 = _itemIdsAvailableToShow[i];
			Rectangle itemSlotHitbox = GetItemSlotHitbox(startX, startY, startItemIndex, i);
			Item inv = ContentSamples.ItemsByType[num2];
			int context = 29;
			if (TextureAssets.Item[num2].State == AssetState.NotLoaded)
			{
				num--;
			}
			bool cREATIVE_ItemSlotShouldHighlightAsSelected = false;
			if (base.IsMouseHovering && ((Rectangle)(ref itemSlotHitbox)).Contains(Main.MouseScreen.ToPoint()) && !PlayerInput.IgnoreMouseInterface)
			{
				_item.SetDefaults(inv.type);
				inv = _item;
				Main.LocalPlayer.mouseInterface = true;
				ItemSlot.OverrideHover(ref inv, context);
				ItemSlot.LeftClick(ref inv, context);
				ItemSlot.RightClick(ref inv, context);
				ItemSlot.MouseHover(ref inv, context);
				cREATIVE_ItemSlotShouldHighlightAsSelected = true;
			}
			UILinkPointNavigator.Shortcuts.CREATIVE_ItemSlotShouldHighlightAsSelected = cREATIVE_ItemSlotShouldHighlightAsSelected;
			ItemSlot.Draw(spriteBatch, ref inv, context, itemSlotHitbox.TopLeft());
			if (num <= 0)
			{
				break;
			}
		}
		while (_itemIdsToLoadTexturesFor.Count > 0 && num > 0)
		{
			int num3 = _itemIdsToLoadTexturesFor[0];
			_itemIdsToLoadTexturesFor.RemoveAt(0);
			if (TextureAssets.Item[num3].State == AssetState.NotLoaded)
			{
				Main.instance.LoadItem(num3);
				num -= 4;
			}
		}
	}

	private Rectangle GetItemSlotHitbox(int startX, int startY, int startItemIndex, int i)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		int num4 = i - startItemIndex;
		int num2 = num4 % _itemsPerLine;
		int num3 = num4 / _itemsPerLine;
		return new Rectangle(startX + num2 * 44, startY + num3 * 44, 44, 44);
	}

	private void GetGridParameters(out int startX, out int startY, out int startItemIndex, out int endItemIndex)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = GetDimensions().ToRectangle();
		Rectangle viewCullingArea = base.Parent.GetViewCullingArea();
		int x = ((Rectangle)(ref rectangle)).Center.X;
		startX = x - (int)((float)(44 * _itemsPerLine) * 0.5f);
		startY = ((Rectangle)(ref rectangle)).Top;
		startItemIndex = 0;
		endItemIndex = _itemIdsAvailableToShow.Count;
		int num = (Math.Min(((Rectangle)(ref viewCullingArea)).Top, ((Rectangle)(ref rectangle)).Top) - ((Rectangle)(ref viewCullingArea)).Top) / 44;
		startY += -num * 44;
		startItemIndex += -num * _itemsPerLine;
		int num2 = (int)Math.Ceiling((float)viewCullingArea.Height / 44f) * _itemsPerLine;
		if (endItemIndex > num2 + startItemIndex + _itemsPerLine)
		{
			endItemIndex = num2 + startItemIndex + _itemsPerLine;
		}
	}

	public override void Recalculate()
	{
		base.Recalculate();
		UpdateSize();
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (base.IsMouseHovering)
		{
			Main.LocalPlayer.mouseInterface = true;
		}
	}

	public void SetContentsToShow(List<int> itemIdsToShow)
	{
		_itemIdsAvailableToShow.Clear();
		_itemIdsToLoadTexturesFor.Clear();
		_itemIdsAvailableToShow.AddRange(itemIdsToShow);
		_itemIdsToLoadTexturesFor.AddRange(itemIdsToShow);
		UpdateSize();
	}

	public int GetItemsPerLine()
	{
		return _itemsPerLine;
	}

	public override List<SnapPoint> GetSnapPoints()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		List<SnapPoint> list = new List<SnapPoint>();
		GetGridParameters(out var startX, out var startY, out var startItemIndex, out var endItemIndex);
		_ = _itemsPerLine;
		Rectangle viewCullingArea = base.Parent.GetViewCullingArea();
		int num = endItemIndex - startItemIndex;
		while (_dummySnapPoints.Count < num)
		{
			_dummySnapPoints.Add(new SnapPoint("CreativeInfinitesSlot", 0, Vector2.Zero, Vector2.Zero));
		}
		int num2 = 0;
		Vector2 vector = GetDimensions().Position();
		for (int i = startItemIndex; i < endItemIndex; i++)
		{
			Rectangle itemSlotHitbox = GetItemSlotHitbox(startX, startY, startItemIndex, i);
			Point center = ((Rectangle)(ref itemSlotHitbox)).Center;
			if (((Rectangle)(ref viewCullingArea)).Contains(center))
			{
				SnapPoint snapPoint = _dummySnapPoints[num2];
				snapPoint.ThisIsAHackThatChangesTheSnapPointsInfo(Vector2.Zero, center.ToVector2() - vector, num2);
				snapPoint.Calculate(this);
				num2++;
				list.Add(snapPoint);
			}
		}
		foreach (UIElement element in Elements)
		{
			list.AddRange(element.GetSnapPoints());
		}
		return list;
	}

	public void UpdateSize()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		int num = (_itemsPerLine = GetDimensions().ToRectangle().Width / 44);
		int num2 = (int)Math.Ceiling((float)_itemIdsAvailableToShow.Count / (float)num);
		MinHeight.Set(44 * num2, 0f);
	}
}
