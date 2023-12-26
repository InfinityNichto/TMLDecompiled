using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class SkullShader : ChromaShader
{
	private readonly Vector4 _skullColor;

	private readonly Vector4 _bloodDark;

	private readonly Vector4 _bloodLight;

	private readonly Vector4 _backgroundColor;

	public SkullShader(Color skullColor, Color bloodDark, Color bloodLight)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Color black = Color.Black;
		_backgroundColor = ((Color)(ref black)).ToVector4();
		base._002Ector();
		_skullColor = ((Color)(ref skullColor)).ToVector4();
		_bloodDark = ((Color)(ref bloodDark)).ToVector4();
		_bloodLight = ((Color)(ref bloodLight)).ToVector4();
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.Low })]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			float num = (float)Math.Sin(time * 2f + fragment.GetCanvasPositionOfIndex(i).X * 2f) * 0.5f + 0.5f;
			Vector4 color = Vector4.Lerp(_skullColor, _bloodLight, num);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector4 value = _backgroundColor;
			float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 10f + time * 0.75f) % 10f + canvasPositionOfIndex.Y - 1f;
			if (num > 0f)
			{
				float amount = Math.Max(0f, 1.2f - num);
				if (num < 0.2f)
				{
					amount = num * 5f;
				}
				value = Vector4.Lerp(value, _skullColor, amount);
			}
			float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.5f + new Vector2(12.5f, time * 0.2f));
			staticNoise = Math.Max(0f, 1f - staticNoise * staticNoise * 4f * staticNoise * (1f - canvasPositionOfIndex.Y * canvasPositionOfIndex.Y)) * canvasPositionOfIndex.Y * canvasPositionOfIndex.Y;
			staticNoise = MathHelper.Clamp(staticNoise, 0f, 1f);
			Vector4 value2 = Vector4.Lerp(_bloodDark, _bloodLight, staticNoise);
			value = Vector4.Lerp(value, value2, staticNoise);
			fragment.SetColor(i, value);
		}
	}
}
