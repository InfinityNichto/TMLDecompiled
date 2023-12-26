using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class PlanteraShader : ChromaShader
{
	private readonly Vector4 _bulbColor;

	private readonly Vector4 _vineColor;

	private readonly Vector4 _backgroundColor;

	public PlanteraShader(Color bulbColor, Color vineColor, Color backgroundColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		_bulbColor = ((Color)(ref bulbColor)).ToVector4();
		_vineColor = ((Color)(ref vineColor)).ToVector4();
		_backgroundColor = ((Color)(ref backgroundColor)).ToVector4();
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
			float num = (float)Math.Sin(time * 2f + fragment.GetCanvasPositionOfIndex(i).X * 10f) * 0.5f + 0.5f;
			Vector4 color = Vector4.Lerp(_bulbColor, _vineColor, num);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			canvasPositionOfIndex.X -= 1.8f;
			if (canvasPositionOfIndex.X < 0f)
			{
				canvasPositionOfIndex.X *= -1f;
				gridPositionOfIndex.Y += 101;
			}
			float staticNoise = NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y);
			staticNoise = (staticNoise * 5f + time * 0.4f) % 5f;
			float num = 1f;
			if (staticNoise > 1f)
			{
				num = 1f - MathHelper.Clamp((staticNoise - 0.4f - 1f) / 0.4f, 0f, 1f);
				staticNoise = 1f;
			}
			float num2 = staticNoise - canvasPositionOfIndex.X / 5f;
			Vector4 color = _backgroundColor;
			if (num2 > 0f)
			{
				float num3 = 1f;
				if (num2 < 0.2f)
				{
					num3 = num2 / 0.2f;
				}
				color = (((gridPositionOfIndex.X + 7 * gridPositionOfIndex.Y) % 5 != 0) ? Vector4.Lerp(_backgroundColor, _vineColor, num3 * num) : Vector4.Lerp(_backgroundColor, _bulbColor, num3 * num));
			}
			fragment.SetColor(i, color);
		}
	}
}
