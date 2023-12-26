using Microsoft.Xna.Framework;

namespace Terraria.UI;

public struct CalculatedStyle
{
	public float X;

	public float Y;

	public float Width;

	public float Height;

	public CalculatedStyle(float x, float y, float width, float height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public Rectangle ToRectangle()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
	}

	public Vector2 Position()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(X, Y);
	}

	public Vector2 Center()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(X + Width * 0.5f, Y + Height * 0.5f);
	}
}
