using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIBestiaryBar : UIElement
{
	private class BestiaryBarItem
	{
		internal readonly string Tooltop;

		internal readonly int EntryCount;

		internal readonly int CompletedCount;

		internal readonly Color DrawColor;

		public BestiaryBarItem(string tooltop, int entryCount, int completedCount, Color drawColor)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			Tooltop = tooltop;
			EntryCount = entryCount;
			CompletedCount = completedCount;
			DrawColor = drawColor;
		}
	}

	private BestiaryDatabase _db;

	private List<BestiaryBarItem> _bestiaryBarItems;

	private readonly Color[] _colors = (Color[])(object)new Color[6]
	{
		new Color(232, 76, 61),
		new Color(155, 88, 181),
		new Color(27, 188, 155),
		new Color(243, 156, 17),
		new Color(45, 204, 112),
		new Color(241, 196, 15)
	};

	public UIBestiaryBar(BestiaryDatabase db)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		_db = db;
		_bestiaryBarItems = new List<BestiaryBarItem>();
		RecalculateBars();
	}

	public void RecalculateBars()
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		_bestiaryBarItems.Clear();
		int total = _db.Entries.Count;
		int totalCollected = _db.Entries.Count((BestiaryEntry e) => e.UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0);
		_bestiaryBarItems.Add(new BestiaryBarItem($"Total: {(float)totalCollected / (float)total * 100f:N2}% Collected", total, totalCollected, Main.OurFavoriteColor));
		List<BestiaryEntry> items = _db.GetBestiaryEntriesByMod(null);
		int collected = items.Count((BestiaryEntry oe) => oe.UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0);
		_bestiaryBarItems.Add(new BestiaryBarItem($"Terraria: {(float)collected / (float)items.Count * 100f:N2}% Collected", items.Count, collected, _colors[0]));
		for (int i = 1; i < ModLoader.Mods.Length; i++)
		{
			items = _db.GetBestiaryEntriesByMod(ModLoader.Mods[i]);
			if (items != null)
			{
				collected = items.Count((BestiaryEntry oe) => oe.UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0);
				_bestiaryBarItems.Add(new BestiaryBarItem($"{ModLoader.Mods[i].DisplayName}: {(float)collected / (float)items.Count * 100f:N2}% Collected", items.Count, collected, _colors[i % _colors.Length]));
			}
		}
	}

	protected override void DrawSelf(SpriteBatch sb)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		int xOffset = 0;
		Rectangle rectangle = GetDimensions().ToRectangle();
		rectangle.Height -= 3;
		bool drawHover = false;
		BestiaryBarItem hoverData = null;
		Rectangle drawArea = default(Rectangle);
		Rectangle outlineArea = default(Rectangle);
		for (int i = 1; i < _bestiaryBarItems.Count; i++)
		{
			BestiaryBarItem barData = _bestiaryBarItems[i];
			int offset = (int)((float)rectangle.Width * ((float)barData.EntryCount / (float)_db.Entries.Count));
			if (i == _bestiaryBarItems.Count - 1)
			{
				offset = rectangle.Width - xOffset;
			}
			int width = (int)((float)offset * ((float)barData.CompletedCount / (float)barData.EntryCount));
			((Rectangle)(ref drawArea))._002Ector(rectangle.X + xOffset, rectangle.Y, width, rectangle.Height);
			((Rectangle)(ref outlineArea))._002Ector(rectangle.X + xOffset, rectangle.Y, offset, rectangle.Height);
			xOffset += offset;
			sb.Draw(TextureAssets.MagicPixel.Value, outlineArea, barData.DrawColor * 0.3f);
			sb.Draw(TextureAssets.MagicPixel.Value, drawArea, barData.DrawColor);
			if (!drawHover && ((Rectangle)(ref outlineArea)).Contains(new Point(Main.mouseX, Main.mouseY)))
			{
				drawHover = true;
				hoverData = barData;
			}
		}
		BestiaryBarItem bottomData = _bestiaryBarItems[0];
		int bottomWidth = (int)((float)rectangle.Width * ((float)bottomData.CompletedCount / (float)bottomData.EntryCount));
		Rectangle bottomDrawArea = default(Rectangle);
		((Rectangle)(ref bottomDrawArea))._002Ector(rectangle.X, ((Rectangle)(ref rectangle)).Bottom, bottomWidth, 3);
		Rectangle bottomOutlineArea = default(Rectangle);
		((Rectangle)(ref bottomOutlineArea))._002Ector(rectangle.X, ((Rectangle)(ref rectangle)).Bottom, rectangle.Width, 3);
		sb.Draw(TextureAssets.MagicPixel.Value, bottomOutlineArea, bottomData.DrawColor * 0.3f);
		sb.Draw(TextureAssets.MagicPixel.Value, bottomDrawArea, bottomData.DrawColor);
		if (!drawHover && ((Rectangle)(ref bottomOutlineArea)).Contains(new Point(Main.mouseX, Main.mouseY)))
		{
			drawHover = true;
			hoverData = bottomData;
		}
		if (drawHover && hoverData != null)
		{
			Main.instance.MouseText(hoverData.Tooltop, 0, 0);
		}
	}
}
