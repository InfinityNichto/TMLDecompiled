using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers;

public class PrettySparkleParticle : ABasicParticle
{
	public float FadeInNormalizedTime = 0.05f;

	public float FadeOutNormalizedTime = 0.9f;

	public float TimeToLive = 60f;

	public Color ColorTint;

	public float Opacity;

	public float AdditiveAmount = 1f;

	public float FadeInEnd = 20f;

	public float FadeOutStart = 30f;

	public float FadeOutEnd = 45f;

	public bool DrawHorizontalAxis = true;

	public bool DrawVerticalAxis = true;

	private float _timeSinceSpawn;

	public override void FetchFromPool()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		base.FetchFromPool();
		ColorTint = Color.Transparent;
		_timeSinceSpawn = 0f;
		Opacity = 0f;
		FadeInNormalizedTime = 0.05f;
		FadeOutNormalizedTime = 0.9f;
		TimeToLive = 60f;
		AdditiveAmount = 1f;
		FadeInEnd = 20f;
		FadeOutStart = 30f;
		FadeOutEnd = 45f;
		DrawVerticalAxis = (DrawHorizontalAxis = true);
	}

	public override void Update(ref ParticleRendererSettings settings)
	{
		base.Update(ref settings);
		_timeSinceSpawn += 1f;
		Opacity = Utils.GetLerpValue(0f, FadeInNormalizedTime, _timeSinceSpawn / TimeToLive, clamped: true) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, _timeSinceSpawn / TimeToLive, clamped: true);
		if (_timeSinceSpawn >= TimeToLive)
		{
			base.ShouldBeRemovedFromRenderer = true;
		}
	}

	public override void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		Color color = Color.White * Opacity * 0.9f;
		((Color)(ref color)).A = (byte)(((Color)(ref color)).A / 2);
		Texture2D value = TextureAssets.Extra[98].Value;
		Color color2 = ColorTint * Opacity * 0.5f;
		((Color)(ref color2)).A = (byte)((float)(int)((Color)(ref color2)).A * (1f - AdditiveAmount));
		Vector2 origin = value.Size() / 2f;
		Color color3 = color * 0.5f;
		float t = _timeSinceSpawn / TimeToLive * 60f;
		float num = Utils.GetLerpValue(0f, FadeInEnd, t, clamped: true) * Utils.GetLerpValue(FadeOutEnd, FadeOutStart, t, clamped: true);
		Vector2 vector = new Vector2(0.3f, 2f) * num * Scale;
		Vector2 vector2 = new Vector2(0.3f, 1f) * num * Scale;
		color2 *= num;
		color3 *= num;
		Vector2 position = settings.AnchorPosition + LocalPosition;
		SpriteEffects effects = (SpriteEffects)0;
		if (DrawHorizontalAxis)
		{
			spritebatch.Draw(value, position, (Rectangle?)null, color2, (float)Math.PI / 2f + Rotation, origin, vector, effects, 0f);
		}
		if (DrawVerticalAxis)
		{
			spritebatch.Draw(value, position, (Rectangle?)null, color2, 0f + Rotation, origin, vector2, effects, 0f);
		}
		if (DrawHorizontalAxis)
		{
			spritebatch.Draw(value, position, (Rectangle?)null, color3, (float)Math.PI / 2f + Rotation, origin, vector * 0.6f, effects, 0f);
		}
		if (DrawVerticalAxis)
		{
			spritebatch.Draw(value, position, (Rectangle?)null, color3, 0f + Rotation, origin, vector2 * 0.6f, effects, 0f);
		}
	}
}
