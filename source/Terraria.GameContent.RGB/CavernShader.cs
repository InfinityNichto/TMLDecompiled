using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class CavernShader : ChromaShader
{
	private readonly Vector4 _backColor;

	private readonly Vector4 _frontColor;

	private readonly float _speed;

	public CavernShader(Color backColor, Color frontColor, float speed)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_backColor = ((Color)(ref backColor)).ToVector4();
		_frontColor = ((Color)(ref frontColor)).ToVector4();
		_speed = speed;
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.Low })]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Vector4 color = Vector4.Lerp(_backColor, _frontColor, (float)Math.Sin(time * _speed + canvasPositionOfIndex.X) * 0.5f + 0.5f);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		time *= _speed * 0.5f;
		float num = time % 1f;
		bool num2 = time % 2f > 1f;
		Vector4 vector = (num2 ? _frontColor : _backColor);
		Vector4 value = (num2 ? _backColor : _frontColor);
		num *= 1.2f;
		for (int i = 0; i < fragment.Count; i++)
		{
			float staticNoise = NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(i) * 0.5f + new Vector2(0f, time * 0.5f));
			Vector4 vector2 = vector;
			staticNoise += num;
			if (staticNoise > 0.999f)
			{
				float amount = MathHelper.Clamp((staticNoise - 0.999f) / 0.2f, 0f, 1f);
				vector2 = Vector4.Lerp(vector2, value, amount);
			}
			fragment.SetColor(i, vector2);
		}
	}
}
