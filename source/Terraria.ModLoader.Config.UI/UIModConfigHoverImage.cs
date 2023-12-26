using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;

namespace Terraria.ModLoader.Config.UI;

internal class UIModConfigHoverImage : UIImage
{
	internal string HoverText;

	public UIModConfigHoverImage(Asset<Texture2D> texture, string hoverText)
		: base(texture)
	{
		HoverText = hoverText;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		base.DrawSelf(spriteBatch);
		if (base.IsMouseHovering)
		{
			UIModConfig.Tooltip = HoverText;
		}
	}
}
