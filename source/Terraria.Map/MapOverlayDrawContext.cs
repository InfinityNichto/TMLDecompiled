using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.UI;

namespace Terraria.Map;

public struct MapOverlayDrawContext
{
	public struct DrawResult
	{
		public static readonly DrawResult Culled = new DrawResult(isMouseOver: false);

		public readonly bool IsMouseOver;

		public DrawResult(bool isMouseOver)
		{
			IsMouseOver = isMouseOver;
		}
	}

	private readonly Vector2 _mapPosition;

	private readonly Vector2 _mapOffset;

	private readonly Rectangle? _clippingRect;

	private readonly float _mapScale;

	private readonly float _drawScale;

	public MapOverlayDrawContext(Vector2 mapPosition, Vector2 mapOffset, Rectangle? clippingRect, float mapScale, float drawScale)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		_mapPosition = mapPosition;
		_mapOffset = mapOffset;
		_clippingRect = clippingRect;
		_mapScale = mapScale;
		_drawScale = drawScale;
	}

	public DrawResult Draw(Texture2D texture, Vector2 position, Alignment alignment)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return Draw(texture, position, new SpriteFrame(1, 1), alignment);
	}

	public DrawResult Draw(Texture2D texture, Vector2 position, SpriteFrame frame, Alignment alignment)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		position = (position - _mapPosition) * _mapScale + _mapOffset;
		Rectangle val;
		if (_clippingRect.HasValue)
		{
			val = _clippingRect.Value;
			if (!((Rectangle)(ref val)).Contains(position.ToPoint()))
			{
				return DrawResult.Culled;
			}
		}
		Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
		Vector2 vector = sourceRectangle.Size() * alignment.OffsetMultiplier;
		Main.spriteBatch.Draw(texture, position, (Rectangle?)sourceRectangle, Color.White, 0f, vector, _drawScale, (SpriteEffects)0, 0f);
		position -= vector * _drawScale;
		val = new Rectangle((int)position.X, (int)position.Y, (int)((float)texture.Width * _drawScale), (int)((float)texture.Height * _drawScale));
		return new DrawResult(((Rectangle)(ref val)).Contains(Main.MouseScreen.ToPoint()));
	}

	public DrawResult Draw(Texture2D texture, Vector2 position, Color color, SpriteFrame frame, float scaleIfNotSelected, float scaleIfSelected, Alignment alignment)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		position = (position - _mapPosition) * _mapScale + _mapOffset;
		Rectangle val;
		if (_clippingRect.HasValue)
		{
			val = _clippingRect.Value;
			if (!((Rectangle)(ref val)).Contains(position.ToPoint()))
			{
				return DrawResult.Culled;
			}
		}
		Rectangle sourceRectangle = frame.GetSourceRectangle(texture);
		Vector2 vector = sourceRectangle.Size() * alignment.OffsetMultiplier;
		Vector2 position2 = position;
		float num = _drawScale * scaleIfNotSelected;
		Vector2 vector2 = position - vector * num;
		val = new Rectangle((int)vector2.X, (int)vector2.Y, (int)((float)sourceRectangle.Width * num), (int)((float)sourceRectangle.Height * num));
		bool num2 = ((Rectangle)(ref val)).Contains(Main.MouseScreen.ToPoint());
		float scale = num;
		if (num2)
		{
			scale = _drawScale * scaleIfSelected;
		}
		Main.spriteBatch.Draw(texture, position2, (Rectangle?)sourceRectangle, color, 0f, vector, scale, (SpriteEffects)0, 0f);
		return new DrawResult(num2);
	}
}
