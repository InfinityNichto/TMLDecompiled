using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.Desert;

public static class AnthillEntrance
{
	public static void Place(DesertDescription description)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		int num = WorldGen.genRand.Next(2, 4);
		for (int i = 0; i < num; i++)
		{
			int holeRadius = WorldGen.genRand.Next(15, 18);
			int num2 = (int)((double)(i + 1) / (double)(num + 1) * (double)description.Surface.Width);
			int num3 = num2;
			Rectangle desert = description.Desert;
			num2 = num3 + ((Rectangle)(ref desert)).Left;
			int y = description.Surface[num2];
			PlaceAt(description, new Point(num2, y), holeRadius);
		}
	}

	private static void PlaceAt(DesertDescription description, Point position, int holeRadius)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		ShapeData data = new ShapeData();
		Point origin = default(Point);
		((Point)(ref origin))._002Ector(position.X, position.Y + 6);
		WorldUtils.Gen(origin, new Shapes.Tail(holeRadius * 2, new Vector2D(0.0, (double)(-holeRadius) * 1.5)), Actions.Chain(new Actions.SetTile(53).Output(data)));
		GenShapeActionPair genShapeActionPair = new GenShapeActionPair(new Shapes.Rectangle(1, 1), Actions.Chain(new Modifiers.Blotches(), new Modifiers.IsSolid(), new Actions.Clear(), new Actions.PlaceWall(187)));
		GenShapeActionPair genShapeActionPair2 = new GenShapeActionPair(new Shapes.Rectangle(1, 1), Actions.Chain(new Modifiers.IsSolid(), new Actions.Clear(), new Actions.PlaceWall(187)));
		GenShapeActionPair pair = new GenShapeActionPair(new Shapes.Circle(2, 3), Actions.Chain(new Modifiers.IsSolid(), new Actions.SetTile(397), new Actions.PlaceWall(187)));
		GenShapeActionPair pair2 = new GenShapeActionPair(new Shapes.Circle(holeRadius, 3), Actions.Chain(new Modifiers.SkipWalls(187), new Actions.SetTile(53)));
		GenShapeActionPair pair3 = new GenShapeActionPair(new Shapes.Circle(holeRadius - 2, 3), Actions.Chain(new Actions.PlaceWall(187)));
		int num = position.X;
		int i = position.Y - holeRadius - 3;
		while (true)
		{
			int num2 = i;
			Rectangle val = description.Hive;
			int top = ((Rectangle)(ref val)).Top;
			int y = position.Y;
			val = description.Desert;
			if (num2 >= top + (y - ((Rectangle)(ref val)).Top) * 2 + 12)
			{
				break;
			}
			WorldUtils.Gen(new Point(num, i), (i < position.Y) ? genShapeActionPair2 : genShapeActionPair);
			WorldUtils.Gen(new Point(num, i), pair);
			if (i % 3 == 0 && i >= position.Y)
			{
				num += WorldGen.genRand.Next(-1, 2);
				WorldUtils.Gen(new Point(num, i), genShapeActionPair);
				if (i >= position.Y + 5)
				{
					WorldUtils.Gen(new Point(num, i), pair2);
					WorldUtils.Gen(new Point(num, i), pair3);
				}
				WorldUtils.Gen(new Point(num, i), pair);
			}
			i++;
		}
		WorldUtils.Gen(new Point(origin.X, origin.Y - (int)((double)holeRadius * 1.5) + 3), new Shapes.Circle(holeRadius / 2, holeRadius / 3), Actions.Chain(Actions.Chain(new Actions.ClearTile(), new Modifiers.Expand(1), new Actions.PlaceWall(0))));
		WorldUtils.Gen(origin, new ModShapes.All(data), new Actions.Smooth());
	}
}
