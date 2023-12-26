using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class GemCaveShader : ChromaShader
{
	private readonly Vector4 _primaryColor;

	private readonly Vector4 _secondaryColor;

	private readonly Vector4[] _gemColors;

	public float CycleTime;

	public float ColorRarity;

	public float TimeRate;

	public GemCaveShader(Color primaryColor, Color secondaryColor, Vector4[] gemColors)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_primaryColor = ((Color)(ref primaryColor)).ToVector4();
		_secondaryColor = ((Color)(ref secondaryColor)).ToVector4();
		_gemColors = gemColors;
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		time *= TimeRate;
		float num = time % 1f;
		bool num2 = time % 2f > 1f;
		Vector4 vector = (num2 ? _secondaryColor : _primaryColor);
		Vector4 value = (num2 ? _primaryColor : _secondaryColor);
		num *= 1.2f;
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			float staticNoise = NoiseHelper.GetStaticNoise(canvasPositionOfIndex * 0.5f + new Vector2(0f, time * 0.5f));
			Vector4 value2 = vector;
			staticNoise += num;
			if (staticNoise > 0.999f)
			{
				float amount = MathHelper.Clamp((staticNoise - 0.999f) / 0.2f, 0f, 1f);
				value2 = Vector4.Lerp(value2, value, amount);
			}
			float dynamicNoise = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / CycleTime);
			dynamicNoise = Math.Max(0f, 1f - dynamicNoise * ColorRarity);
			value2 = Vector4.Lerp(value2, _gemColors[((gridPositionOfIndex.Y * 47 + gridPositionOfIndex.X) % _gemColors.Length + _gemColors.Length) % _gemColors.Length], dynamicNoise);
			fragment.SetColor(i, value2);
			fragment.SetColor(i, value2);
		}
	}
}
