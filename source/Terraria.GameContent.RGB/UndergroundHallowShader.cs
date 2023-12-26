using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class UndergroundHallowShader : ChromaShader
{
	private readonly Vector4 _baseColor;

	private readonly Vector4 _pinkCrystalColor;

	private readonly Vector4 _blueCrystalColor;

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
			float num = (float)Math.Sin(time * 2f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f;
			Vector4 color = Vector4.Lerp(_pinkCrystalColor, _blueCrystalColor, num);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Vector4 baseColor = _baseColor;
			float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.4f, time * 0.05f);
			dynamicNoise = Math.Max(0f, 1f - 2.5f * dynamicNoise);
			float dynamicNoise2 = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * 0.4f + new Vector2(0.05f, 0f), time * 0.05f);
			dynamicNoise2 = Math.Max(0f, 1f - 2.5f * dynamicNoise2);
			baseColor = ((!(dynamicNoise > dynamicNoise2)) ? Vector4.Lerp(baseColor, _blueCrystalColor, dynamicNoise2) : Vector4.Lerp(baseColor, _pinkCrystalColor, dynamicNoise));
			fragment.SetColor(i, baseColor);
		}
	}

	public UndergroundHallowShader()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		Color val = new Color(0.05f, 0.05f, 0.05f);
		_baseColor = ((Color)(ref val)).ToVector4();
		val = Color.HotPink;
		_pinkCrystalColor = ((Color)(ref val)).ToVector4();
		val = Color.DeepSkyBlue;
		_blueCrystalColor = ((Color)(ref val)).ToVector4();
		base._002Ector();
	}
}
