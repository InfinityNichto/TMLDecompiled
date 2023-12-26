using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

public abstract class RangeElement : ConfigElement
{
	private static RangeElement rightLock;

	private static RangeElement rightHover;

	protected Color SliderColor { get; set; } = Color.White;


	protected Utils.ColorLerpMethod ColorMethod { get; set; }

	internal bool DrawTicks { get; set; }

	public abstract int NumberTicks { get; }

	public abstract float TickIncrement { get; }

	protected abstract float Proportion { get; set; }

	public RangeElement()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		ColorMethod = (float percent) => Color.Lerp(Color.Black, SliderColor, percent);
	}

	public override void OnBind()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		base.OnBind();
		DrawTicks = Attribute.IsDefined(base.MemberInfo.MemberInfo, typeof(DrawTicksAttribute));
		SliderColor = ConfigManager.GetCustomAttributeFromMemberThenMemberType<SliderColorAttribute>(base.MemberInfo, base.Item, base.List)?.Color ?? Color.White;
	}

	public float DrawValueBar(SpriteBatch sb, float scale, float perc, int lockState = 0, Utils.ColorLerpMethod colorMethod = null)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		perc = Utils.Clamp(perc, -0.05f, 1.05f);
		if (colorMethod == null)
		{
			colorMethod = Utils.ColorLerp_BlackToWhite;
		}
		Texture2D colorBarTexture = TextureAssets.ColorBar.Value;
		Vector2 vector = new Vector2((float)colorBarTexture.Width, (float)colorBarTexture.Height) * scale;
		IngameOptions.valuePosition.X -= (int)vector.X;
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector((int)IngameOptions.valuePosition.X, (int)IngameOptions.valuePosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y);
		Rectangle destinationRectangle = rectangle;
		int num = 167;
		float num2 = (float)rectangle.X + 5f * scale;
		float num3 = (float)rectangle.Y + 4f * scale;
		if (DrawTicks)
		{
			int numTicks = NumberTicks;
			if (numTicks > 1)
			{
				for (int tick = 0; tick < numTicks; tick++)
				{
					float percent2 = (float)tick * TickIncrement;
					if (percent2 <= 1f)
					{
						sb.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)(num2 + (float)num * percent2 * scale), rectangle.Y - 2, 2, rectangle.Height + 4), Color.White);
					}
				}
			}
		}
		sb.Draw(colorBarTexture, rectangle, Color.White);
		for (float num4 = 0f; num4 < (float)num; num4 += 1f)
		{
			float percent = num4 / (float)num;
			sb.Draw(TextureAssets.ColorBlip.Value, new Vector2(num2 + num4 * scale, num3), (Rectangle?)null, colorMethod(percent), 0f, Vector2.Zero, scale, (SpriteEffects)0, 0f);
		}
		((Rectangle)(ref rectangle)).Inflate((int)(-5f * scale), 2);
		bool flag = ((Rectangle)(ref rectangle)).Contains(new Point(Main.mouseX, Main.mouseY));
		if (lockState == 2)
		{
			flag = false;
		}
		if (flag || lockState == 1)
		{
			sb.Draw(TextureAssets.ColorHighlight.Value, destinationRectangle, Main.OurFavoriteColor);
		}
		Texture2D colorSlider = TextureAssets.ColorSlider.Value;
		sb.Draw(colorSlider, new Vector2(num2 + 167f * scale * perc, num3 + 4f * scale), (Rectangle?)null, Color.White, 0f, colorSlider.Size() * 0.5f, scale, (SpriteEffects)0, 0f);
		if (Main.mouseX >= rectangle.X && Main.mouseX <= rectangle.X + rectangle.Width)
		{
			IngameOptions.inBar = flag;
			return (float)(Main.mouseX - rectangle.X) / (float)rectangle.Width;
		}
		IngameOptions.inBar = false;
		if (rectangle.X >= Main.mouseX)
		{
			return 0f;
		}
		return 1f;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		float num = 6f;
		int num2 = 0;
		rightHover = null;
		if (!Main.mouseLeft)
		{
			rightLock = null;
		}
		if (rightLock == this)
		{
			num2 = 1;
		}
		else if (rightLock != null)
		{
			num2 = 2;
		}
		CalculatedStyle dimensions = GetDimensions();
		Vector2 val = new Vector2(dimensions.X, dimensions.Y);
		_ = base.IsMouseHovering;
		_ = 1;
		_ = 2;
		Vector2 vector2 = val;
		vector2.X += 8f;
		vector2.Y += 2f + num;
		vector2.X -= 17f;
		((Vector2)(ref vector2))._002Ector(dimensions.X + dimensions.Width - 10f, dimensions.Y + 10f + num);
		IngameOptions.valuePosition = vector2;
		float obj = DrawValueBar(spriteBatch, 1f, Proportion, num2, ColorMethod);
		if (IngameOptions.inBar || rightLock == this)
		{
			rightHover = this;
			if (PlayerInput.Triggers.Current.MouseLeft && rightLock == this)
			{
				Proportion = obj;
			}
		}
		if (rightHover != null && rightLock == null && PlayerInput.Triggers.JustPressed.MouseLeft)
		{
			rightLock = rightHover;
		}
	}
}
