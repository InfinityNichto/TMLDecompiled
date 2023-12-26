using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class TempleShader : ChromaShader
{
	private readonly Vector4 _backgroundColor = new Vector4(0.05f, 0.025f, 0f, 1f);

	private readonly Vector4 _glowColor;

	[RgbProcessor(new EffectDetailLevel[]
	{
		EffectDetailLevel.Low,
		EffectDetailLevel.High
	})]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector4 vector = _backgroundColor;
			float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.Y * 7) * 10f + time) % 10f - (canvasPositionOfIndex.X + 2f);
			if (num > 0f)
			{
				float amount = Math.Max(0f, 1.2f - num);
				if (num < 0.2f)
				{
					amount = num * 5f;
				}
				vector = Vector4.Lerp(vector, _glowColor, amount);
			}
			fragment.SetColor(i, vector);
		}
	}

	public TempleShader()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Color orange = Color.Orange;
		_glowColor = ((Color)(ref orange)).ToVector4();
		base._002Ector();
	}
}
