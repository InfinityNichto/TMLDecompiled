using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.Graphics.Capture;

public class CaptureInterface
{
	public static class Settings
	{
		public static bool PackImage = true;

		public static bool IncludeEntities = true;

		public static bool TransparentBackground;

		public static int BiomeChoiceIndex = -1;

		public static int ScreenAnchor = 0;

		public static Color MarkedAreaColor = new Color(0.8f, 0.8f, 0.8f, 0f) * 0.3f;
	}

	private abstract class CaptureInterfaceMode
	{
		public bool Selected;

		public abstract void Update();

		public abstract void Draw(SpriteBatch sb);

		public abstract void ToggleActive(bool tickedOn);

		public abstract bool UsingMap();
	}

	private class ModeEdgeSelection : CaptureInterfaceMode
	{
		public override void Update()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (Selected)
			{
				PlayerInput.SetZoom_Context();
				Vector2 mouse = default(Vector2);
				((Vector2)(ref mouse))._002Ector((float)Main.mouseX, (float)Main.mouseY);
				EdgePlacement(mouse);
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			if (Selected)
			{
				sb.End();
				sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.CurrentWantedZoomMatrix);
				PlayerInput.SetZoom_Context();
				DrawMarkedArea(sb);
				DrawCursors(sb);
				sb.End();
				sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
				PlayerInput.SetZoom_UI();
			}
		}

		public override void ToggleActive(bool tickedOn)
		{
		}

		public override bool UsingMap()
		{
			return true;
		}

