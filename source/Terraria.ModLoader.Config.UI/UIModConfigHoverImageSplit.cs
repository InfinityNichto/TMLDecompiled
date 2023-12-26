using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;

namespace Terraria.ModLoader.Config.UI;

internal class UIModConfigHoverImageSplit : UIImage
{
	internal string HoverTextUp;

	internal string HoverTextDown;

	public UIModConfigHoverImageSplit(Asset<Texture2D> texture, string hoverTextUp, string hoverTextDown)
		: base(texture)
	{
		HoverTextUp = hoverTextUp;
		HoverTextDown = hoverTextDown;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		Rectangle r = GetDimensions().ToRectangle();
		if (base.IsMouseHovering)
		{
			if (Main.mouseY < r.Y + r.Height / 2)
			{
				UIModConfig.Tooltip = HoverTextUp;
			}
			else
			{
				UIModConfig.Tooltip = HoverTextDown;
			}
		}
	}
}
