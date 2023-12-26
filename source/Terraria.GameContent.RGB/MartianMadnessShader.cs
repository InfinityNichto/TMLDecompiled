using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class MartianMadnessShader : ChromaShader
{
	private readonly Vector4 _metalColor;

	private readonly Vector4 _glassColor;

	private readonly Vector4 _beamColor;

	private readonly Vector4 _backgroundColor;

	public MartianMadnessShader(Color metalColor, Color glassColor, Color beamColor, Color backgroundColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		_metalColor = ((Color)(ref metalColor)).ToVector4();
		_glassColor = ((Color)(ref glassColor)).ToVector4();
		_beamColor = ((Color)(ref beamColor)).ToVector4();
		_backgroundColor = ((Color)(ref backgroundColor)).ToVector4();
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.Low })]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			float amount = (float)Math.Sin(time * 2f + canvasPositionOfIndex.X * 5f) * 0.5f + 0.5f;
			int num = (gridPositionOfIndex.X + gridPositionOfIndex.Y) % 2;
			if (num < 0)
			{
				num += 2;
			}
			Vector4 color = ((num == 1) ? Vector4.Lerp(_glassColor, _beamColor, amount) : _metalColor);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		if (device.Type != 0 && device.Type != RgbDeviceType.Virtual)
		{
			ProcessLowDetail(device, fragment, quality, time);
			return;
		}
		float num = time * 0.5f % ((float)Math.PI * 2f);
		if (num > (float)Math.PI)
		{
			num = (float)Math.PI * 2f - num;
		}
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(1.7f + (float)Math.Cos(num) * 2f, -0.5f + (float)Math.Sin(num) * 1.1f);
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Vector4 vector2 = _backgroundColor;
			float num2 = Math.Abs(vector.X - canvasPositionOfIndex.X);
			if (canvasPositionOfIndex.Y > vector.Y && num2 < 0.2f)
			{
				float num3 = 1f - MathHelper.Clamp((num2 - 0.2f + 0.2f) / 0.2f, 0f, 1f);
				float num4 = Math.Abs((num - (float)Math.PI / 2f) / ((float)Math.PI / 2f));
				num4 = Math.Max(0f, 1f - num4 * 3f);
				vector2 = Vector4.Lerp(vector2, _beamColor, num3 * num4);
			}
			Vector2 vector3 = vector - canvasPositionOfIndex;
			vector3.X /= 1f;
			vector3.Y /= 0.2f;
			float num5 = ((Vector2)(ref vector3)).Length();
			if (num5 < 1f)
			{
				float amount = 1f - MathHelper.Clamp((num5 - 1f + 0.2f) / 0.2f, 0f, 1f);
				vector2 = Vector4.Lerp(vector2, _metalColor, amount);
			}
			Vector2 vector4 = vector - canvasPositionOfIndex + new Vector2(0f, -0.1f);
			vector4.X /= 0.3f;
			vector4.Y /= 0.3f;
			if (vector4.Y < 0f)
			{
				vector4.Y *= 2f;
			}
			float num6 = ((Vector2)(ref vector4)).Length();
			if (num6 < 1f)
			{
				float amount2 = 1f - MathHelper.Clamp((num6 - 1f + 0.2f) / 0.2f, 0f, 1f);
				vector2 = Vector4.Lerp(vector2, _glassColor, amount2);
			}
			fragment.SetColor(i, vector2);
		}
	}
}
