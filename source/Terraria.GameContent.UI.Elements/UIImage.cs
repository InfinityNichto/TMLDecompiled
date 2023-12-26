using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIImage : UIElement
{
	private Asset<Texture2D> _texture;

	public float ImageScale = 1f;

	public float Rotation;

	public bool ScaleToFit;

	public bool AllowResizingDimensions = true;

	public Color Color = Color.White;

	public Vector2 NormalizedOrigin = Vector2.Zero;

	public bool RemoveFloatingPointsFromDrawPosition;

	private Texture2D _nonReloadingTexture;

	public UIImage(Asset<Texture2D> texture)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		SetImage(texture);
	}

	public UIImage(Texture2D nonReloadingTexture)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		SetImage(nonReloadingTexture);
	}

	public void SetImage(Asset<Texture2D> texture)
	{
		_texture = texture;
		_nonReloadingTexture = null;
		if (AllowResizingDimensions)
		{
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}
	}

	public void SetImage(Texture2D nonReloadingTexture)
	{
		_texture = null;
		_nonReloadingTexture = nonReloadingTexture;
		if (AllowResizingDimensions)
		{
			Width.Set(_nonReloadingTexture.Width, 0f);
			Height.Set(_nonReloadingTexture.Height, 0f);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Texture2D texture2D = null;
		if (_texture != null)
		{
			texture2D = _texture.Value;
		}
		if (_nonReloadingTexture != null)
		{
			texture2D = _nonReloadingTexture;
		}
		if (ScaleToFit)
		{
			spriteBatch.Draw(texture2D, dimensions.ToRectangle(), Color);
			return;
		}
		Vector2 vector = texture2D.Size();
		Vector2 vector2 = dimensions.Position() + vector * (1f - ImageScale) / 2f + vector * NormalizedOrigin;
		if (RemoveFloatingPointsFromDrawPosition)
		{
			vector2 = vector2.Floor();
		}
		spriteBatch.Draw(texture2D, vector2, (Rectangle?)null, Color, Rotation, vector * NormalizedOrigin, ImageScale, (SpriteEffects)0, 0f);
	}
}
