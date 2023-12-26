using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

public class UIAnimatedImage : UIElement
{
	private readonly Asset<Texture2D> _texture;

	private readonly int _padding;

	private readonly int _textureOffsetX;

	private readonly int _textureOffsetY;

	private readonly int _countX;

	private readonly int _countY;

	private int _tickCounter;

	private int _frameCounter;

	public int FrameStart { get; set; }

	public int FrameCount { get; set; } = 1;


	public int TicksPerFrame { get; set; } = 5;


	protected int DrawHeight => (int)Height.Pixels;

	protected int DrawWidth => (int)Width.Pixels;

	public UIAnimatedImage(Asset<Texture2D> texture, int width, int height, int textureOffsetX, int textureOffsetY, int countX, int countY, int padding = 2)
	{
		_texture = texture;
		_textureOffsetX = textureOffsetX;
		_textureOffsetY = textureOffsetY;
		_countX = countX;
		_countY = countY;
		Width.Pixels = width;
		Height.Pixels = height;
		_padding = padding;
	}

	private Rectangle FrameToRect(int frame)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		if (frame < 0 || frame >= _countX * _countY)
		{
			return new Rectangle(0, 0, 0, 0);
		}
		int x = frame % _countX;
		int y = frame / _countX;
		return new Rectangle(_textureOffsetX + (_padding + DrawHeight) * x, _textureOffsetY + (_padding + DrawHeight) * y, DrawWidth, DrawHeight);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (++_tickCounter >= TicksPerFrame)
		{
			_tickCounter = 0;
			if (++_frameCounter >= FrameCount)
			{
				_frameCounter = 0;
			}
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Color color = (base.IsMouseHovering ? Color.White : Color.Silver);
		int frame = FrameStart + _frameCounter % FrameCount;
		spriteBatch.Draw(_texture.Value, dimensions.ToRectangle(), (Rectangle?)FrameToRect(frame), color);
	}
}
