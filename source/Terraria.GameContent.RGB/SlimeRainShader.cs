using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class SlimeRainShader : ChromaShader
{
	private static readonly Vector4[] _colors;

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High }, IsTransparent = true)]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		Vector4 vector = default(Vector4);
		((Vector4)(ref vector))._002Ector(0f, 0f, 0f, 0.75f);
		for (int i = 0; i < fragment.Count; i++)
		{
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 7f + time * 0.4f) % 7f - canvasPositionOfIndex.Y;
			Vector4 vector2 = vector;
			if (num > 0f)
			{
				float amount = Math.Max(0f, 1.2f - num);
				if (num < 0.4f)
				{
					amount = num / 0.4f;
				}
				int num2 = (gridPositionOfIndex.X % _colors.Length + _colors.Length) % _colors.Length;
				vector2 = Vector4.Lerp(vector2, _colors[num2], amount);
			}
			fragment.SetColor(i, vector2);
		}
	}

	static SlimeRainShader()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Vector4[] array = new Vector4[3];
		Color val = Color.Blue;
		array[0] = ((Color)(ref val)).ToVector4();
		val = Color.Green;
		array[1] = ((Color)(ref val)).ToVector4();
		val = Color.Purple;
		array[2] = ((Color)(ref val)).ToVector4();
		_colors = (Vector4[])(object)array;
	}
}
