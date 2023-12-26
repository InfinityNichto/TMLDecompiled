using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Events;

public class ScreenObstruction
{
	public static float screenObstruction;

	public static void Update()
	{
		float value = 0f;
		float amount = 0.1f;
		if (Main.player[Main.myPlayer].headcovered)
		{
			value = 0.95f;
			amount = 0.3f;
		}
		screenObstruction = MathHelper.Lerp(screenObstruction, value, amount);
	}

	public static void Draw(SpriteBatch spriteBatch)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		if (screenObstruction != 0f)
		{
			Color color = Color.Black * screenObstruction;
			int num = TextureAssets.Extra[49].Width();
			int num2 = 10;
			Rectangle rect = Main.player[Main.myPlayer].getRect();
			((Rectangle)(ref rect)).Inflate((num - rect.Width) / 2, (num - rect.Height) / 2 + num2 / 2);
			((Rectangle)(ref rect)).Offset(-(int)Main.screenPosition.X, -(int)Main.screenPosition.Y + (int)Main.player[Main.myPlayer].gfxOffY - num2);
			Rectangle destinationRectangle = Rectangle.Union(new Rectangle(0, 0, 1, 1), new Rectangle(((Rectangle)(ref rect)).Right - 1, ((Rectangle)(ref rect)).Top - 1, 1, 1));
			Rectangle destinationRectangle2 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, 0, 1, 1), new Rectangle(((Rectangle)(ref rect)).Right, ((Rectangle)(ref rect)).Bottom - 1, 1, 1));
			Rectangle destinationRectangle3 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, Main.screenHeight - 1, 1, 1), new Rectangle(((Rectangle)(ref rect)).Left, ((Rectangle)(ref rect)).Bottom, 1, 1));
			Rectangle destinationRectangle4 = Rectangle.Union(new Rectangle(0, Main.screenHeight - 1, 1, 1), new Rectangle(((Rectangle)(ref rect)).Left - 1, ((Rectangle)(ref rect)).Top, 1, 1));
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle, (Rectangle?)new Rectangle(0, 0, 1, 1), color);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle2, (Rectangle?)new Rectangle(0, 0, 1, 1), color);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle3, (Rectangle?)new Rectangle(0, 0, 1, 1), color);
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle4, (Rectangle?)new Rectangle(0, 0, 1, 1), color);
			spriteBatch.Draw(TextureAssets.Extra[49].Value, rect, color);
		}
	}
}
