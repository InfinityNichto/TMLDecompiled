using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert;

public static class ChambersEntrance
{
	private struct PathConnection
	{
		public readonly Vector2D Position;

		public readonly double Direction;

		public PathConnection(Point position, int direction)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			Position = new Vector2D(position.X, position.Y);
			Direction = direction;
		}
	}

	public static void Place(DesertDescription description)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Rectangle desert = description.Desert;
		int num = ((Rectangle)(ref desert)).Center.X + WorldGen.genRand.Next(-40, 41);
		Point position = default(Point);
		((Point)(ref position))._002Ector(num, (int)description.Surface[num]);
		PlaceAt(description, position);
	}

	private static void PlaceAt(DesertDescription description, Point position)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		ShapeData shapeData = new ShapeData();
		Point origin = default(Point);
		((Point)(ref origin))._002Ector(position.X, position.Y + 2);
		WorldUtils.Gen(origin, new Shapes.Circle(24, 12), Actions.Chain(new Modifiers.Blotches(), new Actions.SetTile(53).Output(shapeData)));
		UnifiedRandom genRand = WorldGen.genRand;
		ShapeData data = new ShapeData();
		Rectangle hive = description.Hive;
		int num = ((Rectangle)(ref hive)).Top - position.Y;
		int num2 = ((genRand.Next(2) != 0) ? 1 : (-1));
		List<PathConnection> list = new List<PathConnection>
		{
			new PathConnection(new Point(position.X + -num2 * 26, position.Y - 8), num2)
		};
		int num3 = genRand.Next(2, 4);
		for (int i = 0; i < num3; i++)
		{
			int num4 = (int)((double)(i + 1) / (double)num3 * (double)num) + genRand.Next(-8, 9);
			int num5 = num2 * genRand.Next(20, 41);
			int num6 = genRand.Next(18, 29);
			WorldUtils.Gen(position, new Shapes.Circle(num6 / 2, 3), Actions.Chain(new Modifiers.Offset(num5, num4), new Modifiers.Blotches(), new Actions.Clear().Output(data), new Actions.PlaceWall(187)));
			list.Add(new PathConnection(new Point(num5 + num6 / 2 * -num2 + position.X, num4 + position.Y), -num2));
			num2 *= -1;
		}
		WorldUtils.Gen(position, new ModShapes.OuterOutline(data), Actions.Chain(new Modifiers.Expand(1), new Modifiers.OnlyTiles(53), new Actions.SetTile(397), new Actions.PlaceWall(187)));
		GenShapeActionPair pair = new GenShapeActionPair(new Shapes.Rectangle(2, 4), Actions.Chain(new Modifiers.IsSolid(), new Modifiers.Blotches(), new Actions.Clear(), new Modifiers.Expand(1), new Actions.PlaceWall(187), new Modifiers.OnlyTiles(53), new Actions.SetTile(397)));
		for (int j = 1; j < list.Count; j++)
		{
			PathConnection pathConnection = list[j - 1];
			PathConnection pathConnection2 = list[j];
			double num7 = Math.Abs(pathConnection2.Position.X - pathConnection.Position.X) * 1.5;
			for (double num8 = 0.0; num8 <= 1.0; num8 += 0.02)
			{
				Vector2D value4 = new Vector2D(pathConnection.Position.X + pathConnection.Direction * num7 * num8, pathConnection.Position.Y);
				Vector2D value2 = new Vector2D(pathConnection2.Position.X + pathConnection2.Direction * num7 * (1.0 - num8), pathConnection2.Position.Y);
				Vector2D vector2D = Vector2D.Lerp(pathConnection.Position, pathConnection2.Position, num8);
				Vector2D value5 = Vector2D.Lerp(value4, vector2D, num8);
				Vector2D value3 = Vector2D.Lerp(vector2D, value2, num8);
				WorldUtils.Gen(Vector2D.Lerp(value5, value3, num8).ToPoint(), pair);
			}
		}
		WorldUtils.Gen(origin, new Shapes.Rectangle(new Rectangle(-29, -12, 58, 12)), Actions.Chain(new Modifiers.NotInShape(shapeData), new Modifiers.Expand(1), new Actions.PlaceWall(0)));
	}
}
