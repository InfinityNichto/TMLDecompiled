using System;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class TerrainPass : GenPass
{
	private enum TerrainFeatureType
	{
		Plateau,
		Hill,
		Dale,
		Mountain,
		Valley
	}

	private class SurfaceHistory
	{
		private readonly double[] _heights;

		private int _index;

		public double this[int index]
		{
			get
			{
				return _heights[(index + _index) % _heights.Length];
			}
			set
			{
				_heights[(index + _index) % _heights.Length] = value;
			}
		}

		public int Length => _heights.Length;

		public SurfaceHistory(int size)
		{
			_heights = new double[size];
		}

		public void Record(double height)
		{
			_heights[_index] = height;
			_index = (_index + 1) % _heights.Length;
		}
	}

	public TerrainPass()
		: base("Terrain", 449.3721923828125)
	{
	}

	protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
	{
		int num = configuration.Get<int>("FlatBeachPadding");
		progress.Message = Lang.gen[0].Value;
		TerrainFeatureType terrainFeatureType = TerrainFeatureType.Plateau;
		int num7 = 0;
		double num8 = (double)Main.maxTilesY * 0.3;
		num8 *= (double)GenBase._random.Next(90, 110) * 0.005;
		double num9 = num8 + (double)Main.maxTilesY * 0.2;
		num9 *= (double)GenBase._random.Next(90, 110) * 0.01;
		if (WorldGen.remixWorldGen)
		{
			num9 = (double)Main.maxTilesY * 0.5;
			if (Main.maxTilesX > 2500)
			{
				num9 = (double)Main.maxTilesY * 0.6;
			}
			num9 *= (double)GenBase._random.Next(95, 106) * 0.01;
		}
		double num10 = num8;
		double num11 = num8;
		double num12 = num9;
		double num13 = num9;
		double num14 = (double)Main.maxTilesY * 0.23;
		SurfaceHistory surfaceHistory = new SurfaceHistory(500);
		num7 = GenVars.leftBeachEnd + num;
		for (int i = 0; i < Main.maxTilesX; i++)
		{
			progress.Set((double)i / (double)Main.maxTilesX);
			num10 = Math.Min(num8, num10);
			num11 = Math.Max(num8, num11);
			num12 = Math.Min(num9, num12);
			num13 = Math.Max(num9, num13);
			if (num7 <= 0)
			{
				terrainFeatureType = (TerrainFeatureType)GenBase._random.Next(0, 5);
				num7 = GenBase._random.Next(5, 40);
				if (terrainFeatureType == TerrainFeatureType.Plateau)
				{
					num7 *= (int)((double)GenBase._random.Next(5, 30) * 0.2);
				}
			}
			num7--;
			if ((double)i > (double)Main.maxTilesX * 0.45 && (double)i < (double)Main.maxTilesX * 0.55 && (terrainFeatureType == TerrainFeatureType.Mountain || terrainFeatureType == TerrainFeatureType.Valley))
			{
				terrainFeatureType = (TerrainFeatureType)GenBase._random.Next(3);
			}
			if ((double)i > (double)Main.maxTilesX * 0.48 && (double)i < (double)Main.maxTilesX * 0.52)
			{
				terrainFeatureType = TerrainFeatureType.Plateau;
			}
			num8 += GenerateWorldSurfaceOffset(terrainFeatureType);
			double num2 = 0.17;
			double num3 = 0.26;
			if (WorldGen.drunkWorldGen)
			{
				num2 = 0.15;
				num3 = 0.28;
			}
			if (i < GenVars.leftBeachEnd + num || i > GenVars.rightBeachStart - num)
			{
				num8 = Utils.Clamp(num8, (double)Main.maxTilesY * 0.17, num14);
			}
			else if (num8 < (double)Main.maxTilesY * num2)
			{
				num8 = (double)Main.maxTilesY * num2;
				num7 = 0;
			}
			else if (num8 > (double)Main.maxTilesY * num3)
			{
				num8 = (double)Main.maxTilesY * num3;
				num7 = 0;
			}
			while (GenBase._random.Next(0, 3) == 0)
			{
				num9 += (double)GenBase._random.Next(-2, 3);
			}
			if (WorldGen.remixWorldGen)
			{
				if (Main.maxTilesX > 2500)
				{
					if (num9 > (double)Main.maxTilesY * 0.7)
					{
						num9 -= 1.0;
					}
				}
				else if (num9 > (double)Main.maxTilesY * 0.6)
				{
					num9 -= 1.0;
				}
			}
			else
			{
				if (num9 < num8 + (double)Main.maxTilesY * 0.06)
				{
					num9 += 1.0;
				}
				if (num9 > num8 + (double)Main.maxTilesY * 0.35)
				{
					num9 -= 1.0;
				}
			}
			surfaceHistory.Record(num8);
			FillColumn(i, num8, num9);
			if (i == GenVars.rightBeachStart - num)
			{
				if (num8 > num14)
				{
					RetargetSurfaceHistory(surfaceHistory, i, num14);
				}
				terrainFeatureType = TerrainFeatureType.Plateau;
				num7 = Main.maxTilesX - i;
			}
		}
		Main.worldSurface = (int)(num11 + 25.0);
		Main.rockLayer = num13;
		double num4 = (int)((Main.rockLayer - Main.worldSurface) / 6.0) * 6;
		Main.rockLayer = (int)(Main.worldSurface + num4);
		int num15 = (int)(Main.rockLayer + (double)Main.maxTilesY) / 2 + GenBase._random.Next(-100, 20);
		int lavaLine = num15 + GenBase._random.Next(50, 80);
		if (WorldGen.remixWorldGen)
		{
			lavaLine = (int)(Main.worldSurface * 4.0 + num9) / 5;
		}
		int num5 = 20;
		if (num12 < num11 + (double)num5)
		{
			double num16 = (num12 + num11) / 2.0;
			double num6 = Math.Abs(num12 - num11);
			if (num6 < (double)num5)
			{
				num6 = num5;
			}
			num12 = num16 + num6 / 2.0;
			num11 = num16 - num6 / 2.0;
		}
		GenVars.rockLayer = num9;
		GenVars.rockLayerHigh = num13;
		GenVars.rockLayerLow = num12;
		GenVars.worldSurface = num8;
		GenVars.worldSurfaceHigh = num11;
		GenVars.worldSurfaceLow = num10;
		GenVars.waterLine = num15;
		GenVars.lavaLine = lavaLine;
	}

	private static void FillColumn(int x, double worldSurface, double rockLayer)
	{
		for (int i = 0; (double)i < worldSurface; i++)
		{
			Main.tile[x, i].active(active: false);
			Main.tile[x, i].frameX = -1;
			Main.tile[x, i].frameY = -1;
		}
		for (int j = (int)worldSurface; j < Main.maxTilesY; j++)
		{
			if ((double)j < rockLayer)
			{
				Main.tile[x, j].active(active: true);
				Main.tile[x, j].type = 0;
				Main.tile[x, j].frameX = -1;
				Main.tile[x, j].frameY = -1;
			}
			else
			{
				Main.tile[x, j].active(active: true);
				Main.tile[x, j].type = 1;
				Main.tile[x, j].frameX = -1;
				Main.tile[x, j].frameY = -1;
			}
		}
	}

	private static void RetargetColumn(int x, double worldSurface)
	{
		for (int i = 0; (double)i < worldSurface; i++)
		{
			Main.tile[x, i].active(active: false);
			Main.tile[x, i].frameX = -1;
			Main.tile[x, i].frameY = -1;
		}
		for (int j = (int)worldSurface; j < Main.maxTilesY; j++)
		{
			if (Main.tile[x, j].type != 1 || !Main.tile[x, j].active())
			{
				Main.tile[x, j].active(active: true);
				Main.tile[x, j].type = 0;
				Main.tile[x, j].frameX = -1;
				Main.tile[x, j].frameY = -1;
			}
		}
	}

	private static double GenerateWorldSurfaceOffset(TerrainFeatureType featureType)
	{
		double num = 0.0;
		if ((WorldGen.drunkWorldGen || WorldGen.getGoodWorldGen || WorldGen.remixWorldGen) && WorldGen.genRand.Next(2) == 0)
		{
			switch (featureType)
			{
			case TerrainFeatureType.Plateau:
				while (GenBase._random.Next(0, 6) == 0)
				{
					num += (double)GenBase._random.Next(-1, 2);
				}
				break;
			case TerrainFeatureType.Hill:
				while (GenBase._random.Next(0, 3) == 0)
				{
					num -= 1.0;
				}
				while (GenBase._random.Next(0, 10) == 0)
				{
					num += 1.0;
				}
				break;
			case TerrainFeatureType.Dale:
				while (GenBase._random.Next(0, 3) == 0)
				{
					num += 1.0;
				}
				while (GenBase._random.Next(0, 10) == 0)
				{
					num -= 1.0;
				}
				break;
			case TerrainFeatureType.Mountain:
				while (GenBase._random.Next(0, 3) != 0)
				{
					num -= 1.0;
				}
				while (GenBase._random.Next(0, 6) == 0)
				{
					num += 1.0;
				}
				break;
			case TerrainFeatureType.Valley:
				while (GenBase._random.Next(0, 3) != 0)
				{
					num += 1.0;
				}
				while (GenBase._random.Next(0, 5) == 0)
				{
					num -= 1.0;
				}
				break;
			}
		}
		else
		{
			switch (featureType)
			{
			case TerrainFeatureType.Plateau:
				while (GenBase._random.Next(0, 7) == 0)
				{
					num += (double)GenBase._random.Next(-1, 2);
				}
				break;
			case TerrainFeatureType.Hill:
				while (GenBase._random.Next(0, 4) == 0)
				{
					num -= 1.0;
				}
				while (GenBase._random.Next(0, 10) == 0)
				{
					num += 1.0;
				}
				break;
			case TerrainFeatureType.Dale:
				while (GenBase._random.Next(0, 4) == 0)
				{
					num += 1.0;
				}
				while (GenBase._random.Next(0, 10) == 0)
				{
					num -= 1.0;
				}
				break;
			case TerrainFeatureType.Mountain:
				while (GenBase._random.Next(0, 2) == 0)
				{
					num -= 1.0;
				}
				while (GenBase._random.Next(0, 6) == 0)
				{
					num += 1.0;
				}
				break;
			case TerrainFeatureType.Valley:
				while (GenBase._random.Next(0, 2) == 0)
				{
					num += 1.0;
				}
				while (GenBase._random.Next(0, 5) == 0)
				{
					num -= 1.0;
				}
				break;
			}
		}
		return num;
	}

	private static void RetargetSurfaceHistory(SurfaceHistory history, int targetX, double targetHeight)
	{
		for (int i = 0; i < history.Length / 2; i++)
		{
			if (history[history.Length - 1] <= targetHeight)
			{
				break;
			}
			for (int j = 0; j < history.Length - i * 2; j++)
			{
				double num = history[history.Length - j - 1];
				num -= 1.0;
				history[history.Length - j - 1] = num;
				if (num <= targetHeight)
				{
					break;
				}
			}
		}
		for (int k = 0; k < history.Length; k++)
		{
			double worldSurface = history[history.Length - k - 1];
			RetargetColumn(targetX - k, worldSurface);
		}
	}
}
