using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameInput;

namespace Terraria.GameContent.UI.Elements;

public class UIVerticalSlider : UISliderBase
{
	public float FillPercent;

	public Color FilledColor = Main.OurFavoriteColor;

	public Color EmptyColor = Color.Black;

	private Func<float> _getSliderValue;

	private Action<float> _slideKeyboardAction;

	private Func<float, Color> _blipFunc;

	private Action _slideGamepadAction;

	private bool _isReallyMouseOvered;

	private bool _soundedUsage;

	private bool _alreadyHovered;

	public UIVerticalSlider(Func<float> getStatus, Action<float> setStatusKeyboard, Action setStatusGamepad, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_getSliderValue = ((getStatus != null) ? getStatus : ((Func<float>)(() => 0f)));
		_slideKeyboardAction = ((setStatusKeyboard != null) ? setStatusKeyboard : ((Action<float>)delegate
		{
		}));
		_slideGamepadAction = setStatusGamepad;
		_isReallyMouseOvered = false;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		UISliderBase.CurrentAimedSlider = null;
		if (!Main.mouseLeft)
		{
			UISliderBase.CurrentLockedSlider = null;
		}
		GetUsageLevel();
		FillPercent = _getSliderValue();
		float sliderValueThatWasSet = FillPercent;
		bool flag = false;
		if (DrawValueBarDynamicWidth(spriteBatch, out sliderValueThatWasSet))
		{
			flag = true;
		}
		if (UISliderBase.CurrentLockedSlider == this || flag)
		{
			UISliderBase.CurrentAimedSlider = this;
			if (PlayerInput.Triggers.Current.MouseLeft && !PlayerInput.UsingGamepad && UISliderBase.CurrentLockedSlider == this)
			{
				_slideKeyboardAction(sliderValueThatWasSet);
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

	private bool DrawValueBarDynamicWidth(SpriteBatch spriteBatch, out float sliderValueThatWasSet)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		sliderValueThatWasSet = 0f;
		Texture2D value = TextureAssets.ColorBar.Value;
		Rectangle rectangle = GetDimensions().ToRectangle();
		Rectangle rectangle2 = default(Rectangle);
		((Rectangle)(ref rectangle2))._002Ector(5, 4, 4, 4);
		Utils.DrawSplicedPanel(spriteBatch, value, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, rectangle2.X, rectangle2.Width, rectangle2.Y, rectangle2.Height, Color.White);
		Rectangle rectangle3 = rectangle;
		rectangle3.X += ((Rectangle)(ref rectangle2)).Left;
		rectangle3.Width -= ((Rectangle)(ref rectangle2)).Right;
		rectangle3.Y += ((Rectangle)(ref rectangle2)).Top;
		rectangle3.Height -= ((Rectangle)(ref rectangle2)).Bottom;
		Texture2D value2 = TextureAssets.MagicPixel.Value;
		Rectangle value3 = default(Rectangle);
		((Rectangle)(ref value3))._002Ector(0, 0, 1, 1);
		spriteBatch.Draw(value2, rectangle3, (Rectangle?)value3, EmptyColor);
		Rectangle destinationRectangle = rectangle3;
		destinationRectangle.Height = (int)((float)destinationRectangle.Height * FillPercent);
		destinationRectangle.Y += rectangle3.Height - destinationRectangle.Height;
		spriteBatch.Draw(value2, destinationRectangle, (Rectangle?)value3, FilledColor);
		Vector2 center = new Vector2((float)(((Rectangle)(ref destinationRectangle)).Center.X + 1), (float)((Rectangle)(ref destinationRectangle)).Top);
		Vector2 size = default(Vector2);
		((Vector2)(ref size))._002Ector((float)(destinationRectangle.Width + 16), 4f);
		Rectangle rectangle4 = Utils.CenteredRectangle(center, size);
		Rectangle destinationRectangle2 = rectangle4;
		((Rectangle)(ref destinationRectangle2)).Inflate(2, 2);
		spriteBatch.Draw(value2, destinationRectangle2, (Rectangle?)value3, Color.Black);
		spriteBatch.Draw(value2, rectangle4, (Rectangle?)value3, Color.White);
		Rectangle rectangle5 = rectangle3;
		((Rectangle)(ref rectangle5)).Inflate(4, 0);
		bool flag = (_isReallyMouseOvered = ((Rectangle)(ref rectangle5)).Contains(Main.MouseScreen.ToPoint()));
		if (IgnoresMouseInteraction)
		{
			flag = false;
		}
		int usageLevel = GetUsageLevel();
		if (usageLevel == 2)
		{
			flag = false;
		}
		if (usageLevel == 1)
		{
			flag = true;
		}
		if (flag || usageLevel == 1)
		{
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
		if (flag)
		{
			sliderValueThatWasSet = Utils.GetLerpValue(((Rectangle)(ref rectangle3)).Bottom, ((Rectangle)(ref rectangle3)).Top, Main.mouseY, clamped: true);
			return true;
		}
		return false;
	}
}
