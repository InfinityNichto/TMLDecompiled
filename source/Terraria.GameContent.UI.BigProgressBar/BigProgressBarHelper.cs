using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Terraria.GameContent.UI.BigProgressBar;

public class BigProgressBarHelper
{
	private const string _bossBarTexturePath = "Images/UI/UI_BossBar";

	public static void DrawBareBonesBar(SpriteBatch spriteBatch, float lifePercent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), new Vector2(400f, 20f));
		Rectangle destinationRectangle = rectangle;
		((Rectangle)(ref destinationRectangle)).Inflate(2, 2);
		Texture2D value = TextureAssets.MagicPixel.Value;
		Rectangle value2 = default(Rectangle);
		((Rectangle)(ref value2))._002Ector(0, 0, 1, 1);
		Rectangle destinationRectangle2 = rectangle;
		destinationRectangle2.Width = (int)((float)destinationRectangle2.Width * lifePercent);
		spriteBatch.Draw(value, destinationRectangle, (Rectangle?)value2, Color.White * 0.6f);
		spriteBatch.Draw(value, rectangle, (Rectangle?)value2, Color.Black * 0.6f);
		spriteBatch.Draw(value, destinationRectangle2, (Rectangle?)value2, Color.LimeGreen * 0.5f);
	}

	public static void DrawFancyBar(SpriteBatch spriteBatch, float lifeAmount, float lifeMax, Texture2D barIconTexture, Rectangle barIconFrame)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		if (BossBarLoader.drawingInfo.HasValue)
		{
			DrawFancyBar(spriteBatch, lifeAmount, lifeMax, barIconTexture, barIconFrame, 0f, 0f);
			return;
		}
		Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/UI_BossBar").Value;
		Point p = default(Point);
		((Point)(ref p))._002Ector(456, 22);
		Point p2 = default(Point);
		((Point)(ref p2))._002Ector(32, 24);
		int verticalFrames = 6;
		Rectangle value2 = value.Frame(1, verticalFrames, 0, 3);
		Color color = Color.White * 0.2f;
		float num = lifeAmount / lifeMax;
		int num2 = (int)((float)p.X * num);
		num2 -= num2 % 2;
		Rectangle value3 = value.Frame(1, verticalFrames, 0, 2);
		value3.X += p2.X;
		value3.Y += p2.Y;
		value3.Width = 2;
		value3.Height = p.Y;
		Rectangle value4 = value.Frame(1, verticalFrames, 0, 1);
		value4.X += p2.X;
		value4.Y += p2.Y;
		value4.Width = 2;
		value4.Height = p.Y;
		Rectangle rectangle = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), p.ToVector2());
		Vector2 vector = rectangle.TopLeft() - p2.ToVector2();
		spriteBatch.Draw(value, vector, (Rectangle?)value2, color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		spriteBatch.Draw(value, rectangle.TopLeft(), (Rectangle?)value3, Color.White, 0f, Vector2.Zero, new Vector2((float)(num2 / value3.Width), 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(value, rectangle.TopLeft() + new Vector2((float)(num2 - 2), 0f), (Rectangle?)value4, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		Rectangle value5 = value.Frame(1, verticalFrames);
		spriteBatch.Draw(value, vector, (Rectangle?)value5, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		Vector2 vector2 = new Vector2(4f, 20f) + new Vector2(26f, 28f) / 2f;
		spriteBatch.Draw(barIconTexture, vector + vector2, (Rectangle?)barIconFrame, Color.White, 0f, barIconFrame.Size() / 2f, 1f, (SpriteEffects)0, 0f);
		if (BigProgressBarSystem.ShowText)
		{
			DrawHealthText(spriteBatch, rectangle, lifeAmount, lifeMax);
		}
	}

	private static void DrawHealthText(SpriteBatch spriteBatch, Rectangle area, float current, float max)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		DynamicSpriteFont value = FontAssets.ItemStack.Value;
		Vector2 vector = ((Rectangle)(ref area)).Center.ToVector2();
		vector.Y += 1f;
		string text = "/";
		Vector2 vector2 = value.MeasureString(text);
		Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X, vector.Y, Color.White, Color.Black, vector2 * 0.5f);
		text = ((int)current).ToString();
		vector2 = value.MeasureString(text);
		Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X - 5f, vector.Y, Color.White, Color.Black, vector2 * new Vector2(1f, 0.5f));
		text = ((int)max).ToString();
		vector2 = value.MeasureString(text);
		Utils.DrawBorderStringFourWay(spriteBatch, value, text, vector.X + 5f, vector.Y, Color.White, Color.Black, vector2 * new Vector2(0f, 0.5f));
	}

	public static void DrawFancyBar(SpriteBatch spriteBatch, float lifeAmount, float lifeMax, Texture2D barIconTexture, Rectangle barIconFrame, float shieldCurrent, float shieldMax)
	{
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/UI_BossBar").Value;
		if (BossBarLoader.drawingInfo.HasValue)
		{
			BigProgressBarInfo info = BossBarLoader.drawingInfo.Value;
			BossBarLoader.drawingInfo = null;
			Vector2 barCenter = Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f);
			Color iconColor = Color.White;
			float iconScale = 1f;
			BossBarDrawParams drawParams = new BossBarDrawParams(value, barCenter, barIconTexture, barIconFrame, iconColor, lifeAmount, lifeMax, shieldCurrent, shieldMax, iconScale, info.showText, Vector2.Zero);
			if (BossBarLoader.PreDraw(spriteBatch, info, ref drawParams))
			{
				BossBarLoader.DrawFancyBar_TML(spriteBatch, drawParams);
			}
			BossBarLoader.PostDraw(spriteBatch, info, drawParams);
			return;
		}
		Point p = default(Point);
		((Point)(ref p))._002Ector(456, 22);
		Point p2 = default(Point);
		((Point)(ref p2))._002Ector(32, 24);
		int verticalFrames = 6;
		Rectangle value2 = value.Frame(1, verticalFrames, 0, 3);
		Color color = Color.White * 0.2f;
		float num = lifeAmount / lifeMax;
		int num2 = (int)((float)p.X * num);
		num2 -= num2 % 2;
		Rectangle value3 = value.Frame(1, verticalFrames, 0, 2);
		value3.X += p2.X;
		value3.Y += p2.Y;
		value3.Width = 2;
		value3.Height = p.Y;
		Rectangle value4 = value.Frame(1, verticalFrames, 0, 1);
		value4.X += p2.X;
		value4.Y += p2.Y;
		value4.Width = 2;
		value4.Height = p.Y;
		float num3 = shieldCurrent / shieldMax;
		int num4 = (int)((float)p.X * num3);
		num4 -= num4 % 2;
		Rectangle value5 = value.Frame(1, verticalFrames, 0, 5);
		value5.X += p2.X;
		value5.Y += p2.Y;
		value5.Width = 2;
		value5.Height = p.Y;
		Rectangle value6 = value.Frame(1, verticalFrames, 0, 4);
		value6.X += p2.X;
		value6.Y += p2.Y;
		value6.Width = 2;
		value6.Height = p.Y;
		Rectangle rectangle = Utils.CenteredRectangle(Main.ScreenSize.ToVector2() * new Vector2(0.5f, 1f) + new Vector2(0f, -50f), p.ToVector2());
		Vector2 vector = rectangle.TopLeft() - p2.ToVector2();
		spriteBatch.Draw(value, vector, (Rectangle?)value2, color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		spriteBatch.Draw(value, rectangle.TopLeft(), (Rectangle?)value3, Color.White, 0f, Vector2.Zero, new Vector2((float)(num2 / value3.Width), 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(value, rectangle.TopLeft() + new Vector2((float)(num2 - 2), 0f), (Rectangle?)value4, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		if (!(shieldMax <= 0f))
		{
			spriteBatch.Draw(value, rectangle.TopLeft(), (Rectangle?)value5, Color.White, 0f, Vector2.Zero, new Vector2((float)(num4 / value5.Width), 1f), (SpriteEffects)0, 0f);
			spriteBatch.Draw(value, rectangle.TopLeft() + new Vector2((float)(num4 - 2), 0f), (Rectangle?)value6, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		}
		Rectangle value7 = value.Frame(1, verticalFrames);
		spriteBatch.Draw(value, vector, (Rectangle?)value7, Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
		Vector2 vector2 = new Vector2(4f, 20f) + barIconFrame.Size() / 2f;
		spriteBatch.Draw(barIconTexture, vector + vector2, (Rectangle?)barIconFrame, Color.White, 0f, barIconFrame.Size() / 2f, 1f, (SpriteEffects)0, 0f);
		if (BigProgressBarSystem.ShowText)
		{
			if (shieldCurrent > 0f)
			{
				DrawHealthText(spriteBatch, rectangle, shieldCurrent, shieldMax);
			}
			else
			{
				DrawHealthText(spriteBatch, rectangle, lifeAmount, lifeMax);
			}
		}
	}

	/// <summary>
	/// Draws "<paramref name="current" />/<paramref name="max" />" as text centered on <paramref name="area" />, offset by <paramref name="textOffset" />.
	/// </summary>
	/// <param name="spriteBatch">The spriteBatch that is drawn on</param>
	/// <param name="area">The Rectangle that the text is centered on</param>
	/// <param name="textOffset">Offset for the text position</param>
	/// <param name="current">Number shown left of the "/"</param>
	/// <param name="max">Number shown right of the "/"</param>
	public static void DrawHealthText(SpriteBatch spriteBatch, Rectangle area, Vector2 textOffset, float current, float max)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		DynamicSpriteFont font = FontAssets.ItemStack.Value;
		Vector2 center = ((Rectangle)(ref area)).Center.ToVector2() + textOffset;
		center.Y += 1f;
		string text = "/";
		Vector2 textSize = font.MeasureString(text);
		Utils.DrawBorderStringFourWay(spriteBatch, font, text, center.X, center.Y, Color.White, Color.Black, textSize * 0.5f);
		text = ((int)current).ToString();
		textSize = font.MeasureString(text);
		Utils.DrawBorderStringFourWay(spriteBatch, font, text, center.X - 5f, center.Y, Color.White, Color.Black, textSize * new Vector2(1f, 0.5f));
		text = ((int)max).ToString();
		textSize = font.MeasureString(text);
		Utils.DrawBorderStringFourWay(spriteBatch, font, text, center.X + 5f, center.Y, Color.White, Color.Black, textSize * new Vector2(0f, 0.5f));
	}
}
