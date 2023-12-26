using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Effects;
using Terraria.Utilities;

namespace Terraria.GameContent.Skies;

public class NebulaSky : CustomSky
{
	private struct LightPillar
	{
		public Vector2 Position;

		public float Depth;
	}

	private LightPillar[] _pillars;

	private UnifiedRandom _random = new UnifiedRandom();

	private Asset<Texture2D> _planetTexture;

	private Asset<Texture2D> _bgTexture;

	private Asset<Texture2D> _beamTexture;

	private Asset<Texture2D>[] _rockTextures;

	private bool _isActive;

	private float _fadeOpacity;

	public override void OnLoad()
	{
		_planetTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Planet");
		_bgTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Background");
		_beamTexture = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Beam");
		_rockTextures = new Asset<Texture2D>[3];
		for (int i = 0; i < _rockTextures.Length; i++)
		{
			_rockTextures[i] = Main.Assets.Request<Texture2D>("Images/Misc/NebulaSky/Rock_" + i);
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
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
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
		for (int i = 0; i < _pillars.Length; i++)
		{
			float depth = _pillars[i].Depth;
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
		Vector2 vector3 = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(-1000, -1000, 4000, 4000);
		float num3 = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
		Vector2 vector4 = default(Vector2);
		for (int j = num; j < num2; j++)
		{
			((Vector2)(ref vector4))._002Ector(1f / _pillars[j].Depth, 0.9f / _pillars[j].Depth);
			Vector2 position = _pillars[j].Position;
			position = (position - vector3) * vector4 + vector3 - Main.screenPosition;
			if (((Rectangle)(ref rectangle)).Contains((int)position.X, (int)position.Y))
			{
				float num4 = vector4.X * 450f;
				spriteBatch.Draw(_beamTexture.Value, position, (Rectangle?)null, Color.White * 0.2f * num3 * _fadeOpacity, 0f, Vector2.Zero, new Vector2(num4 / 70f, num4 / 45f), (SpriteEffects)0, 0f);
				int num5 = 0;
				for (float num6 = 0f; num6 <= 1f; num6 += 0.03f)
				{
					float num7 = 1f - (num6 + Main.GlobalTimeWrappedHourly * 0.02f + (float)Math.Sin(j)) % 1f;
					spriteBatch.Draw(_rockTextures[num5].Value, position + new Vector2((float)Math.Sin(num6 * 1582f) * (num4 * 0.5f) + num4 * 0.5f, num7 * 2000f), (Rectangle?)null, Color.White * num7 * num3 * _fadeOpacity, num7 * 20f, new Vector2((float)(_rockTextures[num5].Width() >> 1), (float)(_rockTextures[num5].Height() >> 1)), 0.9f, (SpriteEffects)0, 0f);
					num5 = (num5 + 1) % _rockTextures.Length;
				}
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
		_pillars = new LightPillar[40];
		for (int i = 0; i < _pillars.Length; i++)
		{
			_pillars[i].Position.X = (float)i / (float)_pillars.Length * ((float)Main.maxTilesX * 16f + 20000f) + _random.NextFloat() * 40f - 20f - 20000f;
			_pillars[i].Position.Y = _random.NextFloat() * 200f - 2000f;
			_pillars[i].Depth = _random.NextFloat() * 8f + 7f;
		}
		Array.Sort(_pillars, SortMethod);
	}

	private int SortMethod(LightPillar pillar1, LightPillar pillar2)
	{
		return pillar2.Depth.CompareTo(pillar1.Depth);
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
