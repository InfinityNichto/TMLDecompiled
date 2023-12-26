using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class EyeOfCthulhuShader : ChromaShader
{
	private readonly Vector4 _eyeColor;

	private readonly Vector4 _veinColor;

	private readonly Vector4 _backgroundColor;

	public EyeOfCthulhuShader(Color eyeColor, Color veinColor, Color backgroundColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		_eyeColor = ((Color)(ref eyeColor)).ToVector4();
		_veinColor = ((Color)(ref veinColor)).ToVector4();
		_backgroundColor = ((Color)(ref backgroundColor)).ToVector4();
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.Low })]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			float num = (float)Math.Sin(time + fragment.GetCanvasPositionOfIndex(i).X * 4f) * 0.5f + 0.5f;
			Vector4 color = Vector4.Lerp(_veinColor, _eyeColor, num);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		if (device.Type != 0 && device.Type != RgbDeviceType.Virtual)
		{
			ProcessLowDetail(device, fragment, quality, time);
			return;
		}
		float num = time * 0.2f % 2f;
		int num2 = 1;
		if (num > 1f)
		{
			num = 2f - num;
			num2 = -1;
		}
		Vector2 vector = new Vector2(num * 7f - 3.5f, 0f) + fragment.CanvasCenter;
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Vector4 vector2 = _backgroundColor;
			Vector2 vector3 = canvasPositionOfIndex - vector;
			float num3 = ((Vector2)(ref vector3)).Length();
			if (num3 < 0.5f)
			{
				float amount = 1f - MathHelper.Clamp((num3 - 0.5f + 0.2f) / 0.2f, 0f, 1f);
				float num4 = MathHelper.Clamp((vector3.X + 0.5f - 0.2f) / 0.6f, 0f, 1f);
				if (num2 == 1)
				{
					num4 = 1f - num4;
				}
				Vector4 value = Vector4.Lerp(_eyeColor, _veinColor, num4);
				vector2 = Vector4.Lerp(vector2, value, amount);
			}
			fragment.SetColor(i, vector2);
		}
	}
}
