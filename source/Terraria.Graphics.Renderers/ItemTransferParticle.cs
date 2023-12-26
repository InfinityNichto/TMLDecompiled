using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.Graphics.Renderers;

public class ItemTransferParticle : IPooledParticle, IParticle
{
	public Vector2 StartPosition;

	public Vector2 EndPosition;

	public Vector2 BezierHelper1;

	public Vector2 BezierHelper2;

	private Item _itemInstance;

	private int _lifeTimeCounted;

	private int _lifeTimeTotal;

	public bool ShouldBeRemovedFromRenderer { get; private set; }

	public bool IsRestingInPool { get; private set; }

	public ItemTransferParticle()
	{
		_itemInstance = new Item();
	}

	public void Update(ref ParticleRendererSettings settings)
	{
		if (++_lifeTimeCounted >= _lifeTimeTotal)
		{
			ShouldBeRemovedFromRenderer = true;
		}
	}

	public void Prepare(int itemType, int lifeTimeTotal, Vector2 playerPosition, Vector2 chestPosition)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		_itemInstance.SetDefaults(itemType);
		_lifeTimeTotal = lifeTimeTotal;
		StartPosition = playerPosition;
		EndPosition = chestPosition;
		Vector2 vector = (EndPosition - StartPosition).SafeNormalize(Vector2.UnitY).RotatedBy(1.5707963705062866);
		bool num3 = vector.Y < 0f;
		bool flag = vector.Y == 0f;
		if (!num3 || (flag && Main.rand.Next(2) == 0))
		{
			vector *= -1f;
		}
		((Vector2)(ref vector))._002Ector(0f, -1f);
		float num2 = Vector2.Distance(EndPosition, StartPosition);
		BezierHelper1 = vector * num2 + Main.rand.NextVector2Circular(32f, 32f);
		BezierHelper2 = -vector * num2 + Main.rand.NextVector2Circular(32f, 32f);
	}

	public void Draw(ref ParticleRendererSettings settings, SpriteBatch spritebatch)
	{
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		float fromValue = (float)_lifeTimeCounted / (float)_lifeTimeTotal;
		float toMin = Utils.Remap(fromValue, 0.1f, 0.5f, 0f, 0.85f);
		toMin = Utils.Remap(fromValue, 0.5f, 0.9f, toMin, 1f);
		Vector2 result = default(Vector2);
		Vector2.Hermite(ref StartPosition, ref BezierHelper1, ref EndPosition, ref BezierHelper2, toMin, ref result);
		float toMin2 = Utils.Remap(fromValue, 0f, 0.1f, 0f, 1f);
		toMin2 = Utils.Remap(fromValue, 0.85f, 0.95f, toMin2, 0f);
		float num = Utils.Remap(fromValue, 0f, 0.25f, 0f, 1f) * Utils.Remap(fromValue, 0.85f, 0.95f, 1f, 0f);
		ItemSlot.DrawItemIcon(_itemInstance, 31, Main.spriteBatch, settings.AnchorPosition + result, _itemInstance.scale * toMin2, 100f, Color.White * num);
	}

	public void RestInPool()
	{
		IsRestingInPool = true;
	}

	public virtual void FetchFromPool()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		_lifeTimeCounted = 0;
		_lifeTimeTotal = 0;
		IsRestingInPool = false;
		ShouldBeRemovedFromRenderer = false;
		StartPosition = (EndPosition = (BezierHelper1 = (BezierHelper2 = Vector2.Zero)));
	}
}
