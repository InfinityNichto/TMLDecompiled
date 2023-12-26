using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class StardustSky : CustomSky
{
	private struct Star
	{
		public Vector2 Position;

		public float Depth;

		public int TextureIndex;

		public float SinOffset;

		public float AlphaFrequency;

		public float AlphaAmplitude;
	}

	private UnifiedRandom _random = new UnifiedRandom();

	private Asset<Texture2D> _planetTexture;

	private Asset<Texture2D> _bgTexture;

	private Asset<Texture2D>[] _starTextures;

	private bool _isActive;

	private Star[] _stars;

	private float _fadeOpacity;

	public override void OnLoad()
	{
		_planetTexture = Main.Assets.Request<Texture2D>("Images/Misc/StarDustSky/Planet");
		_bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/StarDustSky/Background");
		_starTextures = new Asset<Texture2D>[2];
		for (int i = 0; i < _starTextures.Length; i++)
		{
			_starTextures[i] = Main.Assets.Request<Texture2D>("Images/Misc/StarDustSky/Star " + i);
		}
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
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
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
		for (int i = 0; i < _stars.Length; i++)
		{
			float depth = _stars[i].Depth;
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
			((Vector2)(ref vector4))._002Ector(1f / _stars[j].Depth, 1.1f / _stars[j].Depth);
			Vector2 position = (_stars[j].Position - vector3) * vector4 + vector3 - Main.screenPosition;
			if (((Rectangle)(ref rectangle)).Contains((int)position.X, (int)position.Y))
			{
				float value = (float)Math.Sin(_stars[j].AlphaFrequency * Main.GlobalTimeWrappedHourly + _stars[j].SinOffset) * _stars[j].AlphaAmplitude + _stars[j].AlphaAmplitude;
				float num4 = (float)Math.Sin(_stars[j].AlphaFrequency * Main.GlobalTimeWrappedHourly * 5f + _stars[j].SinOffset) * 0.1f - 0.1f;
				value = MathHelper.Clamp(value, 0f, 1f);
				Texture2D value2 = _starTextures[_stars[j].TextureIndex].Value;
				spriteBatch.Draw(value2, position, (Rectangle?)null, Color.White * num3 * value * 0.8f * (1f - num4) * _fadeOpacity, 0f, new Vector2((float)(value2.Width >> 1), (float)(value2.Height >> 1)), (vector4.X * 0.5f + 0.5f) * (value * 0.3f + 0.7f), (SpriteEffects)0, 0f);
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
		int num = 200;
		int num2 = 10;
		_stars = new Star[num * num2];
		int num3 = 0;
		for (int i = 0; i < num; i++)
		{
			float num4 = (float)i / (float)num;
			for (int j = 0; j < num2; j++)
			{
				float num5 = (float)j / (float)num2;
				_stars[num3].Position.X = num4 * (float)Main.maxTilesX * 16f;
				_stars[num3].Position.Y = num5 * ((float)Main.worldSurface * 16f + 2000f) - 1000f;
				_stars[num3].Depth = _random.NextFloat() * 8f + 1.5f;
				_stars[num3].TextureIndex = _random.Next(_starTextures.Length);
				_stars[num3].SinOffset = _random.NextFloat() * 6.28f;
				_stars[num3].AlphaAmplitude = _random.NextFloat() * 5f;
				_stars[num3].AlphaFrequency = _random.NextFloat() + 1f;
				num3++;
			}
		}
		Array.Sort(_stars, SortMethod);
	}

	private int SortMethod(Star meteor1, Star meteor2)
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
