using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements;

public class UIText : UIElement
{
	private object _text = "";

	private float _textScale = 1f;

	private Vector2 _textSize = Vector2.Zero;

	private bool _isLarge;

	private Color _color = Color.White;

	private Color _shadowColor = Color.Black;

	private bool _isWrapped;

	public bool DynamicallyScaleDownToWidth;

	private string _visibleText;

	private string _lastTextReference;

	public string Text => _text.ToString();

	public float TextOriginX { get; set; }

	public float TextOriginY { get; set; }

	public float WrappedTextBottomPadding { get; set; }

	public bool IsWrapped
	{
		get
		{
			return _isWrapped;
		}
		set
		{
			_isWrapped = value;
			if (value)
			{
				MinWidth.Set(0f, 0f);
			}
			InternalSetText(_text, _textScale, _isLarge);
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

	public Color ShadowColor
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _shadowColor;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_shadowColor = value;
		}
	}

	public event Action OnInternalTextChange;

	public UIText(string text, float textScale = 1f, bool large = false)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		TextOriginX = 0.5f;
		TextOriginY = 0f;
		IsWrapped = false;
		WrappedTextBottomPadding = 20f;
		InternalSetText(text, textScale, large);
	}

	public UIText(LocalizedText text, float textScale = 1f, bool large = false)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		TextOriginX = 0.5f;
		TextOriginY = 0f;
		IsWrapped = false;
		WrappedTextBottomPadding = 20f;
		InternalSetText(text, textScale, large);
	}

	public override void Recalculate()
	{
		InternalSetText(_text, _textScale, _isLarge);
		base.Recalculate();
	}

	public void SetText(string text)
	{
		InternalSetText(text, _textScale, _isLarge);
	}

	public void SetText(LocalizedText text)
	{
		InternalSetText(text, _textScale, _isLarge);
	}

	public void SetText(string text, float textScale, bool large)
	{
		InternalSetText(text, textScale, large);
	}

	public void SetText(LocalizedText text, float textScale, bool large)
	{
		InternalSetText(text, textScale, large);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		VerifyTextState();
		CalculatedStyle innerDimensions = GetInnerDimensions();
		Vector2 position = innerDimensions.Position();
		if (_isLarge)
		{
			position.Y -= 10f * _textScale;
		}
		else
		{
			position.Y -= 2f * _textScale;
		}
		position.X += (innerDimensions.Width - _textSize.X) * TextOriginX;
		position.Y += (innerDimensions.Height - _textSize.Y) * TextOriginY;
		float num = _textScale;
		if (DynamicallyScaleDownToWidth && _textSize.X > innerDimensions.Width)
		{
			num *= innerDimensions.Width / _textSize.X;
		}
		DynamicSpriteFont value = (_isLarge ? FontAssets.DeathText : FontAssets.MouseText).Value;
		Vector2 vector = value.MeasureString(_visibleText);
		Color baseColor = _shadowColor * ((float)(int)((Color)(ref _color)).A / 255f);
		Vector2 origin = new Vector2(0f, 0f) * vector;
		Vector2 baseScale = default(Vector2);
		((Vector2)(ref baseScale))._002Ector(num);
		TextSnippet[] snippets = ChatManager.ParseMessage(_visibleText, _color).ToArray();
		ChatManager.ConvertNormalSnippets(snippets);
		ChatManager.DrawColorCodedStringShadow(spriteBatch, value, snippets, position, baseColor, 0f, origin, baseScale, -1f, 1.5f);
		ChatManager.DrawColorCodedString(spriteBatch, value, snippets, position, Color.White, 0f, origin, baseScale, out var _, -1f);
	}

	private void VerifyTextState()
	{
		if ((object)_lastTextReference != Text)
		{
			InternalSetText(_text, _textScale, _isLarge);
		}
	}

	private void InternalSetText(object text, float textScale, bool large)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		DynamicSpriteFont dynamicSpriteFont = (large ? FontAssets.DeathText.Value : FontAssets.MouseText.Value);
		_text = text;
		_isLarge = large;
		_textScale = textScale;
		_lastTextReference = _text.ToString();
		if (IsWrapped)
		{
			_visibleText = dynamicSpriteFont.CreateWrappedText(_lastTextReference, GetInnerDimensions().Width / _textScale);
		}
		else
		{
			_visibleText = _lastTextReference;
		}
		Vector2 vector = ChatManager.GetStringSize(dynamicSpriteFont, _visibleText, new Vector2(1f));
		Vector2 vector2 = (_textSize = ((!IsWrapped) ? (new Vector2(vector.X, large ? 32f : 16f) * textScale) : (new Vector2(vector.X, vector.Y + WrappedTextBottomPadding) * textScale)));
		if (!IsWrapped)
		{
			MinWidth.Set(vector2.X + PaddingLeft + PaddingRight, 0f);
		}
		MinHeight.Set(vector2.Y + PaddingTop + PaddingBottom, 0f);
		if (this.OnInternalTextChange != null)
		{
			this.OnInternalTextChange();
		}
	}
}
