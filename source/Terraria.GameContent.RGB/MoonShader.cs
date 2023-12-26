using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class MoonShader : ChromaShader
{
	private readonly Vector4 _moonCoreColor;

	private readonly Vector4 _moonRingColor;

	private readonly Vector4 _skyColor;

	private readonly Vector4 _cloudColor;

	private float _progress;

	public MoonShader(Color skyColor, Color moonRingColor, Color moonCoreColor)
		: this(skyColor, moonRingColor, moonCoreColor, Color.White)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0002: Unknown result type (might be due to invalid IL or missing references)
	//IL_0003: Unknown result type (might be due to invalid IL or missing references)
	//IL_0004: Unknown result type (might be due to invalid IL or missing references)


	public MoonShader(Color skyColor, Color moonColor)
		: this(skyColor, moonColor, moonColor)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0002: Unknown result type (might be due to invalid IL or missing references)
	//IL_0003: Unknown result type (might be due to invalid IL or missing references)


	public MoonShader(Color skyColor, Color moonRingColor, Color moonCoreColor, Color cloudColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		_skyColor = ((Color)(ref skyColor)).ToVector4();
		_moonRingColor = ((Color)(ref moonRingColor)).ToVector4();
		_moonCoreColor = ((Color)(ref moonCoreColor)).ToVector4();
		_cloudColor = ((Color)(ref cloudColor)).ToVector4();
	}

	public override void Update(float elapsedTime)
	{
		if (Main.dayTime)
		{
			_progress = (float)(Main.time / 54000.0);
		}
		else
		{
			_progress = (float)(Main.time / 32400.0);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.Low })]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			float dynamicNoise = NoiseHelper.GetDynamicNoise(fragment.GetCanvasPositionOfIndex(i) * new Vector2(0.1f, 0.5f) + new Vector2(time * 0.02f, 0f), time / 40f);
			dynamicNoise = (float)Math.Sqrt(Math.Max(0f, 1f - 2f * dynamicNoise));
			Vector4 color = Vector4.Lerp(_skyColor, _cloudColor, dynamicNoise * 0.1f);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		if (device.Type != 0 && device.Type != RgbDeviceType.Virtual)
		{
			ProcessLowDetail(device, fragment, quality, time);
			return;
		}
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(2f, 0.5f);
		Vector2 vector2 = default(Vector2);
		((Vector2)(ref vector2))._002Ector(2.5f, 1f);
		float num = _progress * (float)Math.PI + (float)Math.PI;
		Vector2 vector3 = new Vector2((float)Math.Cos(num), (float)Math.Sin(num)) * vector2 + vector;
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			float dynamicNoise = NoiseHelper.GetDynamicNoise(canvasPositionOfIndex * new Vector2(0.1f, 0.5f) + new Vector2(time * 0.02f, 0f), time / 40f);
			dynamicNoise = (float)Math.Sqrt(Math.Max(0f, 1f - 2f * dynamicNoise));
			Vector2 val = canvasPositionOfIndex - vector3;
			float num2 = ((Vector2)(ref val)).Length();
			Vector4 vector4 = Vector4.Lerp(_skyColor, _cloudColor, dynamicNoise * 0.15f);
			if (num2 < 0.8f)
			{
				vector4 = Vector4.Lerp(_moonRingColor, _moonCoreColor, Math.Min(0.1f, 0.8f - num2) / 0.1f);
			}
			else if (num2 < 1f)
			{
				vector4 = Vector4.Lerp(vector4, _moonRingColor, Math.Min(0.2f, 1f - num2) / 0.2f);
			}
			fragment.SetColor(i, vector4);
		}
	}
}
