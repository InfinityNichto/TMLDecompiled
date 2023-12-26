using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.Utilities;

namespace Terraria.Graphics.Light;

public class LightMap
{
	private Vector3[] _colors;

	private LightMaskMode[] _mask;

	private FastRandom _random = FastRandom.CreateWithRandomSeed();

	private const int DEFAULT_WIDTH = 203;

	private const int DEFAULT_HEIGHT = 203;

	public int NonVisiblePadding { get; set; }

	public int Width { get; private set; }

	public int Height { get; private set; }

	public float LightDecayThroughAir { get; set; }

	public float LightDecayThroughSolid { get; set; }

	public Vector3 LightDecayThroughWater { get; set; }

	public Vector3 LightDecayThroughHoney { get; set; }

	public Vector3 this[int x, int y]
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return _colors[IndexOf(x, y)];
		}
		set
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			_colors[IndexOf(x, y)] = value;
		}
	}

	public LightMap()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		LightDecayThroughAir = 0.91f;
		LightDecayThroughSolid = 0.56f;
		LightDecayThroughWater = new Vector3(0.88f, 0.96f, 1.015f) * 0.91f;
		LightDecayThroughHoney = new Vector3(0.75f, 0.7f, 0.6f) * 0.91f;
		Width = 203;
		Height = 203;
		_colors = (Vector3[])(object)new Vector3[41209];
		_mask = new LightMaskMode[41209];
	}

	public void GetLight(int x, int y, out Vector3 color)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		color = _colors[IndexOf(x, y)];
	}

	public LightMaskMode GetMask(int x, int y)
	{
		return _mask[IndexOf(x, y)];
	}

	public void Clear()
	{
		for (int i = 0; i < _colors.Length; i++)
		{
			_colors[i].X = 0f;
			_colors[i].Y = 0f;
			_colors[i].Z = 0f;
			_mask[i] = LightMaskMode.None;
		}
	}

	public void SetMaskAt(int x, int y, LightMaskMode mode)
	{
		_mask[IndexOf(x, y)] = mode;
	}

	public void Blur()
	{
		BlurPass();
		BlurPass();
		_random.NextSeed();
	}

	private void BlurPass()
	{
		FastParallel.For(0, Width, delegate(int start, int end, object context)
		{
			for (int j = start; j < end; j++)
			{
				BlurLine(IndexOf(j, 0), IndexOf(j, Height - 1 - NonVisiblePadding), 1);
				BlurLine(IndexOf(j, Height - 1), IndexOf(j, NonVisiblePadding), -1);
			}
		});
		FastParallel.For(0, Height, delegate(int start, int end, object context)
		{
			for (int i = start; i < end; i++)
			{
				BlurLine(IndexOf(0, i), IndexOf(Width - 1 - NonVisiblePadding, i), Height);
				BlurLine(IndexOf(Width - 1, i), IndexOf(NonVisiblePadding, i), -Height);
			}
		});
	}

	private void BlurLine(int startIndex, int endIndex, int stride)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.Zero;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		for (int i = startIndex; i != endIndex + stride; i += stride)
		{
			if (zero.X < _colors[i].X)
			{
				zero.X = _colors[i].X;
				flag = false;
			}
			else if (!flag)
			{
				if (zero.X < 0.0185f)
				{
					flag = true;
				}
				else
				{
					_colors[i].X = zero.X;
				}
			}
			if (zero.Y < _colors[i].Y)
			{
				zero.Y = _colors[i].Y;
				flag2 = false;
			}
			else if (!flag2)
			{
				if (zero.Y < 0.0185f)
				{
					flag2 = true;
				}
				else
				{
					_colors[i].Y = zero.Y;
				}
			}
			if (zero.Z < _colors[i].Z)
			{
				zero.Z = _colors[i].Z;
				flag3 = false;
			}
			else if (!flag3)
			{
				if (zero.Z < 0.0185f)
				{
					flag3 = true;
				}
				else
				{
					_colors[i].Z = zero.Z;
				}
			}
			if (flag && flag3 && flag2)
			{
				continue;
			}
			switch (_mask[i])
			{
			case LightMaskMode.None:
				if (!flag)
				{
					zero.X *= LightDecayThroughAir;
				}
				if (!flag2)
				{
					zero.Y *= LightDecayThroughAir;
				}
				if (!flag3)
				{
					zero.Z *= LightDecayThroughAir;
				}
				break;
			case LightMaskMode.Solid:
				if (!flag)
				{
					zero.X *= LightDecayThroughSolid;
				}
				if (!flag2)
				{
					zero.Y *= LightDecayThroughSolid;
				}
				if (!flag3)
				{
					zero.Z *= LightDecayThroughSolid;
				}
				break;
			case LightMaskMode.Water:
			{
				float num = (float)_random.WithModifier((ulong)i).Next(98, 100) / 100f;
				if (!flag)
				{
					zero.X *= LightDecayThroughWater.X * num;
				}
				if (!flag2)
				{
					zero.Y *= LightDecayThroughWater.Y * num;
				}
				if (!flag3)
				{
					zero.Z *= LightDecayThroughWater.Z * num;
				}
				break;
			}
			case LightMaskMode.Honey:
				if (!flag)
				{
					zero.X *= LightDecayThroughHoney.X;
				}
				if (!flag2)
				{
					zero.Y *= LightDecayThroughHoney.Y;
				}
				if (!flag3)
				{
					zero.Z *= LightDecayThroughHoney.Z;
				}
				break;
			}
		}
	}

	private int IndexOf(int x, int y)
	{
		return x * Height + y;
	}

	public void SetSize(int width, int height)
	{
		int neededSize = (width + 1) * (height + 1);
		if (neededSize > _colors.Length)
		{
			_colors = (Vector3[])(object)new Vector3[neededSize];
			_mask = new LightMaskMode[neededSize];
		}
		Width = width;
		Height = height;
	}
}
