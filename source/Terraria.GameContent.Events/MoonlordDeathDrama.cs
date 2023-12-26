using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Utilities;

namespace Terraria.GameContent.Events;

public class MoonlordDeathDrama
{
	public class MoonlordPiece
	{
		private Texture2D _texture;

		private Vector2 _position;

		private Vector2 _velocity;

		private Vector2 _origin;

		private float _rotation;

		private float _rotationVelocity;

		public bool Dead
		{
			get
			{
				if (!(_position.Y > (float)(Main.maxTilesY * 16) - 480f) && !(_position.X < 480f))
				{
					return _position.X >= (float)(Main.maxTilesX * 16) - 480f;
				}
				return true;
			}
		}

		public MoonlordPiece(Texture2D pieceTexture, Vector2 textureOrigin, Vector2 centerPos, Vector2 velocity, float rot, float angularVelocity)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			_texture = pieceTexture;
			_origin = textureOrigin;
			_position = centerPos;
			_velocity = velocity;
			_rotation = rot;
			_rotationVelocity = angularVelocity;
		}

		public void Update()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			_velocity.Y += 0.3f;
			_rotation += _rotationVelocity;
			_rotationVelocity *= 0.99f;
			_position += _velocity;
		}

		public void Draw(SpriteBatch sp)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			Color light = GetLight();
			sp.Draw(_texture, _position - Main.screenPosition, (Rectangle?)null, light, _rotation, _origin, 1f, (SpriteEffects)0, 0f);
		}

		public bool InDrawRange(Rectangle playerScreen)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return ((Rectangle)(ref playerScreen)).Contains(_position.ToPoint());
		}

		public Color GetLight()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			Vector3 zero = Vector3.Zero;
			float num = 0f;
			int num2 = 5;
			Point point = _position.ToTileCoordinates();
			for (int i = point.X - num2; i <= point.X + num2; i++)
			{
				for (int j = point.Y - num2; j <= point.Y + num2; j++)
				{
					Vector3 val = zero;
					Color color = Lighting.GetColor(i, j);
					zero = val + ((Color)(ref color)).ToVector3();
					num += 1f;
				}
			}
			if (num == 0f)
			{
				return Color.White;
			}
			return new Color(zero / num);
		}
	}

	public class MoonlordExplosion
	{
		private Texture2D _texture;

		private Vector2 _position;

		private Vector2 _origin;

		private Rectangle _frame;

		private int _frameCounter;

		private int _frameSpeed;

		public bool Dead
		{
			get
			{
				if (!(_position.Y > (float)(Main.maxTilesY * 16) - 480f) && !(_position.X < 480f) && !(_position.X >= (float)(Main.maxTilesX * 16) - 480f))
				{
					return _frameCounter >= _frameSpeed * 7;
				}
				return true;
			}
		}

		public MoonlordExplosion(Texture2D pieceTexture, Vector2 centerPos, int frameSpeed)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			_texture = pieceTexture;
			_position = centerPos;
			_frameSpeed = frameSpeed;
			_frameCounter = 0;
			_frame = _texture.Frame(1, 7);
			_origin = _frame.Size() / 2f;
		}

		public void Update()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			_frameCounter++;
			_frame = _texture.Frame(1, 7, 0, _frameCounter / _frameSpeed);
		}

		public void Draw(SpriteBatch sp)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			Color light = GetLight();
			sp.Draw(_texture, _position - Main.screenPosition, (Rectangle?)_frame, light, 0f, _origin, 1f, (SpriteEffects)0, 0f);
		}

		public bool InDrawRange(Rectangle playerScreen)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return ((Rectangle)(ref playerScreen)).Contains(_position.ToPoint());
		}

		public Color GetLight()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return new Color(255, 255, 255, 127);
		}
	}

	private static List<MoonlordPiece> _pieces = new List<MoonlordPiece>();

	private static List<MoonlordExplosion> _explosions = new List<MoonlordExplosion>();

	private static List<Vector2> _lightSources = new List<Vector2>();

	private static float whitening;

	private static float requestedLight;

	public static void Update()
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _pieces.Count; i++)
		{
			MoonlordPiece moonlordPiece = _pieces[i];
			moonlordPiece.Update();
			if (moonlordPiece.Dead)
			{
				_pieces.Remove(moonlordPiece);
				i--;
			}
		}
		for (int j = 0; j < _explosions.Count; j++)
		{
			MoonlordExplosion moonlordExplosion = _explosions[j];
			moonlordExplosion.Update();
			if (moonlordExplosion.Dead)
			{
				_explosions.Remove(moonlordExplosion);
				j--;
			}
		}
		bool flag = false;
		for (int k = 0; k < _lightSources.Count; k++)
		{
			if (Main.player[Main.myPlayer].Distance(_lightSources[k]) < 2000f)
			{
				flag = true;
				break;
			}
		}
		_lightSources.Clear();
		if (!flag)
		{
			requestedLight = 0f;
		}
		if (requestedLight != whitening)
		{
			if (Math.Abs(requestedLight - whitening) < 0.02f)
			{
				whitening = requestedLight;
			}
			else
			{
				whitening += (float)Math.Sign(requestedLight - whitening) * 0.02f;
			}
		}
		requestedLight = 0f;
	}

	public static void DrawPieces(SpriteBatch spriteBatch)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Rectangle playerScreen = Utils.CenteredRectangle(Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f, new Vector2((float)(Main.screenWidth + 1000), (float)(Main.screenHeight + 1000)));
		for (int i = 0; i < _pieces.Count; i++)
		{
			if (_pieces[i].InDrawRange(playerScreen))
			{
				_pieces[i].Draw(spriteBatch);
			}
		}
	}

	public static void DrawExplosions(SpriteBatch spriteBatch)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Rectangle playerScreen = Utils.CenteredRectangle(Main.screenPosition + new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f, new Vector2((float)(Main.screenWidth + 1000), (float)(Main.screenHeight + 1000)));
		for (int i = 0; i < _explosions.Count; i++)
		{
			if (_explosions[i].InDrawRange(playerScreen))
			{
				_explosions[i].Draw(spriteBatch);
			}
		}
	}

	public static void DrawWhite(SpriteBatch spriteBatch)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (whitening != 0f)
		{
			Color color = Color.White * whitening;
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle(-2, -2, Main.screenWidth + 4, Main.screenHeight + 4), (Rectangle?)new Rectangle(0, 0, 1, 1), color);
		}
	}

	public static void ThrowPieces(Vector2 MoonlordCoreCenter, int DramaSeed)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		UnifiedRandom r = new UnifiedRandom(DramaSeed);
		Vector2 vector = Vector2.UnitY.RotatedBy(r.NextFloat() * ((float)Math.PI / 2f) - (float)Math.PI / 4f + (float)Math.PI);
		_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Spine").Value, new Vector2(64f, 150f), MoonlordCoreCenter + new Vector2(0f, 50f), vector * 6f, 0f, r.NextFloat() * 0.1f - 0.05f));
		vector = Vector2.UnitY.RotatedBy(r.NextFloat() * ((float)Math.PI / 2f) - (float)Math.PI / 4f + (float)Math.PI);
		_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Shoulder").Value, new Vector2(40f, 120f), MoonlordCoreCenter + new Vector2(50f, -120f), vector * 10f, 0f, r.NextFloat() * 0.1f - 0.05f));
		vector = Vector2.UnitY.RotatedBy(r.NextFloat() * ((float)Math.PI / 2f) - (float)Math.PI / 4f + (float)Math.PI);
		_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Torso").Value, new Vector2(192f, 252f), MoonlordCoreCenter, vector * 8f, 0f, r.NextFloat() * 0.1f - 0.05f));
		vector = Vector2.UnitY.RotatedBy(r.NextFloat() * ((float)Math.PI / 2f) - (float)Math.PI / 4f + (float)Math.PI);
		_pieces.Add(new MoonlordPiece(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Head").Value, new Vector2(138f, 185f), MoonlordCoreCenter - new Vector2(0f, 200f), vector * 12f, 0f, r.NextFloat() * 0.1f - 0.05f));
	}

	public static void AddExplosion(Vector2 spot)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		_explosions.Add(new MoonlordExplosion(Main.Assets.Request<Texture2D>("Images/Misc/MoonExplosion/Explosion").Value, spot, Main.rand.Next(2, 4)));
	}

	public static void RequestLight(float light, Vector2 spot)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		_lightSources.Add(spot);
		if (light > 1f)
		{
			light = 1f;
		}
		if (requestedLight < light)
		{
			requestedLight = light;
		}
	}
}
