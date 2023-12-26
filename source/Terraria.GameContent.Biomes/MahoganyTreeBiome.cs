using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class MahoganyTreeBiome : MicroBiome
{
	public override bool Place(Point origin, StructureMap structures)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldUtils.Find(new Point(origin.X - 3, origin.Y), Searches.Chain(new Searches.Down(200), new Conditions.IsSolid().AreaAnd(6, 1)), out var result))
		{
			return false;
		}
		if (!WorldUtils.Find(new Point(result.X, result.Y - 5), Searches.Chain(new Searches.Up(120), new Conditions.IsSolid().AreaOr(6, 1)), out var result2) || result.Y - 5 - result2.Y > 60)
		{
			return false;
		}
		if (result.Y - result2.Y < 30)
		{
			return false;
		}
		if (!structures.CanPlace(new Rectangle(result.X - 30, result.Y - 60, 60, 90)))
		{
			return false;
		}
		if (!WorldGen.drunkWorldGen || WorldGen.genRand.Next(50) > 0)
		{
			Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
			WorldUtils.Gen(new Point(result.X - 25, result.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(0, 59, 60, 147, 1).Output(dictionary));
			int num = dictionary[0] + dictionary[1];
			int num7 = dictionary[59] + dictionary[60];
			if (dictionary[147] > num7 || num > num7 || num7 < 50)
			{
				return false;
			}
		}
		int num8 = (result.Y - result2.Y - 9) / 5;
		int num9 = num8 * 5;
		int num10 = 0;
		double num11 = GenBase._random.NextDouble() + 1.0;
		double num12 = GenBase._random.NextDouble() + 2.0;
		if (GenBase._random.Next(2) == 0)
		{
			num12 = 0.0 - num12;
		}
		for (int i = 0; i < num8; i++)
		{
			int num13 = (int)(Math.Sin((double)(i + 1) / 12.0 * num11 * 3.1415927410125732) * num12);
			int num14 = ((num13 < num10) ? (num13 - num10) : 0);
			WorldUtils.Gen(new Point(result.X + num10 + num14, result.Y - (i + 1) * 5), new Shapes.Rectangle(6 + Math.Abs(num13 - num10), 7), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.RemoveWall(), new Actions.SetTile(383), new Actions.SetFrames()));
			WorldUtils.Gen(new Point(result.X + num10 + num14 + 2, result.Y - (i + 1) * 5), new Shapes.Rectangle(2 + Math.Abs(num13 - num10), 5), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.ClearTile(frameNeighbors: true), new Actions.PlaceWall(78)));
			WorldUtils.Gen(new Point(result.X + num10 + 2, result.Y - i * 5), new Shapes.Rectangle(2, 2), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.ClearTile(frameNeighbors: true), new Actions.PlaceWall(78)));
			num10 = num13;
		}
		int num2 = 6;
		if (num12 < 0.0)
		{
			num2 = 0;
		}
		List<Point> list = new List<Point>();
		for (int j = 0; j < 2; j++)
		{
			double num3 = ((double)j + 1.0) / 3.0;
			int num4 = num2 + (int)(Math.Sin((double)num8 * num3 / 12.0 * num11 * 3.1415927410125732) * num12);
			double num5 = GenBase._random.NextDouble() * 0.7853981852531433 - 0.7853981852531433 - 0.2;
			if (num2 == 0)
			{
				num5 -= 1.5707963705062866;
			}
			WorldUtils.Gen(new Point(result.X + num4, result.Y - (int)((double)(num8 * 5) * num3)), new ShapeBranch(num5, GenBase._random.Next(12, 16)).OutputEndpoints(list), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.SetTile(383), new Actions.SetFrames(frameNeighbors: true)));
			num2 = 6 - num2;
		}
		int num6 = (int)(Math.Sin((double)num8 / 12.0 * num11 * 3.1415927410125732) * num12);
		WorldUtils.Gen(new Point(result.X + 6 + num6, result.Y - num9), new ShapeBranch(-0.6853981852531433, GenBase._random.Next(16, 22)).OutputEndpoints(list), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.SetTile(383), new Actions.SetFrames(frameNeighbors: true)));
		WorldUtils.Gen(new Point(result.X + num6, result.Y - num9), new ShapeBranch(-2.45619455575943, GenBase._random.Next(16, 22)).OutputEndpoints(list), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.SetTile(383), new Actions.SetFrames(frameNeighbors: true)));
		foreach (Point item in list)
		{
			WorldUtils.Gen(item, new Shapes.Circle(4), Actions.Chain(new Modifiers.Blotches(4, 2), new Modifiers.SkipTiles(383, 21, 467, 226, 237), new Modifiers.SkipWalls(78, 87), new Actions.SetTile(384), new Actions.SetFrames(frameNeighbors: true)));
		}
		for (int k = 0; k < 4; k++)
		{
			double angle = (double)k / 3.0 * 2.0 + 0.57075;
			WorldUtils.Gen(result, new ShapeRoot(angle, GenBase._random.Next(40, 60)), Actions.Chain(new Modifiers.SkipTiles(21, 467, 226, 237), new Modifiers.SkipWalls(87), new Actions.SetTile(383, setSelfFrames: true)));
		}
		WorldGen.AddBuriedChest(result.X + 3, result.Y - 1, (GenBase._random.Next(4) != 0) ? WorldGen.GetNextJungleChestItem() : 0, notNearOtherChests: false, 10, trySlope: false, 0);
		structures.AddProtectedStructure(new Rectangle(result.X - 30, result.Y - 30, 60, 60));
		return true;
	}
}
