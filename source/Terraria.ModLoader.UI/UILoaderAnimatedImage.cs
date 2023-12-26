using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal sealed class UILoaderAnimatedImage : UIElement
{
	public const int MAX_FRAMES = 16;

	public const int MAX_DELAY = 5;

	public bool WithBackground;

	public int FrameTick;

	public int Frame;

	private readonly float _scale;

	private Asset<Texture2D> _backgroundTexture;

	private Asset<Texture2D> _loaderTexture;

	public UILoaderAnimatedImage(float left, float top, float scale = 1f)
	{
		_scale = scale;
		Width.Pixels = 200f * scale;
		Height.Pixels = 200f * scale;
		HAlign = left;
		VAlign = top;
	}

	public override void OnInitialize()
	{
		_backgroundTexture = UICommon.LoaderBgTexture;
		_loaderTexture = UICommon.LoaderTexture;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		if (++FrameTick >= 5)
		{
			FrameTick = 0;
			if (++Frame >= 16)
			{
				Frame = 0;
			}
		}
		CalculatedStyle dimensions = GetDimensions();
		if (WithBackground)
		{
			spriteBatch.Draw(_backgroundTexture.Value, new Vector2((float)(int)dimensions.X, (float)(int)dimensions.Y), (Rectangle?)new Rectangle(0, 0, 200, 200), Color.White, 0f, new Vector2(0f, 0f), _scale, (SpriteEffects)0, 0f);
		}
		spriteBatch.Draw(_loaderTexture.Value, new Vector2((float)(int)dimensions.X, (float)(int)dimensions.Y), (Rectangle?)new Rectangle(200 * (Frame / 8), 200 * (Frame % 8), 200, 200), Color.White, 0f, new Vector2(0f, 0f), _scale, (SpriteEffects)0, 0f);
	}
}
