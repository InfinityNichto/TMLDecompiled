using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terraria.GameContent.UI.ResourceSets;

public class ClassicPlayerResourcesDisplaySet : IPlayerResourcesDisplaySet, IConfigKeyHolder
{
	private float UIDisplay_ManaPerStar = 20f;

	private float UIDisplay_LifePerHeart = 20f;

	private int UI_ScreenAnchorX;

	public string DisplayedName => Language.GetTextValue("UI.HealthManaStyle_" + NameKey);

	public string NameKey { get; private set; }

	public string ConfigKey { get; private set; }

	public ClassicPlayerResourcesDisplaySet(string nameKey, string configKey)
	{
		NameKey = nameKey;
		ConfigKey = configKey;
	}

	public void Draw()
	{
		UI_ScreenAnchorX = Main.screenWidth - 800;
		DrawLife();
		DrawMana();
	}

	private void DrawLife()
	{
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Main.LocalPlayer;
		SpriteBatch spriteBatch = Main.spriteBatch;
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		UIDisplay_LifePerHeart = 20f;
		PlayerStatsSnapshot snapshot = new PlayerStatsSnapshot(localPlayer);
		if (localPlayer.ghost || localPlayer.statLifeMax2 <= 0 || snapshot.AmountOfLifeHearts <= 0)
		{
			return;
		}
		UIDisplay_LifePerHeart = snapshot.LifePerSegment;
		int num2 = snapshot.LifeFruitCount;
		bool drawText;
		bool drawHearts = ResourceOverlayLoader.PreDrawResourceDisplay(snapshot, this, drawingLife: true, ref color, out drawText);
		if (drawText)
		{
			int num3 = (int)((float)localPlayer.statLifeMax2 / UIDisplay_LifePerHeart);
			if (num3 >= 10)
			{
				num3 = 10;
			}
			string text = Lang.inter[0].Value + " " + localPlayer.statLifeMax2 + "/" + localPlayer.statLifeMax2;
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
			if (!localPlayer.ghost)
			{
				spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[0].Value, new Vector2((float)(500 + 13 * num3) - vector.X * 0.5f + (float)UI_ScreenAnchorX, 6f), color, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
				spriteBatch.DrawString(FontAssets.MouseText.Value, localPlayer.statLife + "/" + localPlayer.statLifeMax2, new Vector2((float)(500 + 13 * num3) + vector.X * 0.5f + (float)UI_ScreenAnchorX, 6f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, (SpriteEffects)0, 0f);
			}
		}
		if (drawHearts)
		{
			Vector2 position = default(Vector2);
			for (int i = 1; i < (int)((float)localPlayer.statLifeMax2 / UIDisplay_LifePerHeart) + 1; i++)
			{
				int num4 = 255;
				float num5 = 1f;
				bool flag = false;
				if ((float)localPlayer.statLife >= (float)i * UIDisplay_LifePerHeart)
				{
					num4 = 255;
					if ((float)localPlayer.statLife == (float)i * UIDisplay_LifePerHeart)
					{
						flag = true;
					}
				}
				else
				{
					float num6 = ((float)localPlayer.statLife - (float)(i - 1) * UIDisplay_LifePerHeart) / UIDisplay_LifePerHeart;
					num4 = (int)(30f + 225f * num6);
					if (num4 < 30)
					{
						num4 = 30;
					}
					num5 = num6 / 4f + 0.75f;
					if ((double)num5 < 0.75)
					{
						num5 = 0.75f;
					}
					if (num6 > 0f)
					{
						flag = true;
					}
				}
				if (flag)
				{
					num5 += Main.cursorScale - 1f;
				}
				int num7 = 0;
				int num8 = 0;
				if (i > 10)
				{
					num7 -= 260;
					num8 += 26;
				}
				int a = (int)((double)num4 * 0.9);
				if (!localPlayer.ghost)
				{
					Asset<Texture2D> heartTexture = ((num2 > 0) ? TextureAssets.Heart2 : TextureAssets.Heart);
					if (num2 > 0)
					{
						num2--;
					}
					((Vector2)(ref position))._002Ector((float)(500 + 26 * (i - 1) + num7 + UI_ScreenAnchorX + heartTexture.Width() / 2), 32f + (float)heartTexture.Height() * (1f - num5) / 2f + (float)num8 + (float)(heartTexture.Height() / 2));
					ResourceOverlayDrawContext drawContext = new ResourceOverlayDrawContext(snapshot, this, i - 1, heartTexture);
					drawContext.position = position;
					drawContext.color = new Color(num4, num4, num4, a);
					drawContext.origin = heartTexture.Size() / 2f;
					drawContext.scale = new Vector2(num5);
					ResourceOverlayLoader.DrawResource(drawContext);
				}
			}
		}
		ResourceOverlayLoader.PostDrawResourceDisplay(snapshot, this, drawingLife: true, color, drawText);
	}

	private void DrawMana()
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Main.LocalPlayer;
		SpriteBatch spriteBatch = Main.spriteBatch;
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		UIDisplay_ManaPerStar = 20f;
		PlayerStatsSnapshot snapshot = new PlayerStatsSnapshot(localPlayer);
		if (localPlayer.ghost || localPlayer.statManaMax2 <= 0 || snapshot.AmountOfManaStars <= 0)
		{
			return;
		}
		UIDisplay_ManaPerStar = snapshot.ManaPerSegment;
		bool drawText;
		bool num5 = ResourceOverlayLoader.PreDrawResourceDisplay(snapshot, this, drawingLife: false, ref color, out drawText);
		if (drawText)
		{
			_ = localPlayer.statManaMax2 / 20;
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(Lang.inter[2].Value);
			int num = 50;
			if (vector.X >= 45f)
			{
				num = (int)vector.X + 5;
			}
			spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[2].Value, new Vector2((float)(800 - num + UI_ScreenAnchorX), 6f), color, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		}
		if (num5)
		{
			Vector2 position = default(Vector2);
			for (int i = 1; (float)i < (float)localPlayer.statManaMax2 / UIDisplay_ManaPerStar + 1f; i++)
			{
				int num2 = 255;
				bool flag = false;
				float num3 = 1f;
				if ((float)localPlayer.statMana >= (float)i * UIDisplay_ManaPerStar)
				{
					num2 = 255;
					if ((float)localPlayer.statMana == (float)i * UIDisplay_ManaPerStar)
					{
						flag = true;
					}
				}
				else
				{
					float num4 = ((float)localPlayer.statMana - (float)(i - 1) * UIDisplay_ManaPerStar) / UIDisplay_ManaPerStar;
					num2 = (int)(30f + 225f * num4);
					if (num2 < 30)
					{
						num2 = 30;
					}
					num3 = num4 / 4f + 0.75f;
					if ((double)num3 < 0.75)
					{
						num3 = 0.75f;
					}
					if (num4 > 0f)
					{
						flag = true;
					}
				}
				if (flag)
				{
					num3 += Main.cursorScale - 1f;
				}
				((Vector2)(ref position))._002Ector((float)(775 + UI_ScreenAnchorX), (float)(30 + TextureAssets.Mana.Height() / 2) + (float)TextureAssets.Mana.Height() * (1f - num3) / 2f + (float)(28 * (i - 1)));
				int a = (int)((double)num2 * 0.9);
				ResourceOverlayDrawContext drawContext = new ResourceOverlayDrawContext(snapshot, this, i - 1, TextureAssets.Mana);
				drawContext.position = position;
				drawContext.color = new Color(num2, num2, num2, a);
				drawContext.origin = TextureAssets.Mana.Size() / 2f;
				drawContext.scale = new Vector2(num3);
				ResourceOverlayLoader.DrawResource(drawContext);
			}
		}
		ResourceOverlayLoader.PostDrawResourceDisplay(snapshot, this, drawingLife: false, color, drawText);
	}

