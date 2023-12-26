using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Renderers;

public class FlameParticle : ABasicParticle
{
	public float FadeOutNormalizedTime = 1f;

	private float _timeTolive;

	private float _timeSinceSpawn;

	private int _indexOfPlayerWhoSpawnedThis;

	private int _packedShaderIndex;

	public override void FetchFromPool()
	{
		base.FetchFromPool();
		FadeOutNormalizedTime = 1f;
		_timeTolive = 0f;
		_timeSinceSpawn = 0f;
		_indexOfPlayerWhoSpawnedThis = 0;
		_packedShaderIndex = 0;
	}

	public override void SetBasicInfo(Asset<Texture2D> textureAsset, Rectangle? frame, Vector2 initialVelocity, Vector2 initialLocalPosition)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		base.SetBasicInfo(textureAsset, frame, initialVelocity, initialLocalPosition);
		_origin = new Vector2((float)(_frame.Width / 2), (float)(_frame.Height - 2));
	}

	public void SetTypeInfo(float timeToLive, int indexOfPlayerWhoSpawnedIt, int packedShaderIndex)
	{
		_timeTolive = timeToLive;
		_indexOfPlayerWhoSpawnedThis = indexOfPlayerWhoSpawnedIt;
		_packedShaderIndex = packedShaderIndex;
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
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		Color color = new Color(120, 120, 120, 60) * Utils.GetLerpValue(1f, FadeOutNormalizedTime, _timeSinceSpawn / _timeTolive, clamped: true);
		Vector2 vector = settings.AnchorPosition + LocalPosition;
		ulong seed = Main.TileFrameSeed ^ (((ulong)LocalPosition.X << 32) | (uint)LocalPosition.Y);
		Player player = Main.player[_indexOfPlayerWhoSpawnedThis];
		for (int i = 0; i < 4; i++)
		{
			Vector2 position = vector + new Vector2((float)Utils.RandomInt(ref seed, -2, 3), (float)Utils.RandomInt(ref seed, -2, 3)) * Scale;
			DrawData drawData = new DrawData(_texture.Value, position, _frame, color, Rotation, _origin, Scale, (SpriteEffects)0);
			drawData.shader = _packedShaderIndex;
			DrawData cdd = drawData;
			PlayerDrawHelper.SetShaderForData(player, 0, ref cdd);
			cdd.Draw(spritebatch);
		}
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}
}
