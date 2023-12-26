using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Utilities;

namespace Terraria.GameContent.Biomes.Desert;

public static class DesertHive
{
	private struct Block
	{
		public Vector2D Position;

		public Block(double x, double y)
		{
			Position = new Vector2D(x, y);
		}
	}

	private class Cluster : List<Block>
	{
	}

	private class ClusterGroup : List<Cluster>
	{
		public readonly int Width;

		public readonly int Height;

		private ClusterGroup(int width, int height)
		{
			Width = width;
			Height = height;
			Generate();
		}

		public static ClusterGroup FromDescription(DesertDescription description)
		{
			return new ClusterGroup(description.BlockColumnCount, description.BlockRowCount);
		}

		private static void SearchForCluster(bool[,] blockMap, List<Point> pointCluster, int x, int y, int level = 2)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			pointCluster.Add(new Point(x, y));
			blockMap[x, y] = false;
			level--;
			if (level != -1)
			{
				if (x > 0 && blockMap[x - 1, y])
				{
					SearchForCluster(blockMap, pointCluster, x - 1, y, level);
				}
				if (x < blockMap.GetLength(0) - 1 && blockMap[x + 1, y])
				{
					SearchForCluster(blockMap, pointCluster, x + 1, y, level);
				}
				if (y > 0 && blockMap[x, y - 1])
				{
					SearchForCluster(blockMap, pointCluster, x, y - 1, level);
				}
				if (y < blockMap.GetLength(1) - 1 && blockMap[x, y + 1])
				{
					SearchForCluster(blockMap, pointCluster, x, y + 1, level);
				}
			}
		}

