using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class TwinsShader : ChromaShader
{
	private readonly Vector4 _eyeColor;

	private readonly Vector4 _veinColor;

	private readonly Vector4 _laserColor;

	private readonly Vector4 _mouthColor;

	private readonly Vector4 _flameColor;

	private readonly Vector4 _backgroundColor;

	private static readonly Vector4[] _irisColors;

	public TwinsShader(Color eyeColor, Color veinColor, Color laserColor, Color mouthColor, Color flameColor, Color backgroundColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		_eyeColor = ((Color)(ref eyeColor)).ToVector4();
		_veinColor = ((Color)(ref veinColor)).ToVector4();
		_laserColor = ((Color)(ref laserColor)).ToVector4();
		_mouthColor = ((Color)(ref mouthColor)).ToVector4();
		_flameColor = ((Color)(ref flameColor)).ToVector4();
		_backgroundColor = ((Color)(ref backgroundColor)).ToVector4();
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.Low })]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector4 value = Vector4.Lerp(_veinColor, _eyeColor, (float)Math.Sin(time + canvasPositionOfIndex.X * 4f) * 0.5f + 0.5f);
			float dynamicNoise = NoiseHelper.GetDynamicNoise(gridPositionOfIndex.X, gridPositionOfIndex.Y, time / 25f);
			dynamicNoise = Math.Max(0f, 1f - dynamicNoise * 5f);
			value = Vector4.Lerp(value, _irisColors[((gridPositionOfIndex.Y * 47 + gridPositionOfIndex.X) % _irisColors.Length + _irisColors.Length) % _irisColors.Length], dynamicNoise);
			fragment.SetColor(i, value);
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
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		if (device.Type != 0 && device.Type != RgbDeviceType.Virtual)
		{
			ProcessLowDetail(device, fragment, quality, time);
			return;
		}
		bool flag = true;
		float num = time * 0.1f % 2f;
		if (num > 1f)
		{
			num = 2f - num;
			flag = false;
		}
		Vector2 vector = new Vector2(num * 7f - 3.5f, 0f) + fragment.CanvasCenter;
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector4 vector2 = _backgroundColor;
			Vector2 vector3 = canvasPositionOfIndex - vector;
			float num2 = ((Vector2)(ref vector3)).Length();
			if (num2 < 0.5f)
			{
				float amount = 1f - MathHelper.Clamp((num2 - 0.5f + 0.2f) / 0.2f, 0f, 1f);
				float num3 = MathHelper.Clamp((vector3.X + 0.5f - 0.2f) / 0.6f, 0f, 1f);
				if (flag)
				{
					num3 = 1f - num3;
				}
				Vector4 value = Vector4.Lerp(_eyeColor, _veinColor, num3);
				float value2 = (float)Math.Atan2(vector3.Y, vector3.X);
				if (!flag && (float)Math.PI - Math.Abs(value2) < 0.6f)
				{
					value = _mouthColor;
				}
				vector2 = Vector4.Lerp(vector2, value, amount);
			}
			if (flag && gridPositionOfIndex.Y == 3 && canvasPositionOfIndex.X > vector.X)
			{
				float value3 = 1f - Math.Abs(canvasPositionOfIndex.X - vector.X * 2f - 0.5f) / 0.5f;
				vector2 = Vector4.Lerp(vector2, _laserColor, MathHelper.Clamp(value3, 0f, 1f));
			}
			else if (!flag)
			{
				Vector2 vector4 = canvasPositionOfIndex - (vector - new Vector2(1.2f, 0f));
				vector4.Y *= 3.5f;
				float num4 = ((Vector2)(ref vector4)).Length();
				if (num4 < 0.7f)
				{
					float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex, time);
					dynamicNoise = dynamicNoise * dynamicNoise * dynamicNoise;
					dynamicNoise *= 1f - MathHelper.Clamp((num4 - 0.7f + 0.3f) / 0.3f, 0f, 1f);
					vector2 = Vector4.Lerp(vector2, _flameColor, dynamicNoise);
				}
			}
			fragment.SetColor(i, vector2);
		}
	}

	static TwinsShader()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Vector4[] array = new Vector4[2];
		Color val = Color.Green;
		array[0] = ((Color)(ref val)).ToVector4();
		val = Color.Blue;
		array[1] = ((Color)(ref val)).ToVector4();
		_irisColors = (Vector4[])(object)array;
	}
}
