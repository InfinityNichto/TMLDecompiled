using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.Events;

public class ScreenDarkness
{
	public static float screenObstruction;

	public static Color frontColor = new Color(0, 0, 120);

	public static void Update()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		float value = 0f;
		float amount = 1f / 60f;
		Vector2 mountedCenter = Main.player[Main.myPlayer].MountedCenter;
		for (int i = 0; i < 200; i++)
		{
			if (Main.npc[i].active && Main.npc[i].type == 370 && Main.npc[i].Distance(mountedCenter) < 3000f && (Main.npc[i].ai[0] >= 10f || (Main.npc[i].ai[0] == 9f && Main.npc[i].ai[2] > 120f)))
			{
				value = 0.95f;
				frontColor = new Color(0, 0, 120) * 0.3f;
				amount = 0.03f;
			}
			if (Main.npc[i].active && Main.npc[i].type == 113 && Main.npc[i].Distance(mountedCenter) < 3000f)
			{
				float num = Utils.Remap(Main.npc[i].Distance(mountedCenter), 2000f, 3000f, 1f, 0f);
				value = Main.npc[i].localAI[1] * num;
				amount = 1f;
				frontColor = Color.Black;
			}
		}
		screenObstruction = MathHelper.Lerp(screenObstruction, value, amount);
	}

	public static void DrawBack(SpriteBatch spriteBatch)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (screenObstruction != 0f)
		{
			Color color = Color.Black * screenObstruction;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), (Rectangle?)new Rectangle(0, 0, 1, 1), color);
		}
	}

	public static void DrawFront(SpriteBatch spriteBatch)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (screenObstruction != 0f)
		{
			Color color = frontColor * screenObstruction;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), (Rectangle?)new Rectangle(0, 0, 1, 1), color);
		}
	}
}
