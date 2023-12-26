using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class DeathShader : ChromaShader
{
	private readonly Vector4 _primaryColor;

	private readonly Vector4 _secondaryColor;

	public DeathShader(Color primaryColor, Color secondaryColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_primaryColor = ((Color)(ref primaryColor)).ToVector4();
		_secondaryColor = ((Color)(ref secondaryColor)).ToVector4();
	}

	[RgbProcessor(new EffectDetailLevel[]
	{
		EffectDetailLevel.High,
		EffectDetailLevel.Low
	})]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		time *= 3f;
		float amount = 0f;
		float num = time % ((float)Math.PI * 4f);
		if (num < (float)Math.PI)
		{
			amount = (float)Math.Sin(num);
		}
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector4 color = Vector4.Lerp(_primaryColor, _secondaryColor, amount);
			fragment.SetColor(i, color);
		}
	}
}
