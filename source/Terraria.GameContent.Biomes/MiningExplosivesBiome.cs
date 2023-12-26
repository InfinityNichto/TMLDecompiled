using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class MiningExplosivesBiome : MicroBiome
{
	public override bool Place(Point origin, StructureMap structures)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		if (WorldGen.SolidTile(origin.X, origin.Y))
		{
			return false;
		}
		if (Main.tile[origin.X, origin.Y].wall == 216 || Main.tile[origin.X, origin.Y].wall == 187)
		{
			return false;
		}
		ushort type = Utils.SelectRandom<ushort>(GenBase._random, (ushort)((GenVars.goldBar == 19) ? 8u : 169u), (ushort)((GenVars.silverBar == 21) ? 9u : 168u), (ushort)((GenVars.ironBar == 22) ? 6u : 167u), (ushort)((GenVars.copperBar == 20) ? 7u : 166u));
		double num = GenBase._random.NextDouble() * 2.0 - 1.0;
		if (!WorldUtils.Find(origin, Searches.Chain((num > 0.0) ? ((GenSearch)new Searches.Right(40)) : ((GenSearch)new Searches.Left(40)), new Conditions.IsSolid()), out origin))
		{
			return false;
		}
		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(80), new Conditions.IsSolid()), out origin))
		{
			return false;
		}
		ShapeData shapeData = new ShapeData();
		Ref<int> @ref = new Ref<int>(0);
		Ref<int> ref2 = new Ref<int>(0);
		WorldUtils.Gen(origin, new ShapeRunner(10.0, 20, new Vector2D(num, 1.0)).Output(shapeData), Actions.Chain(new Modifiers.Blotches(), new Actions.Scanner(@ref), new Modifiers.IsSolid(), new Actions.Scanner(ref2)));
		if (ref2.Value < @ref.Value / 2)
		{
			return false;
		}
		Rectangle area = default(Rectangle);
		((Rectangle)(ref area))._002Ector(origin.X - 15, origin.Y - 10, 30, 20);
		if (!structures.CanPlace(area))
		{
			return false;
		}
		WorldUtils.Gen(origin, new ModShapes.All(shapeData), new Actions.SetTile(type, setSelfFrames: true));
		WorldUtils.Gen(new Point(origin.X - (int)(num * -5.0), origin.Y - 5), new Shapes.Circle(5), Actions.Chain(new Modifiers.Blotches(), new Actions.ClearTile(frameNeighbors: true)));
		Point result;
		int num3 = 1 & (WorldUtils.Find(new Point(origin.X - ((num > 0.0) ? 3 : (-3)), origin.Y - 3), Searches.Chain(new Searches.Down(10), new Conditions.IsSolid()), out result) ? 1 : 0);
		int num2 = ((GenBase._random.Next(4) == 0) ? 3 : 7);
		if ((num3 & (WorldUtils.Find(new Point(origin.X - ((num > 0.0) ? (-num2) : num2), origin.Y - 3), Searches.Chain(new Searches.Down(10), new Conditions.IsSolid()), out var result2) ? 1 : 0)) == 0)
		{
			return false;
		}
		result.Y--;
		result2.Y--;
		Tile tile = GenBase._tiles[result.X, result.Y + 1];
		tile.slope(0);
		tile.halfBrick(halfBrick: false);
		for (int i = -1; i <= 1; i++)
		{
			WorldUtils.ClearTile(result2.X + i, result2.Y);
			Tile tile2 = GenBase._tiles[result2.X + i, result2.Y + 1];
			if (!WorldGen.SolidOrSlopedTile(tile2))
			{
				tile2.ResetToType(1);
				tile2.active(active: true);
			}
			tile2.slope(0);
			tile2.halfBrick(halfBrick: false);
			WorldUtils.TileFrame(result2.X + i, result2.Y + 1, frameNeighbors: true);
		}
		WorldGen.PlaceTile(result.X, result.Y, 141);
		WorldGen.PlaceTile(result2.X, result2.Y, 411, mute: true, forced: true);
		WorldUtils.WireLine(result, result2);
		structures.AddProtectedStructure(area, 5);
		return true;
	}
}
