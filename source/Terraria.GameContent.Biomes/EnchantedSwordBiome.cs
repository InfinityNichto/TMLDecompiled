using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class EnchantedSwordBiome : MicroBiome
{
	[JsonProperty("ChanceOfEntrance")]
	private double _chanceOfEntrance;

	[JsonProperty("ChanceOfRealSword")]
	private double _chanceOfRealSword;

	public override bool Place(Point origin, StructureMap structures)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
		WorldUtils.Gen(new Point(origin.X - 25, origin.Y - 25), new Shapes.Rectangle(50, 50), new Actions.TileScanner(0, 1).Output(dictionary));
		if (dictionary[0] + dictionary[1] < 1250)
		{
			return false;
		}
		Point result;
		bool flag = WorldUtils.Find(origin, Searches.Chain(new Searches.Up(1000), new Conditions.IsSolid().AreaOr(1, 50).Not()), out result);
		if (WorldUtils.Find(origin, Searches.Chain(new Searches.Up(origin.Y - result.Y), new Conditions.IsTile(53)), out var _))
		{
			return false;
		}
		if (!flag)
		{
			return false;
		}
		result.Y += 50;
		ShapeData shapeData = new ShapeData();
		ShapeData shapeData2 = new ShapeData();
		Point point = default(Point);
		((Point)(ref point))._002Ector(origin.X, origin.Y + 20);
		Point point2 = default(Point);
		((Point)(ref point2))._002Ector(origin.X, origin.Y + 30);
		bool[] array = new bool[TileID.Sets.GeneralPlacementTiles.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = TileID.Sets.GeneralPlacementTiles[i];
		}
		array[21] = false;
		array[467] = false;
		double num = 0.8 + GenBase._random.NextDouble() * 0.5;
		if (!structures.CanPlace(new Rectangle(point.X - (int)(20.0 * num), point.Y - 20, (int)(40.0 * num), 40), array))
		{
			return false;
		}
		if (!structures.CanPlace(new Rectangle(origin.X, result.Y + 10, 1, origin.Y - result.Y - 9), array, 2))
		{
			return false;
		}
		WorldUtils.Gen(point, new Shapes.Slime(20, num, 1.0), Actions.Chain(new Modifiers.Blotches(2, 0.4), new Actions.ClearTile(frameNeighbors: true).Output(shapeData)));
		WorldUtils.Gen(point2, new Shapes.Mound(14, 14), Actions.Chain(new Modifiers.Blotches(2, 1, 0.8), new Actions.SetTile(0), new Actions.SetFrames(frameNeighbors: true).Output(shapeData2)));
		shapeData.Subtract(shapeData2, point, point2);
		WorldUtils.Gen(point, new ModShapes.InnerOutline(shapeData), Actions.Chain(new Actions.SetTile(2), new Actions.SetFrames(frameNeighbors: true)));
		WorldUtils.Gen(point, new ModShapes.All(shapeData), Actions.Chain(new Modifiers.RectangleMask(-40, 40, 0, 40), new Modifiers.IsEmpty(), new Actions.SetLiquid()));
		WorldUtils.Gen(point, new ModShapes.All(shapeData), Actions.Chain(new Actions.PlaceWall(68), new Modifiers.OnlyTiles(2), new Modifiers.Offset(0, 1), new ActionVines(3, 5, 382)));
		if (GenBase._random.NextDouble() <= _chanceOfEntrance || WorldGen.tenthAnniversaryWorldGen)
		{
			ShapeData data = new ShapeData();
			WorldUtils.Gen(new Point(origin.X, result.Y + 10), new Shapes.Rectangle(1, origin.Y - result.Y - 9), Actions.Chain(new Modifiers.Blotches(2, 0.2), new Modifiers.SkipTiles(191, 192), new Actions.ClearTile().Output(data), new Modifiers.Expand(1), new Modifiers.OnlyTiles(53), new Actions.SetTile(397).Output(data)));
			WorldUtils.Gen(new Point(origin.X, result.Y + 10), new ModShapes.All(data), new Actions.SetFrames(frameNeighbors: true));
		}
		if (GenBase._random.NextDouble() <= _chanceOfRealSword)
		{
			WorldGen.PlaceTile(point2.X, point2.Y - 15, 187, mute: true, forced: false, -1, 17);
		}
		else
		{
			WorldGen.PlaceTile(point2.X, point2.Y - 15, 186, mute: true, forced: false, -1, 15);
		}
		WorldUtils.Gen(point2, new ModShapes.All(shapeData2), Actions.Chain(new Modifiers.Offset(0, -1), new Modifiers.OnlyTiles(2), new Modifiers.Offset(0, -1), new ActionGrass()));
		structures.AddProtectedStructure(new Rectangle(point.X - (int)(20.0 * num), point.Y - 20, (int)(40.0 * num), 40), 10);
		return true;
	}
}
