using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;

namespace Terraria.UI.Chat;

public class TextSnippet
{
	public string Text;

	public string TextOriginal;

	public Color Color = Color.White;

	public float Scale = 1f;

	public bool CheckForHover;

	public bool DeleteWhole;

	public TextSnippet(string text = "")
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Text = text;
		TextOriginal = text;
	}

	public TextSnippet(string text, Color color, float scale = 1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Text = text;
		TextOriginal = text;
		Color = color;
		Scale = scale;
	}

	public virtual void Update()
	{
	}

	public virtual void OnHover()
	{
	}

	public virtual void OnClick()
	{
	}

	public virtual Color GetVisibleColor()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return ChatManager.WaveColor(Color);
	}

	public virtual bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		size = Vector2.Zero;
		return false;
	}

	public virtual TextSnippet CopyMorph(string newText)
	{
		TextSnippet obj = (TextSnippet)MemberwiseClone();
		obj.Text = newText;
		return obj;
	}

	public virtual float GetStringLength(DynamicSpriteFont font)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return font.MeasureString(Text).X * Scale;
	}

	public override string ToString()
	{
		return "Text: " + Text + " | OriginalText: " + TextOriginal;
	}
}
