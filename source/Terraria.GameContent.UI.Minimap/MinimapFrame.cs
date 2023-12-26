using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace Terraria.GameContent.UI.Minimap;

public class MinimapFrame : IConfigKeyHolder
{
	private class Button
	{
		public bool IsHighlighted;

		private readonly Vector2 _position;

		private readonly Asset<Texture2D> _hoverTexture;

		private readonly Action _onMouseDown;

		private Vector2 Size => new Vector2((float)_hoverTexture.Width(), (float)_hoverTexture.Height());

		public Button(Asset<Texture2D> hoverTexture, Vector2 position, Action mouseDownCallback)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_position = position;
			_hoverTexture = hoverTexture;
			_onMouseDown = mouseDownCallback;
		}

		public void Click()
		{
			_onMouseDown();
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 parentPosition)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (IsHighlighted)
			{
				spriteBatch.Draw(_hoverTexture.Value, _position + parentPosition, Color.White);
			}
		}

		public bool IsTouchingPoint(Vector2 testPoint, Vector2 parentPosition)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			Vector2 vector = _position + parentPosition + Size * 0.5f;
			Vector2 vector2 = Vector2.Max(Size, new Vector2(22f, 22f)) * 0.5f;
			Vector2 vector3 = testPoint - vector;
			if (Math.Abs(vector3.X) < vector2.X)
			{
				return Math.Abs(vector3.Y) < vector2.Y;
			}
			return false;
		}
	}

	private const float DEFAULT_ZOOM = 1.05f;

	private const float ZOOM_OUT_MULTIPLIER = 0.975f;

	private const float ZOOM_IN_MULTIPLIER = 1.025f;

	private readonly Asset<Texture2D> _frameTexture;

	private readonly Vector2 _frameOffset;

	private Button _resetButton;

	private Button _zoomInButton;

	private Button _zoomOutButton;

	public string ConfigKey { get; set; }

	public string NameKey { get; set; }

	public Vector2 MinimapPosition { get; set; }

	private Vector2 FramePosition
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return MinimapPosition + _frameOffset;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			MinimapPosition = value - _frameOffset;
		}
	}

	public MinimapFrame(Asset<Texture2D> frameTexture, Vector2 frameOffset)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		_frameTexture = frameTexture;
		_frameOffset = frameOffset;
	}

	public void SetResetButton(Asset<Texture2D> hoverTexture, Vector2 position)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_resetButton = new Button(hoverTexture, position, delegate
		{
			ResetZoom();
		});
	}

	private void ResetZoom()
	{
		Main.mapMinimapScale = 1.05f;
	}

	public void SetZoomInButton(Asset<Texture2D> hoverTexture, Vector2 position)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_zoomInButton = new Button(hoverTexture, position, delegate
		{
			ZoomInButton();
		});
	}

	private void ZoomInButton()
	{
		Main.mapMinimapScale *= 1.025f;
	}

	public void SetZoomOutButton(Asset<Texture2D> hoverTexture, Vector2 position)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_zoomOutButton = new Button(hoverTexture, position, delegate
		{
			ZoomOutButton();
		});
	}

	private void ZoomOutButton()
	{
		Main.mapMinimapScale *= 0.975f;
	}

	public void Update()
	{
		Button buttonUnderMouse = GetButtonUnderMouse();
		_zoomInButton.IsHighlighted = buttonUnderMouse == _zoomInButton;
		_zoomOutButton.IsHighlighted = buttonUnderMouse == _zoomOutButton;
		_resetButton.IsHighlighted = buttonUnderMouse == _resetButton;
		if (buttonUnderMouse == null || Main.LocalPlayer.lastMouseInterface)
		{
			return;
		}
		buttonUnderMouse.IsHighlighted = true;
		if (PlayerInput.IgnoreMouseInterface)
		{
			return;
		}
		Main.LocalPlayer.mouseInterface = true;
		if (Main.mouseLeft)
		{
			buttonUnderMouse.Click();
			if (Main.mouseLeftRelease)
			{
				SoundEngine.PlaySound(12);
			}
		}
	}

	public void DrawBackground(SpriteBatch spriteBatch)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)MinimapPosition.X - 6, (int)MinimapPosition.Y - 6, 244, 244), Color.Black * Main.mapMinimapAlpha);
	}

	public void DrawForeground(SpriteBatch spriteBatch)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(_frameTexture.Value, FramePosition, Color.White);
		_zoomInButton.Draw(spriteBatch, FramePosition);
		_zoomOutButton.Draw(spriteBatch, FramePosition);
		_resetButton.Draw(spriteBatch, FramePosition);
	}

	private Button GetButtonUnderMouse()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		Vector2 testPoint = default(Vector2);
		((Vector2)(ref testPoint))._002Ector((float)Main.mouseX, (float)Main.mouseY);
		if (_zoomInButton.IsTouchingPoint(testPoint, FramePosition))
		{
			return _zoomInButton;
		}
		if (_zoomOutButton.IsTouchingPoint(testPoint, FramePosition))
		{
			return _zoomOutButton;
		}
		if (_resetButton.IsTouchingPoint(testPoint, FramePosition))
		{
			return _resetButton;
		}
		return null;
	}

	[Conditional("DEBUG")]
	private void ValidateState()
	{
	}
}
