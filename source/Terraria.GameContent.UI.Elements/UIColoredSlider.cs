using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIColoredSlider : UISliderBase
{
	private Color _color;

	private LocalizedText _textKey;

	private Func<float> _getStatusTextAct;

	private Action<float> _slideKeyboardAction;

	private Func<float, Color> _blipFunc;

	private Action _slideGamepadAction;

	private const bool BOTHER_WITH_TEXT = false;

	private bool _isReallyMouseOvered;

	private bool _alreadyHovered;

	private bool _soundedUsage;

	public UIColoredSlider(LocalizedText textKey, Func<float> getStatus, Action<float> setStatusKeyboard, Action setStatusGamepad, Func<float, Color> blipColorFunction, Color color)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		_color = color;
		_textKey = textKey;
		_getStatusTextAct = ((getStatus != null) ? getStatus : ((Func<float>)(() => 0f)));
		_slideKeyboardAction = ((setStatusKeyboard != null) ? setStatusKeyboard : ((Action<float>)delegate
		{
		}));
		_blipFunc = ((blipColorFunction != null) ? blipColorFunction : ((Func<float, Color>)((float s) => Color.Lerp(Color.Black, Color.White, s))));
		_slideGamepadAction = setStatusGamepad;
		_isReallyMouseOvered = false;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		UISliderBase.CurrentAimedSlider = null;
		if (!Main.mouseLeft)
		{
			UISliderBase.CurrentLockedSlider = null;
		}
		int usageLevel = GetUsageLevel();
		float num = 8f;
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		Vector2 val = new Vector2(dimensions.X, dimensions.Y);
		bool flag = false;
		bool flag2 = base.IsMouseHovering;
		if (usageLevel == 2)
		{
			flag2 = false;
		}
		if (usageLevel == 1)
		{
			flag2 = true;
		}
		Vector2 vector2 = val + new Vector2(0f, 2f);
		Color.Lerp(flag ? Color.Gold : (flag2 ? Color.White : Color.Silver), Color.White, flag2 ? 0.5f : 0f);
		new Vector2(0.8f);
		vector2.X += 8f;
		vector2.Y += num;
		vector2.X -= 17f;
		TextureAssets.ColorBar.Frame();
		((Vector2)(ref vector2))._002Ector(dimensions.X + dimensions.Width - 10f, dimensions.Y + 10f + num);
		bool wasInBar;
		float obj = DrawValueBar(spriteBatch, vector2, 1f, _getStatusTextAct(), usageLevel, out wasInBar, _blipFunc);
		if (UISliderBase.CurrentLockedSlider == this || wasInBar)
		{
			UISliderBase.CurrentAimedSlider = this;
			if (PlayerInput.Triggers.Current.MouseLeft && !PlayerInput.UsingGamepad && UISliderBase.CurrentLockedSlider == this)
			{
				_slideKeyboardAction(obj);
				if (!_soundedUsage)
				{
					SoundEngine.PlaySound(12);
				}
				_soundedUsage = true;
			}
			else
			{
				_soundedUsage = false;
			}
		}
		if (UISliderBase.CurrentAimedSlider != null && UISliderBase.CurrentLockedSlider == null)
		{
			UISliderBase.CurrentLockedSlider = UISliderBase.CurrentAimedSlider;
		}
		if (_isReallyMouseOvered)
		{
			_slideGamepadAction();
		}
	}

	private float DrawValueBar(SpriteBatch sb, Vector2 drawPosition, float drawScale, float sliderPosition, int lockMode, out bool wasInBar, Func<float, Color> blipColorFunc)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value = TextureAssets.ColorBar.Value;
		Vector2 vector = new Vector2((float)value.Width, (float)value.Height) * drawScale;
		drawPosition.X -= (int)vector.X;
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector((int)drawPosition.X, (int)drawPosition.Y - (int)vector.Y / 2, (int)vector.X, (int)vector.Y);
		Rectangle destinationRectangle = rectangle;
		sb.Draw(value, rectangle, Color.White);
		float num = (float)rectangle.X + 5f * drawScale;
		float num2 = (float)rectangle.Y + 4f * drawScale;
		for (float num3 = 0f; num3 < 167f; num3 += 1f)
		{
			float arg = num3 / 167f;
			Color color = blipColorFunc(arg);
			sb.Draw(TextureAssets.ColorBlip.Value, new Vector2(num + num3 * drawScale, num2), (Rectangle?)null, color, 0f, Vector2.Zero, drawScale, (SpriteEffects)0, 0f);
		}
		rectangle.X = (int)num - 2;
		rectangle.Y = (int)num2;
		rectangle.Width -= 4;
		rectangle.Height -= 8;
		bool flag = (_isReallyMouseOvered = ((Rectangle)(ref rectangle)).Contains(new Point(Main.mouseX, Main.mouseY)));
		if (IgnoresMouseInteraction)
		{
			flag = false;
		}
		if (lockMode == 2)
		{
			flag = false;
		}
		if (flag || lockMode == 1)
		{
			sb.Draw(TextureAssets.ColorHighlight.Value, destinationRectangle, Main.OurFavoriteColor);
			if (!_alreadyHovered)
			{
				SoundEngine.PlaySound(12);
			}
			_alreadyHovered = true;
		}
		else
		{
			_alreadyHovered = false;
		}
		wasInBar = false;
		if (!IgnoresMouseInteraction)
		{
			sb.Draw(TextureAssets.ColorSlider.Value, new Vector2(num + 167f * drawScale * sliderPosition, num2 + 4f * drawScale), (Rectangle?)null, Color.White, 0f, new Vector2(0.5f * (float)TextureAssets.ColorSlider.Value.Width, 0.5f * (float)TextureAssets.ColorSlider.Value.Height), drawScale, (SpriteEffects)0, 0f);
			if (Main.mouseX >= rectangle.X && Main.mouseX <= rectangle.X + rectangle.Width)
			{
				wasInBar = flag;
				return (float)(Main.mouseX - rectangle.X) / (float)rectangle.Width;
			}
		}
		if (rectangle.X >= Main.mouseX)
		{
			return 0f;
		}
		return 1f;
	}
}
