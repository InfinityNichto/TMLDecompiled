using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIBestiaryInfoLine<T> : UIElement, IManuallyOrderedUIElement
{
	private T _text;

	private float _textScale = 1f;

	private Vector2 _textSize = Vector2.Zero;

	private Color _color = Color.White;

	public int OrderInUIList { get; set; }

	public float TextScale
	{
		get
		{
			return _textScale;
		}
		set
		{
			_textScale = value;
		}
	}

	public Vector2 TextSize => _textSize;

	public string Text
	{
		get
		{
			if (_text != null)
			{
				return _text.ToString();
			}
			return "";
		}
	}

	public Color TextColor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _color;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_color = value;
		}
	}

	public UIBestiaryInfoLine(T text, float textScale = 1f)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		SetText(text, textScale);
	}

	public override void Recalculate()
	{
		SetText(_text, _textScale);
		base.Recalculate();
	}

	public void SetText(T text)
	{
		SetText(text, _textScale);
	}

	public virtual void SetText(T text, float textScale)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		Vector2 textSize = new Vector2(FontAssets.MouseText.Value.MeasureString(text.ToString()).X, 16f) * textScale;
		_text = text;
		_textScale = textScale;
		_textSize = textSize;
		MinWidth.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
		MinHeight.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle innerDimensions = GetInnerDimensions();
		Vector2 pos = innerDimensions.Position();
		pos.Y -= 2f * _textScale;
		pos.X += (innerDimensions.Width - _textSize.X) * 0.5f;
		Utils.DrawBorderString(spriteBatch, Text, pos, _color, _textScale);
	}

	public override int CompareTo(object obj)
	{
		if (obj is IManuallyOrderedUIElement manuallyOrderedUIElement)
		{
			return OrderInUIList.CompareTo(manuallyOrderedUIElement.OrderInUIList);
		}
		return base.CompareTo(obj);
	}
}
