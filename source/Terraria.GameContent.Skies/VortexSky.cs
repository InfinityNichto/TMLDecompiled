using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class VortexSky : CustomSky
{
	private struct Bolt
	{
		public Vector2 Position;

		public float Depth;

		public int Life;

		public bool IsAlive;
	}

	private UnifiedRandom _random = new UnifiedRandom();

	private Asset<Texture2D> _planetTexture;

	private Asset<Texture2D> _bgTexture;

	private Asset<Texture2D> _boltTexture;

	private Asset<Texture2D> _flashTexture;

	private bool _isActive;

	private int _ticksUntilNextBolt;

	private float _fadeOpacity;

	private Bolt[] _bolts;

	public override void OnLoad()
	{
		_planetTexture = Main.Assets.Request<Texture2D>("Images/Misc/VortexSky/Planet");
		_bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/VortexSky/Background");
		_boltTexture = Main.Assets.Request<Texture2D>("Images/Misc/VortexSky/Bolt");
		_flashTexture = Main.Assets.Request<Texture2D>("Images/Misc/VortexSky/Flash");
	}

	public override void Update(GameTime gameTime)
	{
		if (_isActive)
		{
			_fadeOpacity = Math.Min(1f, 0.01f + _fadeOpacity);
		}
		else
		{
			_fadeOpacity = Math.Max(0f, _fadeOpacity - 0.01f);
		}
		if (_ticksUntilNextBolt <= 0)
		{
			_ticksUntilNextBolt = _random.Next(1, 5);
			int i;
			for (i = 0; _bolts[i].IsAlive && i != _bolts.Length - 1; i++)
			{
			}
			_bolts[i].IsAlive = true;
			_bolts[i].Position.X = _random.NextFloat() * ((float)Main.maxTilesX * 16f + 4000f) - 2000f;
			_bolts[i].Position.Y = _random.NextFloat() * 500f;
			_bolts[i].Depth = _random.NextFloat() * 8f + 2f;
			_bolts[i].Life = 30;
		}
		_ticksUntilNextBolt--;
		for (int j = 0; j < _bolts.Length; j++)
		{
			if (_bolts[j].IsAlive)
			{
				_bolts[j].Life--;
				if (_bolts[j].Life <= 0)
				{
					_bolts[j].IsAlive = false;
				}
			}
		}
	}

	public override Color OnTileColor(Color inColor)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		return new Color(Vector4.Lerp(((Color)(ref inColor)).ToVector4(), Vector4.One, _fadeOpacity * 0.5f));
	}

	public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		if (maxDepth >= float.MaxValue && minDepth < float.MaxValue)
		{
			spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * _fadeOpacity);
			spriteBatch.Draw(_bgTexture.Value, new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 2400.0) * 0.10000000149011612)), Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f) * _fadeOpacity);
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
			Vector2 vector2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
			spriteBatch.Draw(_planetTexture.Value, vector + new Vector2(-200f, -200f) + vector2, (Rectangle?)null, Color.White * 0.9f * _fadeOpacity, 0f, new Vector2((float)(_planetTexture.Width() >> 1), (float)(_planetTexture.Height() >> 1)), 1f, (SpriteEffects)0, 1f);
		}
		float num = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
		Vector2 vector3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(-1000, -1000, 4000, 4000);
		Vector2 vector4 = default(Vector2);
		for (int i = 0; i < _bolts.Length; i++)
		{
			if (!_bolts[i].IsAlive || !(_bolts[i].Depth > minDepth) || !(_bolts[i].Depth < maxDepth))
			{
				continue;
			}
			((Vector2)(ref vector4))._002Ector(1f / _bolts[i].Depth, 0.9f / _bolts[i].Depth);
			Vector2 position = (_bolts[i].Position - vector3) * vector4 + vector3 - Main.screenPosition;
			if (((Rectangle)(ref rectangle)).Contains((int)position.X, (int)position.Y))
			{
				Texture2D value = _boltTexture.Value;
				int life = _bolts[i].Life;
				if (life > 26 && life % 2 == 0)
				{
					value = _flashTexture.Value;
				}
				float num2 = (float)life / 30f;
				spriteBatch.Draw(value, position, (Rectangle?)null, Color.White * num * num2 * _fadeOpacity, 0f, Vector2.Zero, vector4.X * 5f, (SpriteEffects)0, 0f);
			}
		}
	}

	public override float GetCloudAlpha()
	{
		return (1f - _fadeOpacity) * 0.3f + 0.7f;
	}

	public override void Activate(Vector2 position, params object[] args)
	{
		_fadeOpacity = 0.002f;
		_isActive = true;
		_bolts = new Bolt[500];
		for (int i = 0; i < _bolts.Length; i++)
		{
			_bolts[i].IsAlive = false;
		}
	}

	public override void Deactivate(params object[] args)
	{
		_isActive = false;
	}

	public override void Reset()
	{
		_isActive = false;
	}

	public override bool IsActive()
	{
		if (!_isActive)
		{
			return _fadeOpacity > 0.001f;
		}
		return true;
	}
}
