using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class DungeonShader : ChromaShader
{
	private readonly Vector4 _backgroundColor;

	private readonly Vector4 _spiritTrailColor;

	private readonly Vector4 _spiritColor;

	[RgbProcessor(new EffectDetailLevel[]
	{
		EffectDetailLevel.Low,
		EffectDetailLevel.High
	})]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			float num = ((NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y) * 10f + time) % 10f - (canvasPositionOfIndex.X + 2f)) * 0.5f;
			Vector4 vector = _backgroundColor;
			if (num > 0f)
			{
				float num2 = Math.Max(0f, 1.2f - num);
				float amount = MathHelper.Clamp(num2 * num2 * num2, 0f, 1f);
				if (num < 0.2f)
				{
					num2 = num / 0.2f;
				}
				Vector4 value = Vector4.Lerp(_spiritTrailColor, _spiritColor, amount);
				vector = Vector4.Lerp(vector, value, num2);
			}
			fragment.SetColor(i, vector);
		}
	}

	public DungeonShader()
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		Color val = new Color(5, 5, 5);
		_backgroundColor = ((Color)(ref val)).ToVector4();
		val = new Color(6, 51, 222);
		_spiritTrailColor = ((Color)(ref val)).ToVector4();
		val = Color.White;
		_spiritColor = ((Color)(ref val)).ToVector4();
		base._002Ector();
	}
}
