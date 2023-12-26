using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Renderers;

public class FadingParticle : ABasicParticle
{
	public float FadeInNormalizedTime;

	public float FadeOutNormalizedTime = 1f;

	public Color ColorTint = Color.White;

	private float _timeTolive;

	private float _timeSinceSpawn;

	public override void FetchFromPool()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		base.FetchFromPool();
		FadeInNormalizedTime = 0f;
		FadeOutNormalizedTime = 1f;
		ColorTint = Color.White;
		_timeTolive = 0f;
		_timeSinceSpawn = 0f;
	}

	public void SetTypeInfo(float timeToLive)
	{
		_timeTolive = timeToLive;
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		base.Update(ref settings);
		_timeSinceSpawn += 1f;
		if (_timeSinceSpawn >= _timeTolive)
		{
			base.ShouldBeRemovedFromRenderer = true;
		}
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		Color color = ColorTint * Utils.GetLerpValue(0f, FadeInNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true);
		spritebatch.Draw(_texture.Value, settings.AnchorPosition + LocalPosition, (Rectangle?)_frame, color, Rotation, _origin, Scale, (SpriteEffects)0, 0f);
	}
}
