using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

public class UICycleImage : UIElement
{
	private readonly Asset<Texture2D> _texture;

	private readonly int _padding;

	private readonly int _textureOffsetX;

	private readonly int _textureOffsetY;

	private readonly int _states;

	private int _currentState;

	public int CurrentState
	{
		get
		{
			return _currentState;
		}
		set
		{
			if (value != _currentState)
			{
				_currentState = value;
				this.OnStateChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	protected int DrawHeight => (int)Height.Pixels;

	protected int DrawWidth => (int)Width.Pixels;

	public event EventHandler OnStateChanged;

	public UICycleImage(Asset<Texture2D> texture, int states, int width, int height, int textureOffsetX, int textureOffsetY, int padding = 2)
	{
		_texture = texture;
		_textureOffsetX = textureOffsetX;
		_textureOffsetY = textureOffsetY;
		Width.Pixels = width;
		Height.Pixels = height;
		_states = states;
		_padding = padding;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Point point = default(Point);
		((Point)(ref point))._002Ector(_textureOffsetX, _textureOffsetY + (_padding + DrawHeight) * _currentState);
		Color color = (base.IsMouseHovering ? Color.White : Color.Silver);
		spriteBatch.Draw(_texture.Value, new Rectangle((int)dimensions.X, (int)dimensions.Y, DrawWidth, DrawHeight), (Rectangle?)new Rectangle(point.X, point.Y, DrawWidth, DrawHeight), color);
	}

	public override void LeftClick(UIMouseEvent evt)
	{
		CurrentState = (_currentState + 1) % _states;
		base.LeftClick(evt);
	}

	public override void RightClick(UIMouseEvent evt)
	{
		CurrentState = (_currentState + _states - 1) % _states;
		base.RightClick(evt);
	}

	internal void SetCurrentState(int state)
	{
		CurrentState = state;
	}
}
