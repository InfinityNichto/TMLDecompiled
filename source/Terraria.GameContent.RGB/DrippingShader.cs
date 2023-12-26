using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class DrippingShader : ChromaShader
{
	private readonly Vector4 _baseColor;

	private readonly Vector4 _liquidColor;

	private readonly float _viscosity;

	public DrippingShader(Color baseColor, Color liquidColor, float viscosity = 1f)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_baseColor = ((Color)(ref baseColor)).ToVector4();
		_liquidColor = ((Color)(ref liquidColor)).ToVector4();
		_viscosity = viscosity;
	}

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
			Vector4 color = Vector4.Lerp(_baseColor, _liquidColor, num);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * new Vector2(0.7f * _viscosity, 0.075f) + new Vector2(0f, time * -0.1f * _viscosity));
			staticNoise = Math.Max(0f, 1f - (canvasPositionOfIndex.Y * 4.5f + 0.5f) * staticNoise);
			Vector4 baseColor = _baseColor;
			baseColor = Vector4.Lerp(baseColor, _liquidColor, staticNoise);
			fragment.SetColor(i, baseColor);
		}
	}
}
