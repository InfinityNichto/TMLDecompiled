using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class PillarShader : ChromaShader
{
	private readonly Vector4 _primaryColor;

	private readonly Vector4 _secondaryColor;

	public PillarShader(Color primaryColor, Color secondaryColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_primaryColor = ((Color)(ref primaryColor)).ToVector4();
		_secondaryColor = ((Color)(ref secondaryColor)).ToVector4();
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
			float num = (float)Math.Sin(time * 2.5f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f;
			Vector4 color = Vector4.Lerp(_primaryColor, _secondaryColor, num);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(1.5f, 0.5f);
		time *= 4f;
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 vector2 = fragment.GetCanvasPositionOfIndex(i) - vector;
			float num = ((Vector2)(ref vector2)).Length() * 2f;
			float num2 = (float)Math.Atan2(vector2.Y, vector2.X);
			float amount = (float)Math.Sin(num * 4f - time - num2) * 0.5f + 0.5f;
			Vector4 color = Vector4.Lerp(_primaryColor, _secondaryColor, amount);
			if (num < 1f)
			{
				float num3 = num / 1f;
				num3 *= num3 * num3;
				float amount2 = (float)Math.Sin(4f - time - num2) * 0.5f + 0.5f;
				color = Vector4.Lerp(_primaryColor, _secondaryColor, amount2) * num3;
			}
			color.W = 1f;
			fragment.SetColor(i, color);
		}
	}
}
