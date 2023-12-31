using System;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class MarbleBiome : MicroBiome
{
	private delegate bool SlabState(int x, int y, int scale);

	private static class SlabStates
	{
		public static bool Empty(int x, int y, int scale)
		{
			return false;
		}

		public static bool Solid(int x, int y, int scale)
		{
			return true;
		}

		public static bool HalfBrick(int x, int y, int scale)
		{
			return y >= scale / 2;
		}

		public static bool BottomRightFilled(int x, int y, int scale)
		{
			return x >= scale - y;
		}

		public static bool BottomLeftFilled(int x, int y, int scale)
		{
			return x < y;
		}

		public static bool TopRightFilled(int x, int y, int scale)
		{
			return x > y;
		}

		public static bool TopLeftFilled(int x, int y, int scale)
		{
			return x < scale - y;
		}
	}

	private struct Slab
	{
		public readonly SlabState State;

		public readonly bool HasWall;

		public bool IsSolid => State != new SlabState(SlabStates.Empty);

		private Slab(SlabState state, bool hasWall)
		{
			State = state;
			HasWall = hasWall;
		}

		public Slab WithState(SlabState state)
		{
			return new Slab(state, HasWall);
		}

		public static Slab Create(SlabState state, bool hasWall)
		{
			return new Slab(state, hasWall);
		}
	}

	private const int SCALE = 3;

	private Slab[,] _slabs;

	private void SmoothSlope(int x, int y)
	{
		Slab slab = _slabs[x, y];
		if (slab.IsSolid)
		{
			bool isSolid5 = _slabs[x, y - 1].IsSolid;
			bool isSolid2 = _slabs[x, y + 1].IsSolid;
			bool isSolid3 = _slabs[x - 1, y].IsSolid;
			bool isSolid4 = _slabs[x + 1, y].IsSolid;
			switch (((isSolid5 ? 1 : 0) << 3) | ((isSolid2 ? 1 : 0) << 2) | ((isSolid3 ? 1 : 0) << 1) | (isSolid4 ? 1 : 0))
			{
			case 10:
				_slabs[x, y] = slab.WithState(SlabStates.TopLeftFilled);
				break;
			case 9:
				_slabs[x, y] = slab.WithState(SlabStates.TopRightFilled);
				break;
			case 6:
				_slabs[x, y] = slab.WithState(SlabStates.BottomLeftFilled);
				break;
			case 5:
				_slabs[x, y] = slab.WithState(SlabStates.BottomRightFilled);
				break;
			case 4:
				_slabs[x, y] = slab.WithState(SlabStates.HalfBrick);
				break;
			default:
				_slabs[x, y] = slab.WithState(SlabStates.Solid);
				break;
			}
		}
	}

	private void PlaceSlab(Slab slab, int originX, int originY, int scale)
	{
		ushort num = 367;
		ushort wall = 178;
		if (WorldGen.drunkWorldGen)
		{
			num = 368;
			wall = 180;
		}
		int num2 = -1;
		int num3 = scale + 1;
		int num4 = 0;
		int num5 = scale;
		for (int i = num2; i < num3; i++)
		{
			if ((i == num2 || i == num3 - 1) && WorldGen.genRand.Next(2) == 0)
			{
				continue;
			}
			if (WorldGen.genRand.Next(2) == 0)
			{
				num4--;
			}
			if (WorldGen.genRand.Next(2) == 0)
			{
				num5++;
			}
			for (int j = num4; j < num5; j++)
			{
				Tile tile = GenBase._tiles[originX + i, originY + j];
				tile.ResetToType(TileID.Sets.Ore[tile.type] ? tile.type : num);
				bool active = slab.State(i, j, scale);
				tile.active(active);
				if (slab.HasWall)
				{
					tile.wall = wall;
				}
				WorldUtils.TileFrame(originX + i, originY + j, frameNeighbors: true);
				WorldGen.SquareWallFrame(originX + i, originY + j);
				Tile.SmoothSlope(originX + i, originY + j);
				if (WorldGen.SolidTile(originX + i, originY + j - 1) && GenBase._random.Next(4) == 0)
				{
					WorldGen.PlaceTight(originX + i, originY + j);
				}
				if (WorldGen.SolidTile(originX + i, originY + j) && GenBase._random.Next(4) == 0)
				{
					WorldGen.PlaceTight(originX + i, originY + j - 1);
				}
			}
		}
	}

	private static bool IsGroupSolid(int x, int y, int scale)
	{
		int num = 0;
		for (int i = 0; i < scale; i++)
		{
			for (int j = 0; j < scale; j++)
			{
				if (WorldGen.SolidOrSlopedTile(x + i, y + j))
				{
					num++;
				}
			}
		}
		return num > scale / 4 * 3;
	}

	public override bool Place(Point origin, StructureMap structures)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		if (WorldGen.BiomeTileCheck(origin.X, origin.Y))
		{
			return false;
		}
		if (_slabs == null)
		{
			_slabs = new Slab[56, 26];
		}
		int num = GenBase._random.Next(80, 150) / 3;
		int num6 = GenBase._random.Next(40, 60) / 3;
		int num7 = (num6 * 3 - GenBase._random.Next(20, 30)) / 3;
		origin.X -= num * 3 / 2;
		origin.Y -= num6 * 3 / 2;
		for (int i = -1; i < num + 1; i++)
		{
			double num8 = (double)(i - num / 2) / (double)num + 0.5;
			int num9 = (int)((0.5 - Math.Abs(num8 - 0.5)) * 5.0) - 2;
			for (int j = -1; j < num6 + 1; j++)
			{
				bool hasWall = true;
				bool flag = false;
				bool flag2 = IsGroupSolid(i * 3 + origin.X, j * 3 + origin.Y, 3);
				int num10 = Math.Abs(j - num6 / 2) - num7 / 4 + num9;
				if (num10 > 3)
				{
					flag = flag2;
					hasWall = false;
				}
				else if (num10 > 0)
				{
					flag = j - num6 / 2 > 0 || flag2;
					hasWall = j - num6 / 2 < 0 || num10 <= 2;
				}
				else if (num10 == 0)
				{
					flag = GenBase._random.Next(2) == 0 && (j - num6 / 2 > 0 || flag2);
				}
				if (Math.Abs(num8 - 0.5) > 0.35 + GenBase._random.NextDouble() * 0.1 && !flag2)
				{
					hasWall = false;
					flag = false;
				}
				_slabs[i + 1, j + 1] = Slab.Create(flag ? new SlabState(SlabStates.Solid) : new SlabState(SlabStates.Empty), hasWall);
			}
		}
		for (int k = 0; k < num; k++)
		{
			for (int l = 0; l < num6; l++)
			{
				SmoothSlope(k + 1, l + 1);
			}
		}
		int num11 = num / 2;
		int num12 = num6 / 2;
		int num13 = (num12 + 1) * (num12 + 1);
		double value = GenBase._random.NextDouble() * 2.0 - 1.0;
		double num2 = GenBase._random.NextDouble() * 2.0 - 1.0;
		double value2 = GenBase._random.NextDouble() * 2.0 - 1.0;
		double num3 = 0.0;
		for (int m = 0; m <= num; m++)
		{
			double num4 = (double)num12 / (double)num11 * (double)(m - num11);
			int num5 = Math.Min(num12, (int)Math.Sqrt(Math.Max(0.0, (double)num13 - num4 * num4)));
			num3 = ((m >= num / 2) ? (num3 + Utils.Lerp(num2, value2, (double)m / (double)(num / 2) - 1.0)) : (num3 + Utils.Lerp(value, num2, (double)m / (double)(num / 2))));
			for (int n = num12 - num5; n <= num12 + num5; n++)
			{
				PlaceSlab(_slabs[m + 1, n + 1], m * 3 + origin.X, n * 3 + origin.Y + (int)num3, 3);
			}
		}
		structures.AddStructure(new Rectangle(origin.X, origin.Y, num * 3, num6 * 3), 8);
		return true;
	}
}