		private static void AttemptClaim(int x, int y, int[,] clusterIndexMap, List<List<Point>> pointClusters, int index)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			int num = clusterIndexMap[x, y];
			if (num == -1 || num == index)
			{
				return;
			}
			int num2 = ((WorldGen.genRand.Next(2) == 0) ? (-1) : index);
			foreach (Point item in pointClusters[num])
			{
				clusterIndexMap[item.X, item.Y] = num2;
			}
		}

		private void Generate()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			Clear();
			bool[,] array = new bool[Width, Height];
			int num = Width / 2 - 1;
			int num2 = Height / 2 - 1;
			int num3 = (num + 1) * (num + 1);
			Point point = default(Point);
			((Point)(ref point))._002Ector(num, num2);
			for (int i = point.Y - num2; i <= point.Y + num2; i++)
			{
				double num4 = (double)num / (double)num2 * (double)(i - point.Y);
				int num5 = Math.Min(num, (int)Math.Sqrt((double)num3 - num4 * num4));
				for (int j = point.X - num5; j <= point.X + num5; j++)
				{
					array[j, i] = WorldGen.genRand.Next(2) == 0;
				}
			}
			List<List<Point>> list = new List<List<Point>>();
			for (int k = 0; k < array.GetLength(0); k++)
			{
				for (int l = 0; l < array.GetLength(1); l++)
				{
					if (array[k, l] && WorldGen.genRand.Next(2) == 0)
					{
						List<Point> list2 = new List<Point>();
						SearchForCluster(array, list2, k, l);
						if (list2.Count > 2)
						{
							list.Add(list2);
						}
					}
				}
			}
			int[,] array2 = new int[array.GetLength(0), array.GetLength(1)];
			for (int m = 0; m < array2.GetLength(0); m++)
			{
				for (int n = 0; n < array2.GetLength(1); n++)
				{
					array2[m, n] = -1;
				}
			}
			for (int num6 = 0; num6 < list.Count; num6++)
			{
				foreach (Point item in list[num6])
				{
					array2[item.X, item.Y] = num6;
				}
			}
			for (int num7 = 0; num7 < list.Count; num7++)
			{
				foreach (Point item5 in list[num7])
				{
					int x = item5.X;
					int y = item5.Y;
					if (array2[x, y] == -1)
					{
						break;
					}
					int index = array2[x, y];
					if (x > 0)
					{
						AttemptClaim(x - 1, y, array2, list, index);
					}
					if (x < array2.GetLength(0) - 1)
					{
						AttemptClaim(x + 1, y, array2, list, index);
					}
					if (y > 0)
					{
						AttemptClaim(x, y - 1, array2, list, index);
					}
					if (y < array2.GetLength(1) - 1)
					{
						AttemptClaim(x, y + 1, array2, list, index);
					}
				}
			}
			foreach (List<Point> item6 in list)
			{
				item6.Clear();
			}
			for (int num8 = 0; num8 < array2.GetLength(0); num8++)
			{
				for (int num9 = 0; num9 < array2.GetLength(1); num9++)
				{
					if (array2[num8, num9] != -1)
					{
						list[array2[num8, num9]].Add(new Point(num8, num9));
					}
				}
			}
			foreach (List<Point> item2 in list)
			{
				if (item2.Count < 4)
				{
					item2.Clear();
				}
			}
			foreach (List<Point> item3 in list)
			{
				Cluster cluster = new Cluster();
				if (item3.Count <= 0)
				{
					continue;
				}
				foreach (Point item4 in item3)
				{
					cluster.Add(new Block((double)item4.X + (WorldGen.genRand.NextDouble() - 0.5) * 0.5, (double)item4.Y + (WorldGen.genRand.NextDouble() - 0.5) * 0.5));
				}
				Add(cluster);
			}
		}
	}

	[Flags]
	private enum PostPlacementEffect : byte
	{
		None = 0,
		Smooth = 1
	}

	public static void Place(DesertDescription description)
	{
		ClusterGroup clusters = ClusterGroup.FromDescription(description);
		PlaceClusters(description, clusters);
		AddTileVariance(description);
	}

	private static void PlaceClusters(DesertDescription description, ClusterGroup clusters)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Rectangle hive = description.Hive;
		((Rectangle)(ref hive)).Inflate(20, 20);
		PostPlacementEffect[,] array = new PostPlacementEffect[hive.Width, hive.Height];
		PlaceClustersArea(description, clusters, hive, array, Point.Zero);
		for (int i = ((Rectangle)(ref hive)).Left; i < ((Rectangle)(ref hive)).Right; i++)
		{
			for (int j = ((Rectangle)(ref hive)).Top; j < ((Rectangle)(ref hive)).Bottom; j++)
			{
				PostPlacementEffect postPlacementEffect = array[i - ((Rectangle)(ref hive)).Left, j - ((Rectangle)(ref hive)).Top];
				if (postPlacementEffect.HasFlag(PostPlacementEffect.Smooth))
				{
					Tile.SmoothSlope(i, j, applyToNeighbors: false);
				}
			}
		}
	}

	private static void PlaceClustersArea(DesertDescription description, ClusterGroup clusters, Rectangle area, PostPlacementEffect[,] postEffectMap, Point postEffectMapOffset)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		FastRandom fastRandom = new FastRandom(Main.ActiveWorldFileData.Seed).WithModifier(57005uL);
		Vector2D vector2D = new Vector2D(description.Hive.Width, description.Hive.Height);
		Vector2D vector2D2 = new Vector2D(clusters.Width, clusters.Height);
		Vector2D vector2D3 = description.BlockScale / 2.0;
		for (int i = ((Rectangle)(ref area)).Left; i < ((Rectangle)(ref area)).Right; i++)
		{
			for (int j = ((Rectangle)(ref area)).Top; j < ((Rectangle)(ref area)).Bottom; j++)
			{
				byte liquid = Main.tile[i, j].liquid;
				if (!WorldGen.InWorld(i, j, 1))
				{
					continue;
				}
				double num = 0.0;
				int num2 = -1;
				double num3 = 0.0;
				ushort type = 53;
				if (fastRandom.Next(3) == 0)
				{
					type = 397;
				}
				int num4 = i - description.Hive.X;
				int num5 = j - description.Hive.Y;
				Vector2D value = (new Vector2D(num4, num5) - vector2D3) / vector2D * vector2D2;
				for (int k = 0; k < clusters.Count; k++)
				{
					Cluster cluster = clusters[k];
					if (Math.Abs(cluster[0].Position.X - value.X) > 10.0 || Math.Abs(cluster[0].Position.Y - value.Y) > 10.0)
					{
						continue;
					}
					double num6 = 0.0;
					foreach (Block item in cluster)
					{
						num6 += 1.0 / Vector2D.DistanceSquared(item.Position, value);
					}
					if (num6 > num)
					{
						if (num > num3)
						{
							num3 = num;
						}
						num = num6;
						num2 = k;
					}
					else if (num6 > num3)
					{
						num3 = num6;
					}
				}
				double num7 = num + num3;
				Tile tile = Main.tile[i, j];
				bool flag = ((new Vector2D(num4, num5) - vector2D3) / vector2D * 2.0 - Vector2D.One).Length() >= 0.8;
				PostPlacementEffect postPlacementEffect = PostPlacementEffect.None;
				bool flag2 = true;
				if (num7 > 3.5)
				{
					postPlacementEffect = PostPlacementEffect.Smooth;
					tile.ClearEverything();
					if (!WorldGen.remixWorldGen || !((double)j > Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2)))
					{
						tile.wall = 187;
						if (num2 % 15 == 2)
						{
							tile.ResetToType(404);
						}
					}
				}
				else if (num7 > 1.8)
				{
					if (!WorldGen.remixWorldGen || !((double)j > Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2)))
					{
						tile.wall = 187;
					}
					if ((double)j < Main.worldSurface)
					{
						tile.liquid = 0;
					}
					else if (!WorldGen.remixWorldGen)
					{
						tile.lava(lava: true);
					}
					if (!flag || tile.active())
					{
						tile.ResetToType(396);
						postPlacementEffect = PostPlacementEffect.Smooth;
					}
				}
				else if (num7 > 0.7 || !flag)
				{
					if (!WorldGen.remixWorldGen || !((double)j > Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2)))
					{
						tile.wall = 216;
						tile.liquid = 0;
					}
					if (!flag || tile.active())
					{
						tile.ResetToType(type);
						postPlacementEffect = PostPlacementEffect.Smooth;
					}
				}
				else if (num7 > 0.25)
				{
					FastRandom fastRandom2 = fastRandom.WithModifier(num4, num5);
					double num8 = (num7 - 0.25) / 0.45;
					if (fastRandom2.NextDouble() < num8)
					{
						if (!WorldGen.remixWorldGen || !((double)j > Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2)))
						{
							tile.wall = 187;
						}
						if ((double)j < Main.worldSurface)
						{
							tile.liquid = 0;
						}
						else if (!WorldGen.remixWorldGen)
						{
							tile.lava(lava: true);
						}
						if (tile.active())
						{
							tile.ResetToType(type);
							postPlacementEffect = PostPlacementEffect.Smooth;
						}
					}
				}
				else
				{
					flag2 = false;
				}
				if (flag2)
				{
					WorldGen.UpdateDesertHiveBounds(i, j);
				}
				postEffectMap[i - area.X + postEffectMapOffset.X, j - area.Y + postEffectMapOffset.Y] = postPlacementEffect;
				if (WorldGen.remixWorldGen)
				{
					Main.tile[i, j].liquid = liquid;
				}
			}
		}
	}

	private static void AddTileVariance(DesertDescription description)
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		for (int i = -20; i < description.Hive.Width + 20; i++)
		{
			for (int j = -20; j < description.Hive.Height + 20; j++)
			{
				int num = i + description.Hive.X;
				int num2 = j + description.Hive.Y;
				if (WorldGen.InWorld(num, num2, 1))
				{
					Tile tile = Main.tile[num, num2];
					Tile testTile = Main.tile[num, num2 + 1];
					Tile testTile2 = Main.tile[num, num2 + 2];
					if (tile.type == 53 && (!WorldGen.SolidTile(testTile) || !WorldGen.SolidTile(testTile2)))
					{
						tile.type = 397;
					}
				}
			}
		}
		for (int k = -20; k < description.Hive.Width + 20; k++)
		{
			for (int l = -20; l < description.Hive.Height + 20; l++)
			{
				int num3 = k + description.Hive.X;
				int num4 = l + description.Hive.Y;
				if (!WorldGen.InWorld(num3, num4, 1))
				{
					continue;
				}
				Tile tile2 = Main.tile[num3, num4];
				if (!tile2.active() || tile2.type != 396)
				{
					continue;
				}
				bool flag = true;
				for (int num5 = -1; num5 >= -3; num5--)
				{
					if (Main.tile[num3, num4 + num5].active())
					{
						flag = false;
						break;
					}
				}
				bool flag2 = true;
				for (int m = 1; m <= 3; m++)
				{
					if (Main.tile[num3, num4 + m].active())
					{
						flag2 = false;
						break;
					}
				}
				if (!WorldGen.remixWorldGen || !((double)num4 > Main.rockLayer + (double)WorldGen.genRand.Next(-1, 2)))
				{
					if (flag && WorldGen.genRand.Next(20) == 0)
					{
						WorldGen.PlaceTile(num3, num4 - 1, 485, mute: true, forced: true, -1, WorldGen.genRand.Next(4));
					}
					else if (flag && WorldGen.genRand.Next(5) == 0)
					{
						WorldGen.PlaceTile(num3, num4 - 1, 484, mute: true, forced: true);
					}
					else if ((flag ^ flag2) && WorldGen.genRand.Next(5) == 0)
					{
						WorldGen.PlaceTile(num3, num4 + ((!flag) ? 1 : (-1)), 165, mute: true, forced: true);
					}
					else if (flag && WorldGen.genRand.Next(5) == 0)
					{
						WorldGen.PlaceTile(num3, num4 - 1, 187, mute: true, forced: true, -1, 29 + WorldGen.genRand.Next(6));
					}
				}
			}
		}
	}
}
