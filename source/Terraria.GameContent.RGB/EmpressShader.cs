using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class EmpressShader : ChromaShader
{
	private static readonly Vector4[] _colors = (Vector4[])(object)new Vector4[12]
	{
		new Vector4(1f, 0.1f, 0.1f, 1f),
		new Vector4(1f, 0.5f, 0.1f, 1f),
		new Vector4(1f, 1f, 0.1f, 1f),
		new Vector4(0.5f, 1f, 0.1f, 1f),
		new Vector4(0.1f, 1f, 0.1f, 1f),
		new Vector4(0.1f, 1f, 0.5f, 1f),
		new Vector4(0.1f, 1f, 1f, 1f),
		new Vector4(0.1f, 0.5f, 1f, 1f),
		new Vector4(0.1f, 0.1f, 1f, 1f),
		new Vector4(0.5f, 0.1f, 1f, 1f),
		new Vector4(1f, 0.1f, 1f, 1f),
		new Vector4(1f, 0.1f, 0.5f, 1f)
	};

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High }, IsTransparent = false)]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		float num = time * 2f;
		for (int i = 0; i < fragment.Count; i++)
		{
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			float staticNoise = NoiseHelper.GetStaticNoise(gridPositionOfIndex.X);
			float num5 = MathHelper.Max(0f, (float)Math.Cos((staticNoise + num) * ((float)Math.PI * 2f) * 0.2f));
			Color val = Color.Lerp(Color.Black, Color.Indigo, 0.5f);
			Vector4 value = ((Color)(ref val)).ToVector4();
			float num2 = Math.Max(0f, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f + canvasPositionOfIndex.X * 1f));
			num2 = 0f;
			value = Vector4.Lerp(value, new Vector4(1f, 0.1f, 0.1f, 1f), num2);
			float num3 = (num5 + canvasPositionOfIndex.X + canvasPositionOfIndex.Y) % 1f;
			if (num3 > 0f)
			{
				int num4 = (gridPositionOfIndex.X + gridPositionOfIndex.Y) % _colors.Length;
				if (num4 < 0)
				{
					num4 += _colors.Length;
				}
				val = Main.hslToRgb(((canvasPositionOfIndex.X + canvasPositionOfIndex.Y) * 0.15f + time * 0.1f) % 1f, 1f, 0.5f);
				Vector4 value2 = ((Color)(ref val)).ToVector4();
				value = Vector4.Lerp(value, value2, num3);
			}
			fragment.SetColor(i, value);
		}
	}

	private static void RedsVersion(Fragment fragment, float time)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		time *= 3f;
		for (int i = 0; i < fragment.Count; i++)
		{
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			float num = (NoiseHelper.GetStaticNoise(gridPositionOfIndex.X) * 7f + time * 0.4f) % 7f - canvasPositionOfIndex.Y;
			Vector4 vector = default(Vector4);
			if (num > 0f)
			{
				float amount = Math.Max(0f, 1.4f - num);
				if (num < 0.4f)
				{
					amount = num / 0.4f;
				}
				int num2 = (gridPositionOfIndex.X + _colors.Length + (int)(time / 6f)) % _colors.Length;
				vector = Vector4.Lerp(vector, _colors[num2], amount);
			}
			fragment.SetColor(i, vector);
		}
	}
}
