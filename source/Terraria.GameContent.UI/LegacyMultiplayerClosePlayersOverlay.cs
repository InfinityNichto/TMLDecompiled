using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI;

public class LegacyMultiplayerClosePlayersOverlay : IMultiplayerClosePlayersOverlay
{
	public void Draw()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0457: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		int teamNamePlateDistance = Main.teamNamePlateDistance;
		if (teamNamePlateDistance <= 0)
		{
			return;
		}
		SpriteBatch spriteBatch = Main.spriteBatch;
		spriteBatch.End();
		spriteBatch.Begin((SpriteSortMode)1, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, Main.UIScaleMatrix);
		PlayerInput.SetZoom_World();
		int screenWidth = Main.screenWidth;
		int screenHeight = Main.screenHeight;
		Vector2 screenPosition = Main.screenPosition;
		PlayerInput.SetZoom_UI();
		float uIScale = Main.UIScale;
		int num = teamNamePlateDistance * 8;
		Player[] player = Main.player;
		int myPlayer = Main.myPlayer;
		SpriteViewMatrix gameViewMatrix = Main.GameViewMatrix;
		byte mouseTextColor = Main.mouseTextColor;
		Color[] teamColor = Main.teamColor;
		Camera camera = Main.Camera;
		IPlayerRenderer playerRenderer = Main.PlayerRenderer;
		Vector2 screenPosition2 = Main.screenPosition;
		Vector2 vector = default(Vector2);
		Color namePlateColor = default(Color);
		Vector2 position2 = default(Vector2);
		for (int i = 0; i < 255; i++)
		{
			if (!player[i].active || myPlayer == i || player[i].dead || player[myPlayer].team <= 0 || player[myPlayer].team != player[i].team)
			{
				continue;
			}
			string name = player[i].name;
			Vector2 namePlatePos = FontAssets.MouseText.Value.MeasureString(name);
			float num4 = 0f;
			if (player[i].chatOverhead.timeLeft > 0)
			{
				num4 = (0f - namePlatePos.Y) * uIScale;
			}
			else if (player[i].emoteTime > 0)
			{
				num4 = (0f - namePlatePos.Y) * uIScale;
			}
			((Vector2)(ref vector))._002Ector((float)(screenWidth / 2) + screenPosition.X, (float)(screenHeight / 2) + screenPosition.Y);
			Vector2 position = player[i].position;
			position += (position - vector) * (gameViewMatrix.Zoom - Vector2.One);
			float num5 = 0f;
			float num6 = (float)(int)mouseTextColor / 255f;
			((Color)(ref namePlateColor))._002Ector((int)(byte)((float)(int)((Color)(ref teamColor[player[i].team])).R * num6), (int)(byte)((float)(int)((Color)(ref teamColor[player[i].team])).G * num6), (int)(byte)((float)(int)((Color)(ref teamColor[player[i].team])).B * num6), (int)mouseTextColor);
			float num7 = position.X + (float)(player[i].width / 2) - vector.X;
			float num8 = position.Y - namePlatePos.Y - 2f + num4 - vector.Y;
			float num9 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
			int num10 = screenHeight;
			if (screenHeight > screenWidth)
			{
				num10 = screenWidth;
			}
			num10 = num10 / 2 - 50;
			if (num10 < 100)
			{
				num10 = 100;
			}
			if (num9 < (float)num10)
			{
				namePlatePos.X = position.X + (float)(player[i].width / 2) - namePlatePos.X / 2f - screenPosition.X;
				namePlatePos.Y = position.Y - namePlatePos.Y - 2f + num4 - screenPosition.Y;
			}
			else
			{
				num5 = num9;
				num9 = (float)num10 / num9;
				namePlatePos.X = (float)(screenWidth / 2) + num7 * num9 - namePlatePos.X / 2f;
				namePlatePos.Y = (float)(screenHeight / 2) + num8 * num9 + 40f * uIScale;
			}
			Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(name);
			namePlatePos += vector2 / 2f;
			namePlatePos *= 1f / uIScale;
			namePlatePos -= vector2 / 2f;
			if (player[myPlayer].gravDir == -1f)
			{
				namePlatePos.Y = (float)screenHeight - namePlatePos.Y;
			}
			if (num5 > 0f)
			{
				float num11 = 20f;
				float num2 = -27f;
				num2 -= (vector2.X - 85f) / 2f;
				num7 = player[i].Center.X - player[myPlayer].Center.X;
				num8 = player[i].Center.Y - player[myPlayer].Center.Y;
				float num3 = (float)Math.Sqrt(num7 * num7 + num8 * num8);
				if (!(num3 > (float)num))
				{
					string textValue = Language.GetTextValue("GameUI.PlayerDistance", (int)(num3 / 16f * 2f));
					Vector2 npDistPos = FontAssets.MouseText.Value.MeasureString(textValue);
					npDistPos.X = namePlatePos.X - num2;
					npDistPos.Y = namePlatePos.Y + vector2.Y / 2f - npDistPos.Y / 2f - num11;
					DrawPlayerName2(spriteBatch, ref namePlateColor, textValue, ref npDistPos);
					Color playerHeadBordersColor = Main.GetPlayerHeadBordersColor(player[i]);
					((Vector2)(ref position2))._002Ector(namePlatePos.X, namePlatePos.Y - num11);
					position2.X -= 22f + num2;
					position2.Y += 8f;
					playerRenderer.DrawPlayerHead(camera, player[i], position2, 1f, 0.8f, playerHeadBordersColor);
					Vector2 vector3 = npDistPos + screenPosition2 + new Vector2(26f, 20f);
					if (player[i].statLife != player[i].statLifeMax2)
					{
						Main.instance.DrawHealthBar(vector3.X, vector3.Y, player[i].statLife, player[i].statLifeMax2, 1f, 1.25f, noFlip: true);
					}
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, name, namePlatePos + new Vector2(0f, -40f), namePlateColor, 0f, Vector2.Zero, Vector2.One);
				}
			}
			else
			{
				DrawPlayerName(spriteBatch, name, ref namePlatePos, ref namePlateColor);
			}
		}
	}

	private static void DrawPlayerName2(SpriteBatch spriteBatch, ref Color namePlateColor, string npDist, ref Vector2 npDistPos)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		float scale = 0.85f;
		spriteBatch.DrawString(FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X - 2f, npDistPos.Y), Color.Black, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X + 2f, npDistPos.Y), Color.Black, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X, npDistPos.Y - 2f), Color.Black, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, npDist, new Vector2(npDistPos.X, npDistPos.Y + 2f), Color.Black, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, npDist, npDistPos, namePlateColor, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
	}

	private static void DrawPlayerName(SpriteBatch spriteBatch, string namePlate, ref Vector2 namePlatePos, ref Color namePlateColor)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.DrawString(FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X - 2f, namePlatePos.Y), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X + 2f, namePlatePos.Y), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X, namePlatePos.Y - 2f), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, namePlate, new Vector2(namePlatePos.X, namePlatePos.Y + 2f), Color.Black, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, namePlate, namePlatePos, namePlateColor, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
	}
}
