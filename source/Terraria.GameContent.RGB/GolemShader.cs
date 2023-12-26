using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class GolemShader : ChromaShader
{
	private readonly Vector4 _glowColor;

	private readonly Vector4 _coreColor;

	private readonly Vector4 _backgroundColor;

	public GolemShader(Color glowColor, Color coreColor, Color backgroundColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		_glowColor = ((Color)(ref glowColor)).ToVector4();
		_coreColor = ((Color)(ref coreColor)).ToVector4();
		_backgroundColor = ((Color)(ref backgroundColor)).ToVector4();
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.Low })]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		Vector4 value = Vector4.Lerp(_backgroundColor, _coreColor, Math.Max(0f, (float)Math.Sin(time * 0.5f)));
		for (int i = 0; i < fragment.Count; i++)
		{
			float num = Math.Max(0f, (float)Math.Sin(fragment.GetCanvasPositionOfIndex(i).X * 2f + time + 101f));
			Vector4 color = Vector4.Lerp(value, _glowColor, num);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.5f + (float)Math.Sin(time * 3f) * 0.1f;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(1.6f, 0.5f);
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector4 vector2 = _backgroundColor;
			float num2 = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y) * 10f + time * 2f) % 10f - Math.Abs(canvasPositionOfIndex.X - vector.X);
			if (num2 > 0f)
			{
				float amount = Math.Max(0f, 1.2f - num2);
				if (num2 < 0.2f)
				{
					amount = num2 * 5f;
				}
				vector2 = Vector4.Lerp(vector2, _glowColor, amount);
			}
			Vector2 val = canvasPositionOfIndex - vector;
			float num3 = ((Vector2)(ref val)).Length();
			if (num3 < num)
			{
				float amount2 = 1f - MathHelper.Clamp((num3 - num + 0.1f) / 0.1f, 0f, 1f);
				vector2 = Vector4.Lerp(vector2, _coreColor, amount2);
			}
			fragment.SetColor(i, vector2);
		}
	}
}
