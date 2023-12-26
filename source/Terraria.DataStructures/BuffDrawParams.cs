using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures;

/// <summary>
/// Holds data required for buff drawing.
/// </summary>
public struct BuffDrawParams
{
	/// <summary>
	/// The texture used for drawing the buff.
	/// </summary>
	public Texture2D Texture;

	/// <summary>
	/// Top-left position of the buff on the screen.
	/// </summary>
	public Vector2 Position;

	/// <summary>
	/// Top left position of the text below the buff (remaining time).
	/// </summary>
	public Vector2 TextPosition;

	/// <summary>
	/// The frame displayed from the texture. Defaults to the entire texture size.
	/// </summary>
	public Rectangle SourceRectangle;

	/// <summary>
	/// Defaults to the size of the autoloaded buffs' sprite, it handles mouseovering and clicking on the buff icon.
	/// If you offset the position, or have a non-standard size, change it accordingly.
	/// </summary>
	public Rectangle MouseRectangle;

	/// <summary>
	/// Color used to draw the buff. Use Main.buffAlpha[buffIndex] accordingly if you change it.
	/// </summary>
	public Color DrawColor;

	public BuffDrawParams(Texture2D texture, Vector2 position, Vector2 textPosition, Rectangle sourceRectangle, Rectangle mouseRectangle, Color drawColor)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Texture = texture;
		Position = position;
		TextPosition = textPosition;
		SourceRectangle = sourceRectangle;
		MouseRectangle = mouseRectangle;
		DrawColor = drawColor;
	}

	public void Deconstruct(out Texture2D texture, out Vector2 position, out Vector2 textPosition, out Rectangle sourceRectangle, out Rectangle mouseRectangle, out Color drawColor)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		texture = Texture;
		position = Position;
		textPosition = TextPosition;
		sourceRectangle = SourceRectangle;
		mouseRectangle = MouseRectangle;
		drawColor = DrawColor;
	}
}
