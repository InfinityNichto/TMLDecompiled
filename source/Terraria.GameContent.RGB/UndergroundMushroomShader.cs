using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class UndergroundMushroomShader : ChromaShader
{
	private readonly Vector4 _baseColor;

	private readonly Vector4 _edgeGlowColor;

	private readonly Vector4 _sporeColor;

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.Low })]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			float num = (float)Math.Sin(time * 0.5f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f;
			Vector4 color = Vector4.Lerp(_edgeGlowColor, _sporeColor, num);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Vector4 value = _baseColor;
			float num = ((NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 10f + time * 0.2f) % 10f - (1f - canvasPositionOfIndex.Y)) * 2f;
			if (num > 0f)
			{
				float amount = Math.Max(0f, 1.5f - num);
				if (num < 0.5f)
				{
					amount = num * 2f;
				}
				value = Vector4.Lerp(value, _sporeColor, amount);
			}
			float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.3f + new Vector2(0f, time * 0.1f));
			staticNoise = Math.Max(0f, 1f - staticNoise * (1f + (1f - canvasPositionOfIndex.Y) * 4f));
			staticNoise *= Math.Max(0f, (canvasPositionOfIndex.Y - 0.3f) / 0.7f);
			value = Vector4.Lerp(value, _edgeGlowColor, staticNoise);
			fragment.SetColor(i, value);
		}
	}

	public UndergroundMushroomShader()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Color val = new Color(10, 10, 10);
		_baseColor = ((Color)(ref val)).ToVector4();
		val = new Color(0, 0, 255);
		_edgeGlowColor = ((Color)(ref val)).ToVector4();
		val = new Color(255, 230, 150);
		_sporeColor = ((Color)(ref val)).ToVector4();
		base._002Ector();
	}
}
