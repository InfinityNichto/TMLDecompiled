using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIItemIcon : UIElement
{
	private Item _item;

	private bool _blackedOut;

	public UIItemIcon(Item item, bool blackedOut)
	{
		_item = item;
		Width.Set(32f, 0f);
		Height.Set(32f, 0f);
		_blackedOut = blackedOut;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Vector2 screenPositionForItemCenter = GetDimensions().Center();
		ItemSlot.DrawItemIcon(_item, 31, spriteBatch, screenPositionForItemCenter, _item.scale, 32f, _blackedOut ? Color.Black : Color.White);
	}
}
