using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;

namespace Terraria.ModLoader.UI;

internal class UIHoverImage : UIImage
{
	internal string HoverText;

	public UIHoverImage(Asset<Texture2D> texture, string hoverText)
		: base(texture)
	{
		HoverText = hoverText;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		if (base.IsMouseHovering)
		{
			Rectangle bounds = base.Parent.GetDimensions().ToRectangle();
			bounds.Y = 0;
			bounds.Height = Main.screenHeight;
			UICommon.DrawHoverStringInBounds(spriteBatch, HoverText, bounds);
		}
	}
}
