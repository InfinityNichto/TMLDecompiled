using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class SandstormShader : ChromaShader
{
	private readonly Vector4 _backColor = new Vector4(0.2f, 0f, 0f, 1f);

	private readonly Vector4 _frontColor = new Vector4(1f, 0.5f, 0f, 1f);

	[RgbProcessor(new EffectDetailLevel[]
	{
		EffectDetailLevel.Low,
		EffectDetailLevel.High
	})]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		if (quality == EffectDetailLevel.Low)
		{
			time *= 0.25f;
		}
		for (int i = 0; i < fragment.Count; i++)
		{
			float staticNoise = NoiseHelper.GetStaticNoise(fragment.GetCanvasPositionOfIndex(i) * 0.3f + new Vector2(time, 0f - time) * 0.5f);
			Vector4 color = Vector4.Lerp(_backColor, _frontColor, staticNoise);
			fragment.SetColor(i, color);
		}
	}
}
