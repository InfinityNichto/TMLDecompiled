using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class DD2Shader : ChromaShader
{
	private readonly Vector4 _darkGlowColor;

	private readonly Vector4 _lightGlowColor;

	public DD2Shader(Color darkGlowColor, Color lightGlowColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_darkGlowColor = ((Color)(ref darkGlowColor)).ToVector4();
		_lightGlowColor = ((Color)(ref lightGlowColor)).ToVector4();
	}

	[RgbProcessor(new EffectDetailLevel[]
	{
		EffectDetailLevel.Low,
		EffectDetailLevel.High
	})]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = fragment.CanvasCenter;
		if (quality == EffectDetailLevel.Low)
		{
			((Vector2)(ref vector))._002Ector(1.7f, 0.5f);
		}
		time *= 0.5f;
		Vector4 value = default(Vector4);
		for (int i = 0; i < fragment.Count; i++)
		{
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			((Vector4)(ref value))._002Ector(0f, 0f, 0f, 1f);
			Vector2 val = canvasPositionOfIndex - vector;
			float num = ((Vector2)(ref val)).Length();
			float num2 = num * num * 0.75f;
			float num3 = (num - time) % 1f;
			if (num3 < 0f)
			{
				num3 += 1f;
			}
			num3 = ((!(num3 > 0.8f)) ? (num3 / 0.8f) : (num3 * (1f - (num3 - 1f + 0.2f) / 0.2f)));
			Vector4 value2 = Vector4.Lerp(_darkGlowColor, _lightGlowColor, num3 * num3);
			num3 *= MathHelper.Clamp(1f - num2, 0f, 1f) * 0.75f + 0.25f;
			value = Vector4.Lerp(value, value2, num3);
			if (num < 0.5f)
			{
				float amount = 1f - MathHelper.Clamp((num - 0.5f + 0.4f) / 0.4f, 0f, 1f);
				value = Vector4.Lerp(value, _lightGlowColor, amount);
			}
			fragment.SetColor(i, value);
		}
	}
}
