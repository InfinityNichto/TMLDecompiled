using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

/// <summary>
/// A text panel that supports hover and click sounds, hover colors, and alternate colors.
/// </summary>
/// <typeparam name="T"></typeparam>
public class UIButton<T> : UIAutoScaleTextTextPanel<T>
{
	public SoundStyle? HoverSound;

	public SoundStyle? ClickSound;

	public T HoverText;

	public T AltHoverText;

	public bool TooltipText;

	public Color HoverPanelColor = UICommon.DefaultUIBlue;

	public Color HoverBorderColor = UICommon.DefaultUIBorderMouseOver;

	public Color? AltPanelColor;

	public Color? AltBorderColor;

	public Color? AltHoverPanelColor;

	public Color? AltHoverBorderColor;

	public Func<bool> UseAltColors = () => false;

	private Color? _panelColor;

	private Color? _borderColor;

	public UIButton(T text, float textScaleMax = 1f, bool large = false)
		: base(text, textScaleMax, large)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Unknown result type (might be due to invalid IL or missing references)


	public override void Recalculate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		base.Recalculate();
		Color valueOrDefault = _panelColor.GetValueOrDefault();
		if (!_panelColor.HasValue)
		{
			valueOrDefault = BackgroundColor;
			_panelColor = valueOrDefault;
		}
		valueOrDefault = _borderColor.GetValueOrDefault();
		if (!_borderColor.HasValue)
		{
			valueOrDefault = BorderColor;
			_borderColor = valueOrDefault;
		}
		valueOrDefault = AltPanelColor.GetValueOrDefault();
		if (!AltPanelColor.HasValue)
		{
			valueOrDefault = BackgroundColor;
			AltPanelColor = valueOrDefault;
		}
		valueOrDefault = AltBorderColor.GetValueOrDefault();
		if (!AltBorderColor.HasValue)
		{
			valueOrDefault = BorderColor;
			AltBorderColor = valueOrDefault;
		}
		valueOrDefault = AltHoverPanelColor.GetValueOrDefault();
		if (!AltHoverPanelColor.HasValue)
		{
			valueOrDefault = HoverPanelColor;
			AltHoverPanelColor = valueOrDefault;
		}
		valueOrDefault = AltHoverBorderColor.GetValueOrDefault();
		if (!AltHoverBorderColor.HasValue)
		{
			valueOrDefault = HoverBorderColor;
			AltHoverBorderColor = valueOrDefault;
		}
	}

	protected void SetPanelColors()
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		bool altCondition = UseAltColors();
		if (base.IsMouseHovering)
		{
			BackgroundColor = (altCondition ? AltHoverPanelColor.Value : HoverPanelColor);
			BorderColor = (altCondition ? AltHoverBorderColor.Value : HoverBorderColor);
		}
		else
		{
			BackgroundColor = (altCondition ? AltPanelColor.Value : _panelColor.Value);
			BorderColor = (altCondition ? AltBorderColor.Value : _borderColor.Value);
		}
	}

	public override void OnActivate()
	{
		SetPanelColors();
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		SetPanelColors();
		if (!base.IsMouseHovering)
		{
			return;
		}
		T val;
		object obj;
		if (!UseAltColors())
		{
			ref T reference = ref HoverText;
			val = default(T);
			if (val == null)
			{
				val = reference;
				reference = ref val;
				if (val == null)
				{
					obj = null;
					goto IL_0091;
				}
			}
			obj = reference.ToString();
		}
		else
		{
			ref T reference2 = ref AltHoverText;
			val = default(T);
			if (val == null)
			{
				val = reference2;
				reference2 = ref val;
				if (val == null)
				{
					obj = null;
					goto IL_0091;
				}
			}
			obj = reference2.ToString();
		}
		goto IL_0091;
		IL_0091:
		string text = (string)obj;
		if (text != null)
		{
			if (TooltipText)
			{
				UICommon.TooltipMouseText(text);
			}
			else
			{
				Main.instance.MouseText(text, 0, 0);
			}
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		if (HoverSound.HasValue)
		{
			SoundStyle style = HoverSound.Value;
			SoundEngine.PlaySound(in style);
		}
	}

	public override void LeftClick(UIMouseEvent evt)
	{
		base.LeftClick(evt);
		if (ClickSound.HasValue)
		{
			SoundStyle style = ClickSound.Value;
			SoundEngine.PlaySound(in style);
		}
	}
}
