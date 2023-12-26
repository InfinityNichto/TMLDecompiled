using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI.Chat;

namespace Terraria.ModLoader.UI;

public class UIAutoScaleTextTextPanel<T> : UIPanel
{
	public bool ScalePanel;

	public bool UseInnerDimensions;

	public float TextOriginX = 0.5f;

	public float TextOriginY = 0.5f;

	private Rectangle oldInnerDimensions;

	private T _text;

	private string oldText;

	private string[] textStrings;

	private Vector2[] drawOffsets;

	public string Text
	{
		get
		{
			object obj = _text?.ToString();
			if (obj == null)
			{
				obj = string.Empty;
			}
			return (string)obj;
		}
	}

	public bool IsLarge { get; private set; }

	public bool DrawPanel { get; set; } = true;


	public float TextScaleMax { get; set; } = 1f;


	public float TextScale { get; set; } = 1f;


	public Vector2 TextSize { get; private set; } = Vector2.Zero;


	public Color TextColor { get; set; } = Color.White;


	public UIAutoScaleTextTextPanel(T text, float textScaleMax = 1f, bool large = false)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		SetText(text, textScaleMax, large);
	}

	public override void Recalculate()
	{
		base.Recalculate();
		SetText(_text, TextScaleMax, IsLarge);
	}

	public void SetText(T text)
	{
		SetText(text, TextScaleMax, IsLarge);
	}

	public virtual void SetText(T text, float textScaleMax, bool large)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		if (ScalePanel)
		{
			Vector2 textSize = ChatManager.GetStringSize(IsLarge ? FontAssets.DeathText.Value : FontAssets.MouseText.Value, Text, new Vector2(TextScaleMax));
			Width.Set(PaddingLeft + textSize.X + PaddingRight, 0f);
			Height.Set(PaddingTop + (IsLarge ? 32f : 16f) + PaddingBottom, 0f);
			base.Recalculate();
		}
		Rectangle innerDimensionsRectangle = (UseInnerDimensions ? GetInnerDimensions().ToRectangle() : GetDimensions().ToRectangle());
		if (!(text.ToString() != oldText) && !(oldInnerDimensions != innerDimensionsRectangle))
		{
			return;
		}
		oldInnerDimensions = innerDimensionsRectangle;
		TextScaleMax = textScaleMax;
		DynamicSpriteFont dynamicSpriteFont = (large ? FontAssets.DeathText.Value : FontAssets.MouseText.Value);
		Vector2 textSize2 = ChatManager.GetStringSize(dynamicSpriteFont, text.ToString(), new Vector2(TextScaleMax));
		if (UseInnerDimensions)
		{
			((Rectangle)(ref innerDimensionsRectangle)).Inflate(0, 6);
		}
		else
		{
			((Rectangle)(ref innerDimensionsRectangle)).Inflate(-4, 0);
		}
		Vector2 availableSpace = default(Vector2);
		((Vector2)(ref availableSpace))._002Ector((float)innerDimensionsRectangle.Width, (float)innerDimensionsRectangle.Height);
		if (textSize2.X > availableSpace.X || textSize2.Y > availableSpace.Y)
		{
			float scale = ((textSize2.X / availableSpace.X > textSize2.Y / availableSpace.Y) ? (availableSpace.X / textSize2.X) : (availableSpace.Y / textSize2.Y));
			TextScale = scale;
			textSize2 = ChatManager.GetStringSize(dynamicSpriteFont, text.ToString(), new Vector2(TextScale));
		}
		else
		{
			TextScale = TextScaleMax;
		}
		if (!UseInnerDimensions)
		{
			innerDimensionsRectangle.Y += 8;
			innerDimensionsRectangle.Height -= 8;
		}
		_text = text;
		oldText = _text?.ToString();
		TextSize = textSize2;
		IsLarge = large;
		textStrings = text.ToString().Split('\n');
		drawOffsets = (Vector2[])(object)new Vector2[textStrings.Length];
		for (int i = 0; i < textStrings.Length; i++)
		{
			Vector2 size = ChatManager.GetStringSize(dynamicSpriteFont, textStrings[i], new Vector2(TextScale));
			if (UseInnerDimensions)
			{
				size.Y = (IsLarge ? 32f : 16f);
			}
			float x = ((float)innerDimensionsRectangle.Width - size.X) * TextOriginX;
			float y = (float)(-textStrings.Length) * size.Y * TextOriginY + (float)i * size.Y + (float)innerDimensionsRectangle.Height * TextOriginY;
			if (UseInnerDimensions)
			{
				y -= 2f;
			}
			drawOffsets[i] = new Vector2(x, y);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		if (float.IsNaN(TextScale))
		{
			Recalculate();
		}
		if (DrawPanel)
		{
			base.DrawSelf(spriteBatch);
		}
		Rectangle innerDimensions = (UseInnerDimensions ? GetInnerDimensions().ToRectangle() : GetDimensions().ToRectangle());
		if (UseInnerDimensions)
		{
			((Rectangle)(ref innerDimensions)).Inflate(0, 6);
		}
		else
		{
			((Rectangle)(ref innerDimensions)).Inflate(-4, -8);
		}
		for (int i = 0; i < textStrings.Length; i++)
		{
			Vector2 pos = innerDimensions.TopLeft() + drawOffsets[i];
			if (IsLarge)
			{
				Utils.DrawBorderStringBig(spriteBatch, textStrings[i], pos, TextColor, TextScale);
			}
			else
			{
				Utils.DrawBorderString(spriteBatch, textStrings[i], pos, TextColor, TextScale);
			}
		}
	}
}
