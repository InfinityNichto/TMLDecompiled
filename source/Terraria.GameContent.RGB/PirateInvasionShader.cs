using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class PirateInvasionShader : ChromaShader
{
	private readonly Vector4 _cannonBallColor;

	private readonly Vector4 _splashColor;

	private readonly Vector4 _waterColor;

	private readonly Vector4 _backgroundColor;

	public PirateInvasionShader(Color cannonBallColor, Color splashColor, Color waterColor, Color backgroundColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		_cannonBallColor = ((Color)(ref cannonBallColor)).ToVector4();
		_splashColor = ((Color)(ref splashColor)).ToVector4();
		_waterColor = ((Color)(ref waterColor)).ToVector4();
		_backgroundColor = ((Color)(ref backgroundColor)).ToVector4();
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
			Vector4 color = Vector4.Lerp(_waterColor, _cannonBallColor, num);
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
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			gridPositionOfIndex.X /= 2;
			float num4 = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 40f + time * 1f) % 40f;
			float amount = 0f;
			float num2 = num4 - canvasPositionOfIndex.Y / 1.2f;
			if (num4 > 1f)
			{
				float num3 = 1f - canvasPositionOfIndex.Y / 1.2f;
				amount = (1f - Math.Min(1f, num2 - num3)) * (1f - Math.Min(1f, num3 / 1f));
			}
			Vector4 vector = _backgroundColor;
			if (num2 > 0f)
			{
				float amount2 = Math.Max(0f, 1.2f - num2 * 4f);
				if (num2 < 0.1f)
				{
					amount2 = num2 / 0.1f;
				}
				vector = Vector4.Lerp(vector, _cannonBallColor, amount2);
				vector = Vector4.Lerp(vector, _splashColor, amount);
			}
			if (canvasPositionOfIndex.Y > 0.8f)
			{
				vector = _waterColor;
			}
			fragment.SetColor(i, vector);
		}
	}
}