	public void TryToHover()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		Vector2 mouseScreen = Main.MouseScreen;
		Player localPlayer = Main.LocalPlayer;
		PlayerStatsSnapshot snapshot = new PlayerStatsSnapshot(localPlayer);
		int num = 26 * snapshot.AmountOfLifeHearts;
		float num2 = 0f;
		if (snapshot.AmountOfLifeHearts > 10)
		{
			num = 260;
			num2 += 26f;
		}
		if (mouseScreen.X > (float)(500 + UI_ScreenAnchorX) && mouseScreen.X < (float)(500 + num + UI_ScreenAnchorX) && mouseScreen.Y > 32f && mouseScreen.Y < (float)(32 + TextureAssets.Heart.Height()) + num2 && ResourceOverlayLoader.DisplayHoverText(snapshot, this, drawingLife: true))
		{
			CommonResourceBarMethods.DrawLifeMouseOver();
		}
		num = 24;
		num2 = 28 * snapshot.AmountOfManaStars;
		if (mouseScreen.X > (float)(762 + UI_ScreenAnchorX) && mouseScreen.X < (float)(762 + num + UI_ScreenAnchorX) && mouseScreen.Y > 30f && mouseScreen.Y < 30f + num2 && ResourceOverlayLoader.DisplayHoverText(snapshot, this, drawingLife: false))
		{
			CommonResourceBarMethods.DrawManaMouseOver();
		}
	}
}
