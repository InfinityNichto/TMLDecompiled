using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers;

public class LittleFlyingCritterParticle : IPooledParticle, IParticle
{
	private int _lifeTimeCounted;

	private int _lifeTimeTotal;

	private Vector2 _spawnPosition;

	private Vector2 _localPosition;

	private Vector2 _velocity;

	private float _neverGoBelowThis;

	public bool IsRestingInPool { get; private set; }

	public bool ShouldBeRemovedFromRenderer { get; private set; }

	public void Prepare(Vector2 position, int duration)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		_spawnPosition = position;
		_localPosition = position + Main.rand.NextVector2Circular(4f, 8f);
		_neverGoBelowThis = position.Y + 8f;
		RandomizeVelocity();
		_lifeTimeCounted = 0;
		_lifeTimeTotal = 300 + Main.rand.Next(6) * 60;
	}

	private void RandomizeVelocity()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		_velocity = Main.rand.NextVector2Circular(1f, 1f);
	}

	public void RestInPool()
	{
		IsRestingInPool = true;
	}

	public virtual void FetchFromPool()
	{
		IsRestingInPool = false;
		ShouldBeRemovedFromRenderer = false;
	}

	public void Update(ref ParticleRendererSettings settings)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		if (++_lifeTimeCounted >= _lifeTimeTotal)
		{
			ShouldBeRemovedFromRenderer = true;
		}
		_velocity += new Vector2((float)Math.Sign(_spawnPosition.X - _localPosition.X) * 0.02f, (float)Math.Sign(_spawnPosition.Y - _localPosition.Y) * 0.02f);
		if (_lifeTimeCounted % 30 == 0 && Main.rand.Next(2) == 0)
		{
			RandomizeVelocity();
			if (Main.rand.Next(2) == 0)
			{
				_velocity /= 2f;
			}
		}
		_localPosition += _velocity;
		if (_localPosition.Y > _neverGoBelowThis)
		{
			_localPosition.Y = _neverGoBelowThis;
			if (_velocity.Y > 0f)
			{
				_velocity.Y *= -1f;
			}
		}
	}

	public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = settings.AnchorPosition + _localPosition;
		if (vector.X < -10f || vector.X > (float)(Main.screenWidth + 10) || vector.Y < -10f || vector.Y > (float)(Main.screenHeight + 10))
		{
			ShouldBeRemovedFromRenderer = true;
			return;
		}
		Texture2D value = TextureAssets.Extra[262].Value;
		int frameY = _lifeTimeCounted % 6 / 3;
		Rectangle value2 = value.Frame(1, 2, 0, frameY);
		Vector2 origin = default(Vector2);
		((Vector2)(ref origin))._002Ector((float)((!(_velocity.X > 0f)) ? 1 : 3), 3f);
		float num = Utils.Remap(_lifeTimeCounted, 0f, 90f, 0f, 1f) * Utils.Remap(_lifeTimeCounted, _lifeTimeTotal - 90, _lifeTimeTotal, 1f, 0f);
		spritebatch.Draw(value, settings.AnchorPosition + _localPosition, (Rectangle?)value2, Lighting.GetColor(_localPosition.ToTileCoordinates()) * num, 0f, origin, 1f, (SpriteEffects)(_velocity.X > 0f), 0f);
	}
}
