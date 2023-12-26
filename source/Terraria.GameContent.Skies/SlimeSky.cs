using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class SlimeSky : CustomSky
{
	private struct Slime
	{
		private const int MAX_FRAMES = 4;

		private const int FRAME_RATE = 6;

		private Texture2D _texture;

		public Vector2 Position;

		public float Depth;

		public int FrameHeight;

		public int FrameWidth;

		public float Speed;

		public bool Active;

		private int _frame;

		public Texture2D Texture
		{
			get
			{
				return _texture;
			}
			set
			{
				_texture = value;
				FrameWidth = value.Width;
				FrameHeight = value.Height / 4;
			}
		}

		public int Frame
		{
			get
			{
				return _frame;
			}
			set
			{
				_frame = value % 24;
			}
		}

		public Rectangle GetSourceRectangle()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle(0, _frame / 6 * FrameHeight, FrameWidth, FrameHeight);
		}
	}

	private Asset<Texture2D>[] _textures;

	private Slime[] _slimes;

	private UnifiedRandom _random = new UnifiedRandom();

	private int _slimesRemaining;

	private bool _isActive;

	private bool _isLeaving;

	public override void OnLoad()
	{
		_textures = new Asset<Texture2D>[4];
		for (int i = 0; i < 4; i++)
		{
			_textures[i] = Main.Assets.Request<Texture2D>("Images/Misc/Sky_Slime_" + (i + 1));
		}
		GenerateSlimes();
	}

	private void GenerateSlimes()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		_slimes = new Slime[Main.maxTilesY / 6];
		for (int i = 0; i < _slimes.Length; i++)
		{
			int num = (int)((double)Main.screenPosition.Y * 0.7 - (double)Main.screenHeight);
			int minValue = (int)((double)num - Main.worldSurface * 16.0);
			_slimes[i].Position = new Vector2((float)(_random.Next(0, Main.maxTilesX) * 16), (float)_random.Next(minValue, num));
			_slimes[i].Speed = 5f + 3f * (float)_random.NextDouble();
			_slimes[i].Depth = (float)i / (float)_slimes.Length * 1.75f + 1.6f;
			_slimes[i].Texture = _textures[_random.Next(2)].Value;
			if (_random.Next(60) == 0)
			{
				_slimes[i].Texture = _textures[3].Value;
				_slimes[i].Speed = 6f + 3f * (float)_random.NextDouble();
				_slimes[i].Depth += 0.5f;
			}
			else if (_random.Next(30) == 0)
			{
				_slimes[i].Texture = _textures[2].Value;
				_slimes[i].Speed = 6f + 2f * (float)_random.NextDouble();
			}
			_slimes[i].Active = true;
		}
		_slimesRemaining = _slimes.Length;
	}

	public override void Update(GameTime gameTime)
	{
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gamePaused || !Main.hasFocus)
		{
			return;
		}
		for (int i = 0; i < _slimes.Length; i++)
		{
			if (!_slimes[i].Active)
			{
				continue;
			}
			_slimes[i].Frame++;
			_slimes[i].Position.Y += _slimes[i].Speed;
			if (!((double)_slimes[i].Position.Y > Main.worldSurface * 16.0))
			{
				continue;
			}
			if (!_isLeaving)
			{
				_slimes[i].Depth = (float)i / (float)_slimes.Length * 1.75f + 1.6f;
				_slimes[i].Position = new Vector2((float)(_random.Next(0, Main.maxTilesX) * 16), -100f);
				_slimes[i].Texture = _textures[_random.Next(2)].Value;
				_slimes[i].Speed = 5f + 3f * (float)_random.NextDouble();
				if (_random.Next(60) == 0)
				{
					_slimes[i].Texture = _textures[3].Value;
					_slimes[i].Speed = 6f + 3f * (float)_random.NextDouble();
					_slimes[i].Depth += 0.5f;
				}
				else if (_random.Next(30) == 0)
				{
					_slimes[i].Texture = _textures[2].Value;
					_slimes[i].Speed = 6f + 2f * (float)_random.NextDouble();
				}
			}
			else
			{
				_slimes[i].Active = false;
				_slimesRemaining--;
			}
		}
		if (_slimesRemaining == 0)
		{
			_isActive = false;
		}
	}

	public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		if (Main.screenPosition.Y > 10000f || Main.gameMenu)
		{
			return;
		}
		int num = -1;
		int num2 = 0;
		for (int i = 0; i < _slimes.Length; i++)
		{
			float depth = _slimes[i].Depth;
			if (num == -1 && depth < maxDepth)
			{
				num = i;
			}
			if (depth <= minDepth)
			{
				break;
			}
			num2 = i;
		}
		if (num == -1)
		{
			return;
		}
		Vector2 vector = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(-1000, -1000, 4000, 4000);
		Vector2 vector2 = default(Vector2);
		for (int j = num; j < num2; j++)
		{
			if (_slimes[j].Active)
			{
				Color color = new Color(((Color)(ref Main.ColorOfTheSkies)).ToVector4() * 0.9f + new Vector4(0.1f)) * 0.8f;
				float num3 = 1f;
				if (_slimes[j].Depth > 3f)
				{
					num3 = 0.6f;
				}
				else if ((double)_slimes[j].Depth > 2.5)
				{
					num3 = 0.7f;
				}
				else if (_slimes[j].Depth > 2f)
				{
					num3 = 0.8f;
				}
				else if ((double)_slimes[j].Depth > 1.5)
				{
					num3 = 0.9f;
				}
				num3 *= 0.8f;
				((Color)(ref color))._002Ector((int)((float)(int)((Color)(ref color)).R * num3), (int)((float)(int)((Color)(ref color)).G * num3), (int)((float)(int)((Color)(ref color)).B * num3), (int)((float)(int)((Color)(ref color)).A * num3));
				((Vector2)(ref vector2))._002Ector(1f / _slimes[j].Depth, 0.9f / _slimes[j].Depth);
				Vector2 position = _slimes[j].Position;
				position = (position - vector) * vector2 + vector - Main.screenPosition;
				position.X = (position.X + 500f) % 4000f;
				if (position.X < 0f)
				{
					position.X += 4000f;
				}
				position.X -= 500f;
				if (((Rectangle)(ref rectangle)).Contains((int)position.X, (int)position.Y))
				{
					spriteBatch.Draw(_slimes[j].Texture, position, (Rectangle?)_slimes[j].GetSourceRectangle(), color, 0f, Vector2.Zero, vector2.X * 2f, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	public override void Activate(Vector2 position, params object[] args)
	{
		GenerateSlimes();
		_isActive = true;
		_isLeaving = false;
	}

	public override void Deactivate(params object[] args)
	{
		_isLeaving = true;
	}

	public override void Reset()
	{
		_isActive = false;
	}

	public override bool IsActive()
	{
		return _isActive;
	}
}
