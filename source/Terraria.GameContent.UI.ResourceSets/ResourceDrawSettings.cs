using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace Terraria.GameContent.UI.ResourceSets;

public struct ResourceDrawSettings
{
	public delegate void TextureGetter(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> texture, out Vector2 drawOffset, out float drawScale, out Rectangle? sourceRect);

	public Vector2 TopLeftAnchor;

	public int ElementCount;

	public int ElementIndexOffset;

	public TextureGetter GetTextureMethod;

	public Vector2 OffsetPerDraw;

	public Vector2 OffsetPerDrawByTexturePercentile;

	public Vector2 OffsetSpriteAnchor;

	public Vector2 OffsetSpriteAnchorByTexturePercentile;

	public PlayerStatsSnapshot StatsSnapshot;

	public IPlayerResourcesDisplaySet DisplaySet;

	public int ResourceIndexOffset;

	public void Draw(SpriteBatch spriteBatch, ref bool isHovered)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		int elementCount = ElementCount;
		Vector2 topLeftAnchor = TopLeftAnchor;
		Point value = Main.MouseScreen.ToPoint();
		for (int i = 0; i < elementCount; i++)
		{
			int elementIndex = i + ElementIndexOffset;
			GetTextureMethod(elementIndex, ElementIndexOffset, ElementIndexOffset + elementCount - 1, out var texture, out var drawOffset, out var drawScale, out var sourceRect);
			Rectangle rectangle = texture.Frame();
			if (sourceRect.HasValue)
			{
				rectangle = sourceRect.Value;
			}
			Vector2 position = topLeftAnchor + drawOffset;
			Vector2 origin = OffsetSpriteAnchor + rectangle.Size() * OffsetSpriteAnchorByTexturePercentile;
			Rectangle rectangle2 = rectangle;
			rectangle2.X += (int)(position.X - origin.X);
			rectangle2.Y += (int)(position.Y - origin.Y);
			if (((Rectangle)(ref rectangle2)).Contains(value))
			{
				isHovered = true;
			}
			ResourceOverlayDrawContext drawContext = new ResourceOverlayDrawContext(StatsSnapshot, DisplaySet, elementIndex + ResourceIndexOffset, texture);
			drawContext.position = position;
			drawContext.source = rectangle;
			drawContext.origin = origin;
			drawContext.scale = new Vector2(drawScale);
			drawContext.SpriteBatch = spriteBatch;
			ResourceOverlayLoader.DrawResource(drawContext);
			topLeftAnchor += OffsetPerDraw + rectangle.Size() * OffsetPerDrawByTexturePercentile;
		}
	}
}
