using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIColoredSliderSimple : UIElement
{
	public float FillPercent;

	public Color FilledColor = Main.OurFavoriteColor;

	public Color EmptyColor = Color.Black;

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		DrawValueBarDynamicWidth(spriteBatch);
	}

	private void DrawValueBarDynamicWidth(SpriteBatch sb)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value = TextureAssets.ColorBar.Value;
		Rectangle rectangle = GetDimensions().ToRectangle();
		Rectangle rectangle2 = default(Rectangle);
		((Rectangle)(ref rectangle2))._002Ector(5, 4, 4, 4);
		Utils.DrawSplicedPanel(sb, value, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, rectangle2.X, rectangle2.Width, rectangle2.Y, rectangle2.Height, Color.White);
		Rectangle rectangle3 = rectangle;
		rectangle3.X += ((Rectangle)(ref rectangle2)).Left;
		rectangle3.Width -= ((Rectangle)(ref rectangle2)).Right;
		rectangle3.Y += ((Rectangle)(ref rectangle2)).Top;
		rectangle3.Height -= ((Rectangle)(ref rectangle2)).Bottom;
		Texture2D value2 = TextureAssets.MagicPixel.Value;
		Rectangle value3 = default(Rectangle);
		((Rectangle)(ref value3))._002Ector(0, 0, 1, 1);
		sb.Draw(value2, rectangle3, (Rectangle?)value3, EmptyColor);
		Rectangle destinationRectangle = rectangle3;
		destinationRectangle.Width = (int)((float)destinationRectangle.Width * FillPercent);
		sb.Draw(value2, destinationRectangle, (Rectangle?)value3, FilledColor);
	}
}
