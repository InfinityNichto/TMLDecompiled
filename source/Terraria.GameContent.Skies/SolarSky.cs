using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class SolarSky : CustomSky
{
	private struct Meteor
	{
		public Vector2 Position;

		public float Depth;

		public int FrameCounter;

		public float Scale;

		public float StartX;
	}

	private UnifiedRandom _random = new UnifiedRandom();

	private Asset<Texture2D> _planetTexture;

	private Asset<Texture2D> _bgTexture;

	private Asset<Texture2D> _meteorTexture;

	private bool _isActive;

	private Meteor[] _meteors;

	private float _fadeOpacity;

	public override void OnLoad()
	{
		_planetTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Planet");
		_bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Background");
		_meteorTexture = Main.Assets.Request<Texture2D>("Images/Misc/SolarSky/Meteor");
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
		float num = 1200f;
		for (int i = 0; i < _meteors.Length; i++)
		{
			_meteors[i].Position.X -= num * (float)gameTime.ElapsedGameTime.TotalSeconds;
			_meteors[i].Position.Y += num * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if ((double)_meteors[i].Position.Y > Main.worldSurface * 16.0)
			{
				_meteors[i].Position.X = _meteors[i].StartX;
				_meteors[i].Position.Y = -10000f;
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
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		if (maxDepth >= float.MaxValue && minDepth < float.MaxValue)
		{
			spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * _fadeOpacity);
			spriteBatch.Draw(_bgTexture.Value, new Rectangle(0, Math.Max(0, (int)((Main.worldSurface * 16.0 - (double)Main.screenPosition.Y - 2400.0) * 0.10000000149011612)), Main.screenWidth, Main.screenHeight), Color.White * Math.Min(1f, (Main.screenPosition.Y - 800f) / 1000f * _fadeOpacity));
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
			Vector2 vector2 = 0.01f * (new Vector2((float)Main.maxTilesX * 8f, (float)Main.worldSurface / 2f) - Main.screenPosition);
			spriteBatch.Draw(_planetTexture.Value, vector + new Vector2(-200f, -200f) + vector2, (Rectangle?)null, Color.White * 0.9f * _fadeOpacity, 0f, new Vector2((float)(_planetTexture.Width() >> 1), (float)(_planetTexture.Height() >> 1)), 1f, (SpriteEffects)0, 1f);
		}
		int num = -1;
		int num2 = 0;
		for (int i = 0; i < _meteors.Length; i++)
		{
			float depth = _meteors[i].Depth;
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
		float num3 = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
		Vector2 vector3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(-1000, -1000, 4000, 4000);
		Vector2 vector4 = default(Vector2);
		for (int j = num; j < num2; j++)
		{
			((Vector2)(ref vector4))._002Ector(1f / _meteors[j].Depth, 0.9f / _meteors[j].Depth);
			Vector2 position = (_meteors[j].Position - vector3) * vector4 + vector3 - Main.screenPosition;
			int num4 = _meteors[j].FrameCounter / 3;
			_meteors[j].FrameCounter = (_meteors[j].FrameCounter + 1) % 12;
			if (((Rectangle)(ref rectangle)).Contains((int)position.X, (int)position.Y))
			{
				spriteBatch.Draw(_meteorTexture.Value, position, (Rectangle?)new Rectangle(0, num4 * (_meteorTexture.Height() / 4), _meteorTexture.Width(), _meteorTexture.Height() / 4), Color.White * num3 * _fadeOpacity, 0f, Vector2.Zero, vector4.X * 5f * _meteors[j].Scale, (SpriteEffects)0, 0f);
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
		_meteors = new Meteor[150];
		for (int i = 0; i < _meteors.Length; i++)
		{
			float num = (float)i / (float)_meteors.Length;
			_meteors[i].Position.X = num * ((float)Main.maxTilesX * 16f) + _random.NextFloat() * 40f - 20f;
			_meteors[i].Position.Y = _random.NextFloat() * (0f - ((float)Main.worldSurface * 16f + 10000f)) - 10000f;
			if (_random.Next(3) != 0)
			{
				_meteors[i].Depth = _random.NextFloat() * 3f + 1.8f;
			}
			else
			{
				_meteors[i].Depth = _random.NextFloat() * 5f + 4.8f;
			}
			_meteors[i].FrameCounter = _random.Next(12);
			_meteors[i].Scale = _random.NextFloat() * 0.5f + 1f;
			_meteors[i].StartX = _meteors[i].Position.X;
		}
		Array.Sort(_meteors, SortMethod);
	}

	private int SortMethod(Meteor meteor1, Meteor meteor2)
	{
		return meteor2.Depth.CompareTo(meteor1.Depth);
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
