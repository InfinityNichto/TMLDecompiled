using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Events;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class LanternSky : CustomSky
{
	private struct Lantern
	{
		private const int MAX_FRAMES_X = 3;

		public int Variant;

		public int TimeUntilFloat;

		public int TimeUntilFloatMax;

		private Texture2D _texture;

		public Vector2 Position;

		public float Depth;

		public float Rotation;

		public int FrameHeight;

		public int FrameWidth;

		public float Speed;

		public bool Active;

		public Texture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				_texture = value;
				FrameWidth = value.Width / 3;
				FrameHeight = value.Height;
			}
		}

		public float FloatAdjustedSpeed => Speed * ((float)TimeUntilFloat / (float)TimeUntilFloatMax);

		public Rectangle GetSourceRectangle()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle(FrameWidth * Variant, 0, FrameWidth, FrameHeight);
		}
	}

	private bool _active;

	private bool _leaving;

	private float _opacity;

	private Asset<Texture2D> _texture;

	private Lantern[] _lanterns;

	private UnifiedRandom _random = new UnifiedRandom();

	private int _lanternsDrawing;

	private const float slowDown = 0.5f;

	public override void OnLoad()
	{
		_texture = TextureAssets.Extra[134];
		GenerateLanterns(onlyMissing: false);
	}

	private void GenerateLanterns(bool onlyMissing)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		if (!onlyMissing)
		{
			_lanterns = new Lantern[Main.maxTilesY / 4];
		}
		for (int i = 0; i < _lanterns.Length; i++)
		{
			if (!onlyMissing || !_lanterns[i].Active)
			{
				int num = (int)((double)Main.screenPosition.Y * 0.7 - (double)Main.screenHeight);
				int minValue = (int)((double)num - Main.worldSurface * 16.0);
				_lanterns[i].Position = new Vector2((float)(_random.Next(0, Main.maxTilesX) * 16), (float)_random.Next(minValue, num));
				ResetLantern(i);
				_lanterns[i].Active = true;
			}
		}
		_lanternsDrawing = _lanterns.Length;
	}

	public void ResetLantern(int i)
	{
		_lanterns[i].Depth = (1f - (float)i / (float)_lanterns.Length) * 4.4f + 1.6f;
		_lanterns[i].Speed = -1.5f - 2.5f * (float)_random.NextDouble();
		_lanterns[i].Texture = _texture.Value;
		_lanterns[i].Variant = _random.Next(3);
		_lanterns[i].TimeUntilFloat = (int)((float)(2000 + _random.Next(1200)) * 2f);
		_lanterns[i].TimeUntilFloatMax = _lanterns[i].TimeUntilFloat;
	}

	public override void Update(GameTime gameTime)
	{
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gamePaused || !Main.hasFocus)
		{
			return;
		}
		_opacity = Utils.Clamp(_opacity + (float)LanternNight.LanternsUp.ToDirectionInt() * 0.01f, 0f, 1f);
		for (int i = 0; i < _lanterns.Length; i++)
		{
			if (!_lanterns[i].Active)
			{
				continue;
			}
			float num = Main.windSpeedCurrent;
			if (num == 0f)
			{
				num = 0.1f;
			}
			float num2 = (float)Math.Sin(_lanterns[i].Position.X / 120f) * 0.5f;
			_lanterns[i].Position.Y += num2 * 0.5f;
			_lanterns[i].Position.Y += _lanterns[i].FloatAdjustedSpeed * 0.5f;
			_lanterns[i].Position.X += (0.1f + num) * (3f - _lanterns[i].Speed) * 0.5f * ((float)i / (float)_lanterns.Length + 1.5f) / 2.5f;
			_lanterns[i].Rotation = num2 * (float)((!(num < 0f)) ? 1 : (-1)) * 0.5f;
			_lanterns[i].TimeUntilFloat = Math.Max(0, _lanterns[i].TimeUntilFloat - 1);
			if (_lanterns[i].Position.Y < 300f)
			{
				if (!_leaving)
				{
					ResetLantern(i);
					_lanterns[i].Position = new Vector2((float)(_random.Next(0, Main.maxTilesX) * 16), (float)Main.worldSurface * 16f + 1600f);
				}
				else
				{
					_lanterns[i].Active = false;
					_lanternsDrawing--;
				}
			}
		}
		_active = true;
	}

	public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu && _active)
		{
			_active = false;
			_leaving = false;
			for (int i = 0; i < _lanterns.Length; i++)
			{
				_lanterns[i].Active = false;
			}
		}
		if ((double)Main.screenPosition.Y > Main.worldSurface * 16.0 || Main.gameMenu || _opacity <= 0f)
		{
			return;
		}
		int num = -1;
		int num2 = 0;
		for (int j = 0; j < _lanterns.Length; j++)
		{
			float depth = _lanterns[j].Depth;
			if (num == -1 && depth < maxDepth)
			{
				num = j;
			}
			if (depth <= minDepth)
			{
				break;
			}
			num2 = j;
		}
		if (num == -1)
		{
			return;
		}
		Vector2 vector = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(-1000, -1000, 4000, 4000);
		Color color = default(Color);
		Vector2 vector2 = default(Vector2);
		for (int k = num; k < num2; k++)
		{
			if (_lanterns[k].Active)
			{
				((Color)(ref color))._002Ector(250, 120, 60, 120);
				float num3 = 1f;
				if (_lanterns[k].Depth > 5f)
				{
					num3 = 0.3f;
				}
				else if ((double)_lanterns[k].Depth > 4.5)
				{
					num3 = 0.4f;
				}
				else if (_lanterns[k].Depth > 4f)
				{
					num3 = 0.5f;
				}
				else if ((double)_lanterns[k].Depth > 3.5)
				{
					num3 = 0.6f;
				}
				else if (_lanterns[k].Depth > 3f)
				{
					num3 = 0.7f;
				}
				else if ((double)_lanterns[k].Depth > 2.5)
				{
					num3 = 0.8f;
				}
				else if (_lanterns[k].Depth > 2f)
				{
					num3 = 0.9f;
				}
				((Color)(ref color))._002Ector((int)((float)(int)((Color)(ref color)).R * num3), (int)((float)(int)((Color)(ref color)).G * num3), (int)((float)(int)((Color)(ref color)).B * num3), (int)((float)(int)((Color)(ref color)).A * num3));
				((Vector2)(ref vector2))._002Ector(1f / _lanterns[k].Depth, 0.9f / _lanterns[k].Depth);
				vector2 *= 1.2f;
				Vector2 position = _lanterns[k].Position;
				position = (position - vector) * vector2 + vector - Main.screenPosition;
				position.X = (position.X + 500f) % 4000f;
				if (position.X < 0f)
				{
					position.X += 4000f;
				}
				position.X -= 500f;
				if (((Rectangle)(ref rectangle)).Contains((int)position.X, (int)position.Y))
				{
					DrawLantern(spriteBatch, _lanterns[k], color, vector2, position, num3);
				}
			}
		}
	}

	private void DrawLantern(SpriteBatch spriteBatch, Lantern lantern, Color opacity, Vector2 depthScale, Vector2 position, float alpha)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		float y = (Main.GlobalTimeWrappedHourly % 6f / 6f * ((float)Math.PI * 2f)).ToRotationVector2().Y;
		float num = y * 0.2f + 0.8f;
		Color color = new Color(255, 255, 255, 0) * _opacity * alpha * num * 0.4f;
		for (float num2 = 0f; num2 < 1f; num2 += 1f / 3f)
		{
			Vector2 vector = Utils.RotatedBy(new Vector2(0f, 2f), (float)Math.PI * 2f * num2 + lantern.Rotation) * y;
			spriteBatch.Draw(lantern.Texture, position + vector, (Rectangle?)lantern.GetSourceRectangle(), color, lantern.Rotation, lantern.GetSourceRectangle().Size() / 2f, depthScale.X * 2f, (SpriteEffects)0, 0f);
		}
		spriteBatch.Draw(lantern.Texture, position, (Rectangle?)lantern.GetSourceRectangle(), opacity * _opacity, lantern.Rotation, lantern.GetSourceRectangle().Size() / 2f, depthScale.X * 2f, (SpriteEffects)0, 0f);
	}

	public override void Activate(Vector2 position, params object[] args)
	{
		if (_active)
		{
			_leaving = false;
			GenerateLanterns(onlyMissing: true);
		}
		else
		{
			GenerateLanterns(onlyMissing: false);
			_active = true;
			_leaving = false;
		}
	}

	public override void Deactivate(params object[] args)
	{
		_leaving = true;
	}

	public override bool IsActive()
	{
		return _active;
	}

	public override void Reset()
	{
		_active = false;
	}
}
