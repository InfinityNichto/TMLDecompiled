using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIImageButton : UIElement
{
	private Asset<Texture2D> _texture;

	private float _visibilityActive = 1f;

	private float _visibilityInactive = 0.4f;

	private Asset<Texture2D> _borderTexture;

	public UIImageButton(Asset<Texture2D> texture)
	{
		_texture = texture;
		Width.Set(_texture.Width(), 0f);
		Height.Set(_texture.Height(), 0f);
	}

	public void SetHoverImage(Asset<Texture2D> texture)
	{
		_borderTexture = texture;
	}

	public void SetImage(Asset<Texture2D> texture)
	{
		_texture = texture;
		Width.Set(_texture.Width(), 0f);
		Height.Set(_texture.Height(), 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		spriteBatch.Draw(_texture.Value, dimensions.Position(), Color.White * (base.IsMouseHovering ? _visibilityActive : _visibilityInactive));
		if (_borderTexture != null && base.IsMouseHovering)
		{
			spriteBatch.Draw(_borderTexture.Value, dimensions.Position(), Color.White);
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		SoundEngine.PlaySound(12);
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
	}

	public void SetVisibility(float whenActive, float whenInactive)
	{
		_visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
		_visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
	}
}
