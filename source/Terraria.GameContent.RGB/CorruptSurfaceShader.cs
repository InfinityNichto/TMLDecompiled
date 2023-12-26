using System;
using Microsoft.Xna.Framework;
using ReLogic.Peripherals.RGB;

namespace Terraria.GameContent.RGB;

public class CorruptSurfaceShader : ChromaShader
{
	private readonly Vector4 _baseColor;

	private readonly Vector4 _skyColor;

	private Vector4 _lightColor;

	public CorruptSurfaceShader(Color color)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		_baseColor = ((Color)(ref color)).ToVector4();
		Vector4 baseColor = _baseColor;
		Color deepSkyBlue = Color.DeepSkyBlue;
		_skyColor = Vector4.Lerp(baseColor, ((Color)(ref deepSkyBlue)).ToVector4(), 0.5f);
	}

	public CorruptSurfaceShader(Color vineColor, Color skyColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_baseColor = ((Color)(ref vineColor)).ToVector4();
		_skyColor = ((Color)(ref skyColor)).ToVector4();
	}

	public override void Update(float elapsedTime)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		_lightColor = ((Color)(ref Main.ColorOfTheSkies)).ToVector4() * 0.75f + Vector4.One * 0.25f;
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.Low })]
	private void ProcessLowDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		Vector4 value = _skyColor * _lightColor;
		for (int i = 0; i < fragment.Count; i++)
		{
			float num = (float)Math.Sin(time * 0.5f + fragment.GetCanvasPositionOfIndex(i).X) * 0.5f + 0.5f;
			Vector4 color = Vector4.Lerp(_baseColor, value, num);
			fragment.SetColor(i, color);
		}
	}

	[RgbProcessor(new EffectDetailLevel[] { EffectDetailLevel.High })]
	private void ProcessHighDetail(RgbDevice device, Fragment fragment, EffectDetailLevel quality, float time)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		Vector4 vector = _skyColor * _lightColor;
		for (int i = 0; i < fragment.Count; i++)
		{
			Point gridPositionOfIndex = fragment.GetGridPositionOfIndex(i);
			Vector2 canvasPositionOfIndex = fragment.GetCanvasPositionOfIndex(i);
			float staticNoise = NoiseHelper.GetStaticNoise(gridPositionOfIndex.X);
			staticNoise = (staticNoise * 10f + time * 0.4f) % 10f;
			float num = 1f;
			if (staticNoise > 1f)
			{
				num = MathHelper.Clamp(1f - (staticNoise - 1.4f), 0f, 1f);
				staticNoise = 1f;
			}
			float num2 = (float)Math.Sin(canvasPositionOfIndex.X) * 0.3f + 0.7f;
			float num3 = staticNoise - (1f - canvasPositionOfIndex.Y);
			Vector4 vector2 = vector;
			if (num3 > 0f)
			{
				float num4 = 1f;
				if (num3 < 0.2f)
				{
					num4 = num3 * 5f;
				}
				vector2 = Vector4.Lerp(vector2, _baseColor, num4 * num);
			}
			if (canvasPositionOfIndex.Y > num2)
			{
				vector2 = _baseColor;
			}
			fragment.SetColor(i, vector2);
		}
	}
}