		private void EdgePlacement(Vector2 mouse)
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			if (JustActivated)
			{
				return;
			}
			Point result;
			if (!Main.mapFullscreen)
			{
				if (Main.mouseLeft)
				{
					EdgeAPinned = true;
					EdgeA = Main.MouseWorld.ToTileCoordinates();
				}
				if (Main.mouseRight)
				{
					EdgeBPinned = true;
					EdgeB = Main.MouseWorld.ToTileCoordinates();
				}
			}
			else if (GetMapCoords((int)mouse.X, (int)mouse.Y, 0, out result))
			{
				if (Main.mouseLeft)
				{
					EdgeAPinned = true;
					EdgeA = result;
				}
				if (Main.mouseRight)
				{
					EdgeBPinned = true;
					EdgeB = result;
				}
			}
			ConstraintPoints();
		}

		private void DrawMarkedArea(SpriteBatch sb)
		{
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			if (!EdgeAPinned || !EdgeBPinned)
			{
				return;
			}
			int num = Math.Min(EdgeA.X, EdgeB.X);
			int num2 = Math.Min(EdgeA.Y, EdgeB.Y);
			int num3 = Math.Abs(EdgeA.X - EdgeB.X);
			int num4 = Math.Abs(EdgeA.Y - EdgeB.Y);
			if (!Main.mapFullscreen)
			{
				Rectangle value = Main.ReverseGravitySupport(new Rectangle(num * 16, num2 * 16, (num3 + 1) * 16, (num4 + 1) * 16));
				Rectangle value2 = Main.ReverseGravitySupport(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth + 1, Main.screenHeight + 1));
				Rectangle result = default(Rectangle);
				Rectangle.Intersect(ref value2, ref value, ref result);
				if (result.Width != 0 && result.Height != 0)
				{
					((Rectangle)(ref result)).Offset(-value2.X, -value2.Y);
					sb.Draw(TextureAssets.MagicPixel.Value, result, Settings.MarkedAreaColor);
					for (int i = 0; i < 2; i++)
					{
						sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(result.X, result.Y + ((i == 1) ? result.Height : (-2)), result.Width, 2), Color.White);
						sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(result.X + ((i == 1) ? result.Width : (-2)), result.Y, 2, result.Height), Color.White);
					}
				}
				return;
			}
			GetMapCoords(num, num2, 1, out var result2);
			GetMapCoords(num + num3 + 1, num2 + num4 + 1, 1, out var result3);
			Rectangle value3 = default(Rectangle);
			((Rectangle)(ref value3))._002Ector(result2.X, result2.Y, result3.X - result2.X, result3.Y - result2.Y);
			Rectangle value4 = default(Rectangle);
			((Rectangle)(ref value4))._002Ector(0, 0, Main.screenWidth + 1, Main.screenHeight + 1);
			Rectangle result4 = default(Rectangle);
			Rectangle.Intersect(ref value4, ref value3, ref result4);
			if (result4.Width != 0 && result4.Height != 0)
			{
				((Rectangle)(ref result4)).Offset(-value4.X, -value4.Y);
				sb.Draw(TextureAssets.MagicPixel.Value, result4, Settings.MarkedAreaColor);
				for (int j = 0; j < 2; j++)
				{
					sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(result4.X, result4.Y + ((j == 1) ? result4.Height : (-2)), result4.Width, 2), Color.White);
					sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(result4.X + ((j == 1) ? result4.Width : (-2)), result4.Y, 2, result4.Height), Color.White);
				}
			}
		}

		private void DrawCursors(SpriteBatch sb)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0479: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0402: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_062e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0633: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0494: Unknown result type (might be due to invalid IL or missing references)
			//IL_0499: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0717: Unknown result type (might be due to invalid IL or missing references)
			//IL_071e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0700: Unknown result type (might be due to invalid IL or missing references)
			//IL_0702: Unknown result type (might be due to invalid IL or missing references)
			//IL_0707: Unknown result type (might be due to invalid IL or missing references)
			//IL_0709: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_0585: Unknown result type (might be due to invalid IL or missing references)
			//IL_0587: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Unknown result type (might be due to invalid IL or missing references)
			//IL_058e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0590: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_0592: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_0599: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05be: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04be: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_072f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0740: Unknown result type (might be due to invalid IL or missing references)
			//IL_0742: Unknown result type (might be due to invalid IL or missing references)
			//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05de: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_050e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0515: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_060f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_0616: Unknown result type (might be due to invalid IL or missing references)
			//IL_0620: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Unknown result type (might be due to invalid IL or missing references)
			//IL_0525: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Unknown result type (might be due to invalid IL or missing references)
			float num = 1f / Main.cursorScale;
			float num2 = 0.8f / num;
			Vector2 vector = Main.screenPosition + new Vector2(30f);
			Vector2 max = vector + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) - new Vector2(60f);
			if (Main.mapFullscreen)
			{
				vector -= Main.screenPosition;
				max -= Main.screenPosition;
			}
			Vector3 vector2 = Main.rgbToHsl(Main.cursorColor);
			Color color = Main.hslToRgb((vector2.X + 0.33f) % 1f, vector2.Y, vector2.Z);
			Color color2 = Main.hslToRgb((vector2.X - 0.33f) % 1f, vector2.Y, vector2.Z);
			color = (color2 = Color.White);
			bool flag = Main.player[Main.myPlayer].gravDir == -1f;
			if (!EdgeAPinned)
			{
				Utils.DrawCursorSingle(sb, color, 3.926991f, Main.cursorScale * num * num2, new Vector2((float)Main.mouseX - 5f + 12f, (float)Main.mouseY + 2.5f + 12f), 4);
			}
			else
			{
				int specialMode = 0;
				float num3 = 0f;
				Vector2 zero = Vector2.Zero;
				if (!Main.mapFullscreen)
				{
					Vector2 vector3 = EdgeA.ToVector2() * 16f;
					if (!EdgeBPinned)
					{
						specialMode = 1;
						vector3 += Vector2.One * 8f;
						zero = vector3;
						num3 = (-vector3 + Main.ReverseGravitySupport(new Vector2((float)Main.mouseX, (float)Main.mouseY), 0f) + Main.screenPosition).ToRotation();
						if (flag)
						{
							num3 = 0f - num3;
						}
						zero = Vector2.Clamp(vector3, vector, max);
						if (zero != vector3)
						{
							num3 = (vector3 - zero).ToRotation();
						}
					}
					else
					{
						Vector2 vector4 = default(Vector2);
						((Vector2)(ref vector4))._002Ector((float)((EdgeA.X > EdgeB.X).ToInt() * 16), (float)((EdgeA.Y > EdgeB.Y).ToInt() * 16));
						vector3 += vector4;
						zero = Vector2.Clamp(vector3, vector, max);
						num3 = (EdgeB.ToVector2() * 16f + new Vector2(16f) - vector4 - zero).ToRotation();
						if (zero != vector3)
						{
							num3 = (vector3 - zero).ToRotation();
							specialMode = 1;
						}
						if (flag)
						{
							num3 *= -1f;
						}
					}
					Utils.DrawCursorSingle(sb, color, num3 - (float)Math.PI / 2f, Main.cursorScale * num, Main.ReverseGravitySupport(zero - Main.screenPosition), 4, specialMode);
				}
				else
				{
					Point result = EdgeA;
					if (EdgeBPinned)
					{
						int num4 = (EdgeA.X > EdgeB.X).ToInt();
						int num5 = (EdgeA.Y > EdgeB.Y).ToInt();
						result.X += num4;
						result.Y += num5;
						GetMapCoords(result.X, result.Y, 1, out result);
						Point result2 = EdgeB;
						result2.X += 1 - num4;
						result2.Y += 1 - num5;
						GetMapCoords(result2.X, result2.Y, 1, out result2);
						zero = result.ToVector2();
						zero = Vector2.Clamp(zero, vector, max);
						num3 = (result2.ToVector2() - zero).ToRotation();
					}
					else
					{
						GetMapCoords(result.X, result.Y, 1, out result);
					}
					Utils.DrawCursorSingle(sb, color, num3 - (float)Math.PI / 2f, Main.cursorScale * num, result.ToVector2(), 4);
				}
			}
			if (!EdgeBPinned)
			{
				Utils.DrawCursorSingle(sb, color2, 0.7853981f, Main.cursorScale * num * num2, new Vector2((float)Main.mouseX + 2.5f + 12f, (float)Main.mouseY - 5f + 12f), 5);
				return;
			}
			int specialMode2 = 0;
			float num6 = 0f;
			Vector2 zero2 = Vector2.Zero;
			if (!Main.mapFullscreen)
			{
				Vector2 vector5 = EdgeB.ToVector2() * 16f;
				if (!EdgeAPinned)
				{
					specialMode2 = 1;
					vector5 += Vector2.One * 8f;
					zero2 = vector5;
					num6 = (-vector5 + Main.ReverseGravitySupport(new Vector2((float)Main.mouseX, (float)Main.mouseY), 0f) + Main.screenPosition).ToRotation();
					if (flag)
					{
						num6 = 0f - num6;
					}
					zero2 = Vector2.Clamp(vector5, vector, max);
					if (zero2 != vector5)
					{
						num6 = (vector5 - zero2).ToRotation();
					}
				}
				else
				{
					Vector2 vector6 = default(Vector2);
					((Vector2)(ref vector6))._002Ector((float)((EdgeB.X >= EdgeA.X).ToInt() * 16), (float)((EdgeB.Y >= EdgeA.Y).ToInt() * 16));
					vector5 += vector6;
					zero2 = Vector2.Clamp(vector5, vector, max);
					num6 = (EdgeA.ToVector2() * 16f + new Vector2(16f) - vector6 - zero2).ToRotation();
					if (zero2 != vector5)
					{
						num6 = (vector5 - zero2).ToRotation();
						specialMode2 = 1;
					}
					if (flag)
					{
						num6 *= -1f;
					}
				}
				Utils.DrawCursorSingle(sb, color2, num6 - (float)Math.PI / 2f, Main.cursorScale * num, Main.ReverseGravitySupport(zero2 - Main.screenPosition), 5, specialMode2);
			}
			else
			{
				Point result3 = EdgeB;
				if (EdgeAPinned)
				{
					int num7 = (EdgeB.X >= EdgeA.X).ToInt();
					int num8 = (EdgeB.Y >= EdgeA.Y).ToInt();
					result3.X += num7;
					result3.Y += num8;
					GetMapCoords(result3.X, result3.Y, 1, out result3);
					Point result4 = EdgeA;
					result4.X += 1 - num7;
					result4.Y += 1 - num8;
					GetMapCoords(result4.X, result4.Y, 1, out result4);
					zero2 = result3.ToVector2();
					zero2 = Vector2.Clamp(zero2, vector, max);
					num6 = (result4.ToVector2() - zero2).ToRotation();
				}
				else
				{
					GetMapCoords(result3.X, result3.Y, 1, out result3);
				}
				Utils.DrawCursorSingle(sb, color2, num6 - (float)Math.PI / 2f, Main.cursorScale * num, result3.ToVector2(), 5);
			}
		}
	}

	private class ModeDragBounds : CaptureInterfaceMode
	{
		public int currentAim = -1;

		private bool dragging;

		private int caughtEdge = -1;

		private bool inMap;

		public override void Update()
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			if (Selected && !JustActivated)
			{
				PlayerInput.SetZoom_Context();
				Vector2 mouse = default(Vector2);
				((Vector2)(ref mouse))._002Ector((float)Main.mouseX, (float)Main.mouseY);
				DragBounds(mouse);
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (Selected)
			{
				sb.End();
				sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.CurrentWantedZoomMatrix);
				PlayerInput.SetZoom_Context();
				DrawMarkedArea(sb);
				sb.End();
				sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
				PlayerInput.SetZoom_UI();
			}
		}

		public override void ToggleActive(bool tickedOn)
		{
			if (!tickedOn)
			{
				currentAim = -1;
			}
		}

		public override bool UsingMap()
		{
			return caughtEdge != -1;
		}

		private void DragBounds(Vector2 mouse)
		{
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0360: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0389: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			if (!EdgeAPinned || !EdgeBPinned)
			{
				bool flag = false;
				if (Main.mouseLeft)
				{
					flag = true;
				}
				if (flag)
				{
					bool flag2 = true;
					Point result;
					if (!Main.mapFullscreen)
					{
						result = (Main.screenPosition + mouse).ToTileCoordinates();
					}
					else
					{
						flag2 = GetMapCoords((int)mouse.X, (int)mouse.Y, 0, out result);
					}
					if (flag2)
					{
						if (!EdgeAPinned)
						{
							EdgeAPinned = true;
							EdgeA = result;
						}
						if (!EdgeBPinned)
						{
							EdgeBPinned = true;
							EdgeB = result;
						}
					}
					currentAim = 3;
					caughtEdge = 1;
				}
			}
			int num = Math.Min(EdgeA.X, EdgeB.X);
			int num2 = Math.Min(EdgeA.Y, EdgeB.Y);
			int num3 = Math.Abs(EdgeA.X - EdgeB.X);
			int num4 = Math.Abs(EdgeA.Y - EdgeB.Y);
			bool value = Main.player[Main.myPlayer].gravDir == -1f;
			int num5 = 1 - value.ToInt();
			int num6 = value.ToInt();
			Rectangle value2 = default(Rectangle);
			Rectangle value3 = default(Rectangle);
			Rectangle result2 = default(Rectangle);
			if (!Main.mapFullscreen)
			{
				value2 = Main.ReverseGravitySupport(new Rectangle(num * 16, num2 * 16, (num3 + 1) * 16, (num4 + 1) * 16));
				value3 = Main.ReverseGravitySupport(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth + 1, Main.screenHeight + 1));
				Rectangle.Intersect(ref value3, ref value2, ref result2);
				if (result2.Width == 0 || result2.Height == 0)
				{
					return;
				}
				((Rectangle)(ref result2)).Offset(-value3.X, -value3.Y);
			}
			else
			{
				GetMapCoords(num, num2, 1, out var result3);
				GetMapCoords(num + num3 + 1, num2 + num4 + 1, 1, out var result4);
				((Rectangle)(ref value2))._002Ector(result3.X, result3.Y, result4.X - result3.X, result4.Y - result3.Y);
				((Rectangle)(ref value3))._002Ector(0, 0, Main.screenWidth + 1, Main.screenHeight + 1);
				Rectangle.Intersect(ref value3, ref value2, ref result2);
				if (result2.Width == 0 || result2.Height == 0)
				{
					return;
				}
				((Rectangle)(ref result2)).Offset(-value3.X, -value3.Y);
			}
			dragging = false;
			if (!Main.mouseLeft)
			{
				currentAim = -1;
			}
			if (currentAim != -1)
			{
				dragging = true;
				Point point = default(Point);
				if (!Main.mapFullscreen)
				{
					point = Main.MouseWorld.ToTileCoordinates();
				}
				else
				{
					if (!GetMapCoords((int)mouse.X, (int)mouse.Y, 0, out var result5))
					{
						return;
					}
					point = result5;
				}
				switch (currentAim)
				{
				case 0:
				case 1:
					if (caughtEdge == 0)
					{
						EdgeA.Y = point.Y;
					}
					if (caughtEdge == 1)
					{
						EdgeB.Y = point.Y;
					}
					break;
				case 2:
				case 3:
					if (caughtEdge == 0)
					{
						EdgeA.X = point.X;
					}
					if (caughtEdge == 1)
					{
						EdgeB.X = point.X;
					}
					break;
				}
			}
			else
			{
				caughtEdge = -1;
				Rectangle drawbox = value2;
				((Rectangle)(ref drawbox)).Offset(-value3.X, -value3.Y);
				inMap = ((Rectangle)(ref drawbox)).Contains(mouse.ToPoint());
				for (int i = 0; i < 4; i++)
				{
					Rectangle bound = GetBound(drawbox, i);
					((Rectangle)(ref bound)).Inflate(8, 8);
					if (!((Rectangle)(ref bound)).Contains(mouse.ToPoint()))
					{
						continue;
					}
					currentAim = i;
					switch (i)
					{
					case 0:
						if (EdgeA.Y < EdgeB.Y)
						{
							caughtEdge = num6;
						}
						else
						{
							caughtEdge = num5;
						}
						break;
					case 1:
						if (EdgeA.Y >= EdgeB.Y)
						{
							caughtEdge = num6;
						}
						else
						{
							caughtEdge = num5;
						}
						break;
					case 2:
						if (EdgeA.X < EdgeB.X)
						{
							caughtEdge = 0;
						}
						else
						{
							caughtEdge = 1;
						}
						break;
					case 3:
						if (EdgeA.X >= EdgeB.X)
						{
							caughtEdge = 0;
						}
						else
						{
							caughtEdge = 1;
						}
						break;
					}
					break;
				}
			}
			ConstraintPoints();
		}

		private Rectangle GetBound(Rectangle drawbox, int boundIndex)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			return (Rectangle)(boundIndex switch
			{
				0 => new Rectangle(drawbox.X, drawbox.Y - 2, drawbox.Width, 2), 
				1 => new Rectangle(drawbox.X, drawbox.Y + drawbox.Height, drawbox.Width, 2), 
				2 => new Rectangle(drawbox.X - 2, drawbox.Y, 2, drawbox.Height), 
				3 => new Rectangle(drawbox.X + drawbox.Width, drawbox.Y, 2, drawbox.Height), 
				_ => Rectangle.Empty, 
			});
		}

		public void DrawMarkedArea(SpriteBatch sb)
		{
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0282: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			if (!EdgeAPinned || !EdgeBPinned)
			{
				return;
			}
			int num = Math.Min(EdgeA.X, EdgeB.X);
			int num2 = Math.Min(EdgeA.Y, EdgeB.Y);
			int num3 = Math.Abs(EdgeA.X - EdgeB.X);
			int num4 = Math.Abs(EdgeA.Y - EdgeB.Y);
			Rectangle result = default(Rectangle);
			if (!Main.mapFullscreen)
			{
				Rectangle value = Main.ReverseGravitySupport(new Rectangle(num * 16, num2 * 16, (num3 + 1) * 16, (num4 + 1) * 16));
				Rectangle value3 = Main.ReverseGravitySupport(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth + 1, Main.screenHeight + 1));
				Rectangle.Intersect(ref value3, ref value, ref result);
				if (result.Width == 0 || result.Height == 0)
				{
					return;
				}
				((Rectangle)(ref result)).Offset(-value3.X, -value3.Y);
			}
			else
			{
				GetMapCoords(num, num2, 1, out var result2);
				GetMapCoords(num + num3 + 1, num2 + num4 + 1, 1, out var result3);
				Rectangle value2 = default(Rectangle);
				((Rectangle)(ref value2))._002Ector(result2.X, result2.Y, result3.X - result2.X, result3.Y - result2.Y);
				Rectangle value4 = default(Rectangle);
				((Rectangle)(ref value4))._002Ector(0, 0, Main.screenWidth + 1, Main.screenHeight + 1);
				Rectangle.Intersect(ref value4, ref value2, ref result);
				if (result.Width == 0 || result.Height == 0)
				{
					return;
				}
				((Rectangle)(ref result)).Offset(-value4.X, -value4.Y);
			}
			sb.Draw(TextureAssets.MagicPixel.Value, result, Settings.MarkedAreaColor);
			Rectangle rectangle = Rectangle.Empty;
			for (int i = 0; i < 2; i++)
			{
				if (currentAim != i)
				{
					DrawBound(sb, new Rectangle(result.X, result.Y + ((i == 1) ? result.Height : (-2)), result.Width, 2), 0);
				}
				else
				{
					((Rectangle)(ref rectangle))._002Ector(result.X, result.Y + ((i == 1) ? result.Height : (-2)), result.Width, 2);
				}
				if (currentAim != i + 2)
				{
					DrawBound(sb, new Rectangle(result.X + ((i == 1) ? result.Width : (-2)), result.Y, 2, result.Height), 0);
				}
				else
				{
					((Rectangle)(ref rectangle))._002Ector(result.X + ((i == 1) ? result.Width : (-2)), result.Y, 2, result.Height);
				}
			}
			if (rectangle != Rectangle.Empty)
			{
				DrawBound(sb, rectangle, 1 + dragging.ToInt());
			}
		}

		private void DrawBound(SpriteBatch sb, Rectangle r, int mode)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			Rectangle val = default(Rectangle);
			switch (mode)
			{
			case 0:
				sb.Draw(TextureAssets.MagicPixel.Value, r, Color.Silver);
				break;
			case 1:
				((Rectangle)(ref val))._002Ector(r.X - 2, r.Y, r.Width + 4, r.Height);
				sb.Draw(TextureAssets.MagicPixel.Value, val, Color.White);
				((Rectangle)(ref val))._002Ector(r.X, r.Y - 2, r.Width, r.Height + 4);
				sb.Draw(TextureAssets.MagicPixel.Value, val, Color.White);
				sb.Draw(TextureAssets.MagicPixel.Value, r, Color.White);
				break;
			case 2:
				((Rectangle)(ref val))._002Ector(r.X - 2, r.Y, r.Width + 4, r.Height);
				sb.Draw(TextureAssets.MagicPixel.Value, val, Color.Gold);
				((Rectangle)(ref val))._002Ector(r.X, r.Y - 2, r.Width, r.Height + 4);
				sb.Draw(TextureAssets.MagicPixel.Value, val, Color.Gold);
				sb.Draw(TextureAssets.MagicPixel.Value, r, Color.Gold);
				break;
			}
		}
	}

	private class ModeChangeSettings : CaptureInterfaceMode
	{
		private const int ButtonsCount = 7;

		private int hoveredButton = -1;

		private bool inUI;

		private Rectangle GetRect()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Rectangle result = default(Rectangle);
			((Rectangle)(ref result))._002Ector(0, 0, 224, 170);
			if (Settings.ScreenAnchor == 0)
			{
				result.X = 227 - result.Width / 2;
				result.Y = 80;
			}
			return result;
		}

		private void ButtonDraw(int button, ref string key, ref string value)
		{
			switch (button)
			{
			case 0:
				key = Lang.inter[74].Value;
				value = Lang.inter[73 - Settings.PackImage.ToInt()].Value;
				break;
			case 1:
				key = Lang.inter[75].Value;
				value = Lang.inter[73 - Settings.IncludeEntities.ToInt()].Value;
				break;
			case 2:
				key = Lang.inter[76].Value;
				value = Lang.inter[73 - (!Settings.TransparentBackground).ToInt()].Value;
				break;
			case 6:
				key = "      " + Lang.menu[86].Value;
				value = "";
				break;
			case 3:
			case 4:
			case 5:
				break;
			}
		}

		private void PressButton(int button)
		{
			bool flag = false;
			switch (button)
			{
			case 0:
				Settings.PackImage = !Settings.PackImage;
				flag = true;
				break;
			case 1:
				Settings.IncludeEntities = !Settings.IncludeEntities;
				flag = true;
				break;
			case 2:
				Settings.TransparentBackground = !Settings.TransparentBackground;
				flag = true;
				break;
			case 6:
				Settings.PackImage = true;
				Settings.IncludeEntities = true;
				Settings.TransparentBackground = false;
				Settings.BiomeChoiceIndex = -1;
				flag = true;
				break;
			}
			if (flag)
			{
				SoundEngine.PlaySound(12);
			}
		}

		private void DrawWaterChoices(SpriteBatch spritebatch, Point start, Point mouse)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			Rectangle r = default(Rectangle);
			((Rectangle)(ref r))._002Ector(0, 0, 20, 20);
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 7; j++)
				{
					if (i == 1 && j == 6)
					{
						continue;
					}
					int num5 = j + i * 7;
					r.X = start.X + 24 * j + 12 * i;
					r.Y = start.Y + 24 * i;
					int num2 = num5;
					int num3 = 0;
					if (((Rectangle)(ref r)).Contains(mouse))
					{
						if (Main.mouseLeft && Main.mouseLeftRelease)
						{
							SoundEngine.PlaySound(12);
							Settings.BiomeChoiceIndex = num2;
						}
						Main.instance.MouseText(Language.GetTextValue("CaptureBiomeChoice." + num2), 0, 0);
						num3++;
					}
					if (Settings.BiomeChoiceIndex == num2)
					{
						num3 += 2;
					}
					Texture2D value = TextureAssets.Extra[130].Value;
					int x = num5 * 18;
					_ = Color.White;
					float num4 = 1f;
					if (num3 < 2)
					{
						num4 *= 0.5f;
					}
					if (num3 % 2 == 1)
					{
						spritebatch.Draw(TextureAssets.MagicPixel.Value, r.TopLeft(), (Rectangle?)new Rectangle(0, 0, 1, 1), Color.Gold, 0f, Vector2.Zero, new Vector2(20f), (SpriteEffects)0, 0f);
					}
					else
					{
						spritebatch.Draw(TextureAssets.MagicPixel.Value, r.TopLeft(), (Rectangle?)new Rectangle(0, 0, 1, 1), Color.White * num4, 0f, Vector2.Zero, new Vector2(20f), (SpriteEffects)0, 0f);
					}
					spritebatch.Draw(value, r.TopLeft() + new Vector2(2f), (Rectangle?)new Rectangle(x, 0, 16, 16), Color.White * num4);
				}
			}
		}

		private int UnnecessaryBiomeSelectionTypeConversion(int index)
		{
			switch (index)
			{
			case 0:
				return -1;
			case 1:
				return 0;
			case 2:
				return 2;
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
				return index;
			case 9:
				return 10;
			case 10:
				return 12;
			case 11:
				return 13;
			case 12:
				return 14;
			default:
				return 0;
			}
		}

		public override void Update()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			if (!Selected || JustActivated)
			{
				return;
			}
			PlayerInput.SetZoom_UI();
			Point value = default(Point);
			((Point)(ref value))._002Ector(Main.mouseX, Main.mouseY);
			hoveredButton = -1;
			Rectangle rect = GetRect();
			inUI = ((Rectangle)(ref rect)).Contains(value);
			((Rectangle)(ref rect)).Inflate(-20, -20);
			rect.Height = 16;
			int y = rect.Y;
			for (int i = 0; i < 7; i++)
			{
				rect.Y = y + i * 20;
				if (((Rectangle)(ref rect)).Contains(value))
				{
					hoveredButton = i;
					break;
				}
			}
			if (Main.mouseLeft && Main.mouseLeftRelease && hoveredButton != -1)
			{
				PressButton(hoveredButton);
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			if (!Selected)
			{
				return;
			}
			sb.End();
			sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.CurrentWantedZoomMatrix);
			PlayerInput.SetZoom_Context();
			((ModeDragBounds)Modes[1]).currentAim = -1;
			((ModeDragBounds)Modes[1]).DrawMarkedArea(sb);
			sb.End();
			sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
			PlayerInput.SetZoom_UI();
			Rectangle rect = GetRect();
			Utils.DrawInvBG(sb, rect, new Color(63, 65, 151, 255) * 0.485f);
			for (int i = 0; i < 7; i++)
			{
				string key = "";
				string value = "";
				ButtonDraw(i, ref key, ref value);
				Color baseColor = Color.White;
				if (i == hoveredButton)
				{
					baseColor = Color.Gold;
				}
				ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.ItemStack.Value, key, rect.TopLeft() + new Vector2(20f, (float)(20 + 20 * i)), baseColor, 0f, Vector2.Zero, Vector2.One);
				ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.ItemStack.Value, value, rect.TopRight() + new Vector2(-20f, (float)(20 + 20 * i)), baseColor, 0f, FontAssets.ItemStack.Value.MeasureString(value) * Vector2.UnitX, Vector2.One);
			}
			DrawWaterChoices(sb, (rect.TopLeft() + new Vector2((float)(rect.Width / 2 - 84), 90f)).ToPoint(), Main.MouseScreen.ToPoint());
		}

		public override void ToggleActive(bool tickedOn)
		{
			if (tickedOn)
			{
				hoveredButton = -1;
			}
		}

		public override bool UsingMap()
		{
			return inUI;
		}
	}

	private static Dictionary<int, CaptureInterfaceMode> Modes = FillModes();

	public bool Active;

	public static bool JustActivated;

	private bool KeyToggleActiveHeld;

	public int SelectedMode;

	public int HoveredMode;

	public static bool EdgeAPinned;

	public static bool EdgeBPinned;

	public static Point EdgeA;

	public static Point EdgeB;

	public static bool CameraLock;

	private static float CameraFrame;

	private static float CameraWaiting;

	private const float CameraMaxFrame = 5f;

	private const float CameraMaxWait = 60f;

	private static CaptureSettings CameraSettings;

	private static Dictionary<int, CaptureInterfaceMode> FillModes()
	{
		return new Dictionary<int, CaptureInterfaceMode>
		{
			{
				0,
				new ModeEdgeSelection()
			},
			{
				1,
				new ModeDragBounds()
			},
			{
				2,
				new ModeChangeSettings()
			}
		};
	}

	public static Rectangle GetArea()
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		int num3 = Math.Min(EdgeA.X, EdgeB.X);
		int y = Math.Min(EdgeA.Y, EdgeB.Y);
		int num = Math.Abs(EdgeA.X - EdgeB.X);
		int num2 = Math.Abs(EdgeA.Y - EdgeB.Y);
		return new Rectangle(num3, y, num + 1, num2 + 1);
	}

	public void Update()
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		PlayerInput.SetZoom_UI();
		UpdateCamera();
		if (CameraLock)
		{
			return;
		}
		bool toggleCameraMode = PlayerInput.Triggers.Current.ToggleCameraMode;
		if (toggleCameraMode && !KeyToggleActiveHeld && (Main.mouseItem.type == 0 || Active) && !Main.CaptureModeDisabled && !Main.player[Main.myPlayer].dead && !Main.player[Main.myPlayer].ghost)
		{
			ToggleCamera(!Active);
		}
		KeyToggleActiveHeld = toggleCameraMode;
		if (!Active)
		{
			return;
		}
		Main.blockMouse = true;
		if (JustActivated && Main.mouseLeftRelease && !Main.mouseLeft)
		{
			JustActivated = false;
		}
		Vector2 mouse = default(Vector2);
		((Vector2)(ref mouse))._002Ector((float)Main.mouseX, (float)Main.mouseY);
		if (UpdateButtons(mouse) && Main.mouseLeft)
		{
			return;
		}
		foreach (KeyValuePair<int, CaptureInterfaceMode> mode in Modes)
		{
			mode.Value.Selected = mode.Key == SelectedMode;
			mode.Value.Update();
		}
		PlayerInput.SetZoom_Unscaled();
	}

	public void Draw(SpriteBatch sb)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		if (!Active)
		{
			return;
		}
		sb.End();
		sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
		PlayerInput.SetZoom_UI();
		foreach (CaptureInterfaceMode value in Modes.Values)
		{
			value.Draw(sb);
		}
		sb.End();
		sb.Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null, Main.UIScaleMatrix);
		PlayerInput.SetZoom_UI();
		Main.mouseText = false;
		Main.instance.GUIBarsDraw();
		DrawButtons(sb);
		Main.instance.DrawMouseOver();
		Utils.DrawBorderStringBig(sb, Lang.inter[81].Value, new Vector2((float)Main.screenWidth * 0.5f, 100f), Color.White, 1f, 0.5f, 0.5f);
		Utils.DrawCursorSingle(sb, Main.cursorColor, float.NaN, Main.cursorScale);
		DrawCameraLock(sb);
		sb.End();
		sb.Begin();
	}

	public void ToggleCamera(bool On = true)
	{
		if (CameraLock)
		{
			return;
		}
		bool active = Active;
		Active = Modes.ContainsKey(SelectedMode) && On;
		if (active != Active)
		{
			SoundEngine.PlaySound(On ? 10 : 11);
		}
		foreach (KeyValuePair<int, CaptureInterfaceMode> mode in Modes)
		{
			mode.Value.ToggleActive(Active && mode.Key == SelectedMode);
		}
		if (On && !active)
		{
			JustActivated = true;
		}
	}

	private bool UpdateButtons(Vector2 mouse)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		HoveredMode = -1;
		bool flag = !Main.graphics.IsFullScreen;
		int num = 9;
		for (int i = 0; i < num; i++)
		{
			Rectangle val = new Rectangle(24 + 46 * i, 24, 42, 42);
			if (!((Rectangle)(ref val)).Contains(mouse.ToPoint()))
			{
				continue;
			}
			HoveredMode = i;
			bool flag2 = Main.mouseLeft && Main.mouseLeftRelease;
			int num2 = 0;
			if (i == num2++ && flag2)
			{
				QuickScreenshot();
			}
			if (i == num2++ && flag2 && EdgeAPinned && EdgeBPinned)
			{
				CaptureSettings obj = new CaptureSettings
				{
					Area = GetArea(),
					Biome = CaptureBiome.GetCaptureBiome(Settings.BiomeChoiceIndex),
					CaptureBackground = !Settings.TransparentBackground,
					CaptureEntities = Settings.IncludeEntities,
					UseScaling = Settings.PackImage,
					CaptureMech = WiresUI.Settings.DrawWires
				};
				if (obj.Biome.WaterStyle != 13)
				{
					Main.liquidAlpha[13] = 0f;
				}
				StartCamera(obj);
			}
			if (i == num2++ && flag2 && SelectedMode != 0)
			{
				SoundEngine.PlaySound(12);
				SelectedMode = 0;
				ToggleCamera();
			}
			if (i == num2++ && flag2 && SelectedMode != 1)
			{
				SoundEngine.PlaySound(12);
				SelectedMode = 1;
				ToggleCamera();
			}
			if (i == num2++ && flag2)
			{
				SoundEngine.PlaySound(12);
				ResetFocus();
			}
			if (i == num2++ && flag2 && Main.mapEnabled)
			{
				SoundEngine.PlaySound(12);
				Main.mapFullscreen = !Main.mapFullscreen;
			}
			if (i == num2++ && flag2 && SelectedMode != 2)
			{
				SoundEngine.PlaySound(12);
				SelectedMode = 2;
				ToggleCamera();
			}
			if (i == num2++ && flag2 && flag)
			{
				SoundEngine.PlaySound(12);
				Utils.OpenFolder(Path.Combine(Main.SavePath, "Captures"));
			}
			if (i == num2++ && flag2)
			{
				ToggleCamera(On: false);
				Main.blockMouse = true;
				Main.mouseLeftRelease = false;
			}
			return true;
		}
		return false;
	}

	public static void QuickScreenshot()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		Point point = Main.ViewPosition.ToTileCoordinates();
		Point point2 = (Main.ViewPosition + Main.ViewSize).ToTileCoordinates();
		StartCamera(new CaptureSettings
		{
			Area = new Rectangle(point.X, point.Y, point2.X - point.X + 1, point2.Y - point.Y + 1),
			Biome = CaptureBiome.GetCaptureBiome(Settings.BiomeChoiceIndex),
			CaptureBackground = !Settings.TransparentBackground,
			CaptureEntities = Settings.IncludeEntities,
			UseScaling = Settings.PackImage,
			CaptureMech = WiresUI.Settings.DrawWires
		});
	}

	private void DrawButtons(SpriteBatch sb)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		new Vector2((float)Main.mouseX, (float)Main.mouseY);
		int num = 9;
		Vector2 vector = default(Vector2);
		for (int i = 0; i < num; i++)
		{
			Texture2D texture2D = TextureAssets.InventoryBack.Value;
			float num2 = 0.8f;
			((Vector2)(ref vector))._002Ector((float)(24 + 46 * i), 24f);
			Color color = Main.inventoryBack * 0.8f;
			if (SelectedMode == 0 && i == 2)
			{
				texture2D = TextureAssets.InventoryBack14.Value;
			}
			else if (SelectedMode == 1 && i == 3)
			{
				texture2D = TextureAssets.InventoryBack14.Value;
			}
			else if (SelectedMode == 2 && i == 6)
			{
				texture2D = TextureAssets.InventoryBack14.Value;
			}
			else if (i >= 2 && i <= 3)
			{
				texture2D = TextureAssets.InventoryBack2.Value;
			}
			sb.Draw(texture2D, vector, (Rectangle?)null, color, 0f, default(Vector2), num2, (SpriteEffects)0, 0f);
			switch (i)
			{
			case 0:
				texture2D = TextureAssets.Camera[7].Value;
				break;
			case 1:
				texture2D = TextureAssets.Camera[0].Value;
				break;
			case 2:
			case 3:
			case 4:
				texture2D = TextureAssets.Camera[i].Value;
				break;
			case 5:
				texture2D = (Main.mapFullscreen ? TextureAssets.MapIcon[0].Value : TextureAssets.MapIcon[4].Value);
				break;
			case 6:
				texture2D = TextureAssets.Camera[1].Value;
				break;
			case 7:
				texture2D = TextureAssets.Camera[6].Value;
				break;
			case 8:
				texture2D = TextureAssets.Camera[5].Value;
				break;
			}
			sb.Draw(texture2D, vector + new Vector2(26f) * num2, (Rectangle?)null, Color.White, 0f, texture2D.Size() / 2f, 1f, (SpriteEffects)0, 0f);
			bool flag = false;
			switch (i)
			{
			case 1:
				if (!EdgeAPinned || !EdgeBPinned)
				{
					flag = true;
				}
				break;
			case 7:
				if (Main.graphics.IsFullScreen)
				{
					flag = true;
				}
				break;
			case 5:
				if (!Main.mapEnabled)
				{
					flag = true;
				}
				break;
			}
			if (flag)
			{
				sb.Draw(TextureAssets.Cd.Value, vector + new Vector2(26f) * num2, (Rectangle?)null, Color.White * 0.65f, 0f, TextureAssets.Cd.Value.Size() / 2f, 1f, (SpriteEffects)0, 0f);
			}
		}
		string text = "";
		switch (HoveredMode)
		{
		case 0:
			text = Lang.inter[111].Value;
			break;
		case 1:
			text = Lang.inter[67].Value;
			break;
		case 2:
			text = Lang.inter[69].Value;
			break;
		case 3:
			text = Lang.inter[70].Value;
			break;
		case 4:
			text = Lang.inter[78].Value;
			break;
		case 5:
			text = (Main.mapFullscreen ? Lang.inter[109].Value : Lang.inter[108].Value);
			break;
		case 6:
			text = Lang.inter[68].Value;
			break;
		case 7:
			text = Lang.inter[110].Value;
			break;
		case 8:
			text = Lang.inter[71].Value;
			break;
		default:
			text = "???";
			break;
		case -1:
			break;
		}
		switch (HoveredMode)
		{
		case 1:
			if (!EdgeAPinned || !EdgeBPinned)
			{
				text = text + "\n" + Lang.inter[112].Value;
			}
			break;
		case 7:
			if (Main.graphics.IsFullScreen)
			{
				text = text + "\n" + Lang.inter[113].Value;
			}
			break;
		case 5:
			if (!Main.mapEnabled)
			{
				text = text + "\n" + Lang.inter[114].Value;
			}
			break;
		}
		if (text != "")
		{
			Main.instance.MouseText(text, 0, 0);
		}
	}

	private static bool GetMapCoords(int PinX, int PinY, int Goal, out Point result)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.mapFullscreen)
		{
			result = new Point(-1, -1);
			return false;
		}
		float num = 0f;
		float num9 = 0f;
		float num10 = 2f;
		_ = Main.maxTilesX / Main.textureMaxWidth;
		_ = Main.maxTilesY / Main.textureMaxHeight;
		float num11 = 10f;
		float num12 = 10f;
		float num13 = Main.maxTilesX - 10;
		float num14 = Main.maxTilesY - 10;
		num = 200f;
		num9 = 300f;
		num10 = Main.mapFullscreenScale;
		float num15 = (float)Main.screenWidth / (float)Main.maxTilesX * 0.8f;
		if (Main.mapFullscreenScale < num15)
		{
			Main.mapFullscreenScale = num15;
		}
		if (Main.mapFullscreenScale > 16f)
		{
			Main.mapFullscreenScale = 16f;
		}
		num10 = Main.mapFullscreenScale;
		if (Main.mapFullscreenPos.X < num11)
		{
			Main.mapFullscreenPos.X = num11;
		}
		if (Main.mapFullscreenPos.X > num13)
		{
			Main.mapFullscreenPos.X = num13;
		}
		if (Main.mapFullscreenPos.Y < num12)
		{
			Main.mapFullscreenPos.Y = num12;
		}
		if (Main.mapFullscreenPos.Y > num14)
		{
			Main.mapFullscreenPos.Y = num14;
		}
		float x = Main.mapFullscreenPos.X;
		float y = Main.mapFullscreenPos.Y;
		float num16 = x * num10;
		y *= num10;
		num = 0f - num16 + (float)(Main.screenWidth / 2);
		num9 = 0f - y + (float)(Main.screenHeight / 2);
		num += num11 * num10;
		num9 += num12 * num10;
		float num2 = Main.maxTilesX / 840;
		num2 *= Main.mapFullscreenScale;
		float num3 = num;
		float num4 = num9;
		float num5 = TextureAssets.Map.Width();
		float num6 = TextureAssets.Map.Height();
		if (Main.maxTilesX == 8400)
		{
			num2 *= 0.999f;
			num3 -= 40.6f * num2;
			num4 = num9 - 5f * num2;
			num5 -= 8.045f;
			num5 *= num2;
			num6 += 0.12f;
			num6 *= num2;
			if ((double)num2 < 1.2)
			{
				num6 += 1f;
			}
		}
		else if (Main.maxTilesX == 6400)
		{
			num2 *= 1.09f;
			num3 -= 38.8f * num2;
			num4 = num9 - 3.85f * num2;
			num5 -= 13.6f;
			num5 *= num2;
			num6 -= 6.92f;
			num6 *= num2;
			if ((double)num2 < 1.2)
			{
				num6 += 2f;
			}
		}
		else if (Main.maxTilesX == 6300)
		{
			num2 *= 1.09f;
			num3 -= 39.8f * num2;
			num4 = num9 - 4.08f * num2;
			num5 -= 26.69f;
			num5 *= num2;
			num6 -= 6.92f;
			num6 *= num2;
			if ((double)num2 < 1.2)
			{
				num6 += 2f;
			}
		}
		else if (Main.maxTilesX == 4200)
		{
			num2 *= 0.998f;
			num3 -= 37.3f * num2;
			num4 -= 1.7f * num2;
			num5 -= 16f;
			num5 *= num2;
			num6 -= 8.31f;
			num6 *= num2;
		}
		switch (Goal)
		{
		case 0:
		{
			int num7 = (int)((0f - num + (float)PinX) / num10 + num11);
			int num8 = (int)((0f - num9 + (float)PinY) / num10 + num12);
			bool flag = false;
			if ((float)num7 < num11)
			{
				flag = true;
			}
			if ((float)num7 >= num13)
			{
				flag = true;
			}
			if ((float)num8 < num12)
			{
				flag = true;
			}
			if ((float)num8 >= num14)
			{
				flag = true;
			}
			if (!flag)
			{
				result = new Point(num7, num8);
				return true;
			}
			result = new Point(-1, -1);
			return false;
		}
		case 1:
		{
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector(num, num9);
			Vector2 vector2 = new Vector2((float)PinX, (float)PinY) * num10 - new Vector2(10f * num10);
			result = (vector + vector2).ToPoint();
			return true;
		}
		default:
			result = new Point(-1, -1);
			return false;
		}
	}

	private static void ConstraintPoints()
	{
		int offScreenTiles = Lighting.OffScreenTiles;
		if (EdgeAPinned)
		{
			PointWorldClamp(ref EdgeA, offScreenTiles);
		}
		if (EdgeBPinned)
		{
			PointWorldClamp(ref EdgeB, offScreenTiles);
		}
	}

	private static void PointWorldClamp(ref Point point, int fluff)
	{
		if (point.X < fluff)
		{
			point.X = fluff;
		}
		if (point.X > Main.maxTilesX - 1 - fluff)
		{
			point.X = Main.maxTilesX - 1 - fluff;
		}
		if (point.Y < fluff)
		{
			point.Y = fluff;
		}
		if (point.Y > Main.maxTilesY - 1 - fluff)
		{
			point.Y = Main.maxTilesY - 1 - fluff;
		}
	}

	public bool UsingMap()
	{
		if (CameraLock)
		{
			return true;
		}
		return Modes[SelectedMode].UsingMap();
	}

	public static void ResetFocus()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		EdgeAPinned = false;
		EdgeBPinned = false;
		EdgeA = new Point(-1, -1);
		EdgeB = new Point(-1, -1);
	}

	public void Scrolling()
	{
		int num = PlayerInput.ScrollWheelDelta / 120;
		num %= 30;
		if (num < 0)
		{
			num += 30;
		}
		int selectedMode = SelectedMode;
		SelectedMode -= num;
		while (SelectedMode < 0)
		{
			SelectedMode += 2;
		}
		while (SelectedMode > 2)
		{
			SelectedMode -= 2;
		}
		if (SelectedMode != selectedMode)
		{
			SoundEngine.PlaySound(12);
		}
	}

	private void UpdateCamera()
	{
		if (CameraLock && CameraFrame == 4f)
		{
			CaptureManager.Instance.Capture(CameraSettings);
		}
		CameraFrame += CameraLock.ToDirectionInt();
		if (CameraFrame < 0f)
		{
			CameraFrame = 0f;
		}
		if (CameraFrame > 5f)
		{
			CameraFrame = 5f;
		}
		if (CameraFrame == 5f)
		{
			CameraWaiting += 1f;
		}
		if (CameraWaiting > 60f)
		{
			CameraWaiting = 60f;
		}
	}

	private void DrawCameraLock(SpriteBatch sb)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		if (CameraFrame == 0f)
		{
			return;
		}
		sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), (Rectangle?)new Rectangle(0, 0, 1, 1), Color.Black * (CameraFrame / 5f));
		if (CameraFrame != 5f)
		{
			return;
		}
		float num = CameraWaiting - 60f + 5f;
		if (!(num <= 0f))
		{
			num /= 5f;
			float num2 = CaptureManager.Instance.GetProgress() * 100f;
			if (num2 > 100f)
			{
				num2 = 100f;
			}
			string text = num2.ToString("##") + " ";
			string text2 = "/ 100%";
			Vector2 vector = FontAssets.DeathText.Value.MeasureString(text);
			Vector2 vector2 = FontAssets.DeathText.Value.MeasureString(text2);
			Vector2 vector3 = default(Vector2);
			((Vector2)(ref vector3))._002Ector(0f - vector.X, (0f - vector.Y) / 2f);
			Vector2 vector4 = default(Vector2);
			((Vector2)(ref vector4))._002Ector(0f, (0f - vector2.Y) / 2f);
			ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.DeathText.Value, text, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f + vector3, Color.White * num, 0f, Vector2.Zero, Vector2.One);
			ChatManager.DrawColorCodedStringWithShadow(sb, FontAssets.DeathText.Value, text2, new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f + vector4, Color.White * num, 0f, Vector2.Zero, Vector2.One);
		}
	}

	public static void StartCamera(CaptureSettings settings)
	{
		SoundEngine.PlaySound(40);
		CameraSettings = settings;
		CameraLock = true;
		CameraWaiting = 0f;
	}

	public static void EndCamera()
	{
		CameraLock = false;
	}
}
