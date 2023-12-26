using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class WormShader : ChromaShader
{
	private readonly Vector4 _skinColor;

	private readonly Vector4 _eyeColor;

	private readonly Vector4 _innerEyeColor;

	public WormShader()
	{
	}

	public WormShader(Color skinColor, Color eyeColor, Color innerEyeColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		_skinColor = ((Color)(ref skinColor)).ToVector4();
		_eyeColor = ((Color)(ref eyeColor)).ToVector4();
		_innerEyeColor = ((Color)(ref innerEyeColor)).ToVector4();
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.Low })]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			float amount = Math.Max(0f, (float)Math.Sin(time * -3f + fragment.GetCanvasPositionOfIndex(i).X));
			Vector4 color = Vector4.Lerp(_skinColor, _eyeColor, amount);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		time *= 0.25f;
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			canvasPositionOfIndex.X -= time * 1.5f;
			canvasPositionOfIndex.X %= 2f;
			if (canvasPositionOfIndex.X < 0f)
			{
				canvasPositionOfIndex.X += 2f;
			}
			Vector2 val = canvasPositionOfIndex - new Vector2(0.5f);
			float num = ((Vector2)(ref val)).Length();
			Vector4 vector = _skinColor;
			if (num < 0.5f)
			{
				float num2 = MathHelper.Clamp((num - 0.5f + 0.2f) / 0.2f, 0f, 1f);
				vector = Vector4.Lerp(vector, _eyeColor, 1f - num2);
				if (num < 0.4f)
				{
					num2 = MathHelper.Clamp((num - 0.4f + 0.2f) / 0.2f, 0f, 1f);
					vector = Vector4.Lerp(vector, _innerEyeColor, 1f - num2);
				}
			}
			fragment.SetColor(i, vector);
		}
	}
}
