using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class CorruptionPitBiome : MicroBiome
{
	public static bool[] ValidTiles = TileID.Sets.Factory.CreateBoolSet(true, 21, 31, 26);

	public override bool Place(Point origin, StructureMap structures)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		if (WorldGen.SolidTile(origin.X, origin.Y) && GenBase._tiles[origin.X, origin.Y].wall == 3)
		{
			return false;
		}
		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(100), new Conditions.IsSolid()), out origin))
		{
			return false;
		}
		if (!WorldUtils.Find(new Point(origin.X - 4, origin.Y), Searches.Chain(new Searches.Down(5), new Conditions.IsTile(25).AreaAnd(8, 1)), out var _))
		{
			return false;
		}
		ShapeData shapeData = new ShapeData();
		ShapeData shapeData2 = new ShapeData();
		ShapeData shapeData3 = new ShapeData();
		for (int i = 0; i < 6; i++)
		{
			WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(10, 12) + i), Actions.Chain(new Modifiers.Offset(0, 5 * i + 5), new Modifiers.Blotches(3).Output(shapeData)));
		}
		for (int j = 0; j < 6; j++)
		{
			WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(5, 7) + j), Actions.Chain(new Modifiers.Offset(0, 2 * j + 18), new Modifiers.Blotches(3).Output(shapeData2)));
		}
		for (int k = 0; k < 6; k++)
		{
			WorldUtils.Gen(origin, new Shapes.Circle(GenBase._random.Next(4, 6) + k / 2), Actions.Chain(new Modifiers.Offset(0, (int)(7.5 * (double)k) - 10), new Modifiers.Blotches(3).Output(shapeData3)));
		}
		ShapeData shapeData4 = new ShapeData(shapeData2);
		shapeData2.Subtract(shapeData3, origin, origin);
		shapeData4.Subtract(shapeData2, origin, origin);
		Rectangle bounds = ShapeData.GetBounds(origin, shapeData, shapeData3);
		if (!structures.CanPlace(bounds, ValidTiles, 2))
		{
			return false;
		}
		WorldUtils.Gen(origin, new ModShapes.All(shapeData), Actions.Chain(new Actions.SetTile(25, setSelfFrames: true), new Actions.PlaceWall(3)));
		WorldUtils.Gen(origin, new ModShapes.All(shapeData2), new Actions.SetTile(0, setSelfFrames: true));
		WorldUtils.Gen(origin, new ModShapes.All(shapeData3), new Actions.ClearTile(frameNeighbors: true));
		WorldUtils.Gen(origin, new ModShapes.All(shapeData2), Actions.Chain(new Modifiers.IsTouchingAir(useDiagonals: true), new Modifiers.NotTouching(false, 25), new Actions.SetTile(23, setSelfFrames: true)));
		WorldUtils.Gen(origin, new ModShapes.All(shapeData4), new Actions.PlaceWall(69));
		structures.AddProtectedStructure(bounds, 2);
		return true;
	}
}
