using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements;

public class UITextPanel<T> : UIPanel
{
	protected T _text;

	protected float _textScale = 1f;

	protected Vector2 _textSize = Vector2.Zero;

	protected bool _isLarge;

	protected Color _color = Color.White;

	protected bool _drawPanel = true;

	public float TextHAlign = 0.5f;

	public bool HideContents;

	private string _asterisks;

	public bool IsLarge => _isLarge;

	public bool DrawPanel
	{
		get
		{
			return _drawPanel;
		}
		set
		{
			_drawPanel = value;
		}
	}

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

	public UITextPanel(T text, float textScale = 1f, bool large = false)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		SetText(text, textScale, large);
	}

	public override void Recalculate()
	{
		SetText(_text, _textScale, _isLarge);
		base.Recalculate();
	}

	public void SetText(T text)
	{
		SetText(text, _textScale, _isLarge);
	}

	public virtual void SetText(T text, float textScale, bool large)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		Vector2 textSize = ChatManager.GetStringSize(large ? FontAssets.DeathText.Value : FontAssets.MouseText.Value, text.ToString(), new Vector2(textScale));
		textSize.Y = (large ? 32f : 16f) * textScale;
		_text = text;
		_textScale = textScale;
		_textSize = textSize;
		_isLarge = large;
		MinWidth.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
		MinHeight.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		if (_drawPanel)
		{
			base.DrawSelf(spriteBatch);
		}
		DrawText(spriteBatch);
	}

	protected void DrawText(SpriteBatch spriteBatch)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle innerDimensions = GetInnerDimensions();
		Vector2 pos = innerDimensions.Position();
		if (_isLarge)
		{
			pos.Y -= 10f * _textScale * _textScale;
		}
		else
		{
			pos.Y -= 2f * _textScale;
		}
		pos.X += (innerDimensions.Width - _textSize.X) * TextHAlign;
		string text = Text;
		if (HideContents)
		{
			if (_asterisks == null || _asterisks.Length != text.Length)
			{
				_asterisks = new string('*', text.Length);
			}
			text = _asterisks;
		}
		if (_isLarge)
		{
			Utils.DrawBorderStringBig(spriteBatch, text, pos, _color, _textScale);
		}
		else
		{
			Utils.DrawBorderString(spriteBatch, text, pos, _color, _textScale);
		}
	}
}
