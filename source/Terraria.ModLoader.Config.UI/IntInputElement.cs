using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class IntInputElement : ConfigElement
{
	public IList<int> IntList { get; set; }

	public int Min { get; set; }

	public int Max { get; set; } = 100;


	public int Increment { get; set; } = 1;


	public override void OnBind()
	{
		base.OnBind();
		IntList = (IList<int>)base.List;
		if (IntList != null)
		{
			base.TextDisplayFunction = () => base.Index + 1 + ": " + IntList[base.Index];
		}
		if (RangeAttribute != null && RangeAttribute.Min is int && RangeAttribute.Max is int)
		{
			Min = (int)RangeAttribute.Min;
			Max = (int)RangeAttribute.Max;
		}
		if (IncrementAttribute != null && IncrementAttribute.Increment is int)
		{
			Increment = (int)IncrementAttribute.Increment;
		}
		UIPanel textBoxBackground = new UIPanel();
		textBoxBackground.SetPadding(0f);
		UIFocusInputTextField uIInputTextField = new UIFocusInputTextField("Type here");
		textBoxBackground.Top.Set(0f, 0f);
		textBoxBackground.Left.Set(-190f, 1f);
		textBoxBackground.Width.Set(180f, 0f);
		textBoxBackground.Height.Set(30f, 0f);
		Append(textBoxBackground);
		uIInputTextField.SetText(GetValue().ToString());
		uIInputTextField.Top.Set(5f, 0f);
		uIInputTextField.Left.Set(10f, 0f);
		uIInputTextField.Width.Set(-42f, 1f);
		uIInputTextField.Height.Set(20f, 0f);
		uIInputTextField.OnTextChange += delegate
		{
			if (int.TryParse(uIInputTextField.CurrentString, out var result))
			{
				SetValue(result);
			}
		};
		uIInputTextField.OnUnfocus += delegate
		{
			uIInputTextField.SetText(GetValue().ToString());
		};
		textBoxBackground.Append(uIInputTextField);
		UIModConfigHoverImageSplit upDownButton = new UIModConfigHoverImageSplit(base.UpDownTexture, "+" + Increment, "-" + Increment);
		upDownButton.Recalculate();
		upDownButton.Top.Set(4f, 0f);
		upDownButton.Left.Set(-30f, 1f);
		upDownButton.OnLeftClick += delegate(UIMouseEvent a, UIElement b)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Rectangle val = b.GetDimensions().ToRectangle();
			if (a.MousePosition.Y < (float)(val.Y + val.Height / 2))
			{
				SetValue(Utils.Clamp(GetValue() + Increment, Min, Max));
			}
			else
			{
				SetValue(Utils.Clamp(GetValue() - Increment, Min, Max));
			}
			uIInputTextField.SetText(GetValue().ToString());
		};
		textBoxBackground.Append(upDownButton);
		Recalculate();
	}

	protected virtual int GetValue()
	{
		return (int)GetObject();
	}

	protected virtual void SetValue(int value)
	{
		SetObject(Utils.Clamp(value, Min, Max));
	}
}
