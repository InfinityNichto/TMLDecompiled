using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse;

public class WoodHouseBuilder : HouseBuilder
{
	public WoodHouseBuilder(IEnumerable<Rectangle> rooms)
		: base(HouseType.Wood, rooms)
	{
		base.TileType = 30;
		base.WallType = 27;
		base.BeamType = 124;
		if (Main.tenthAnniversaryWorld)
		{
			if (Main.getGoodWorld)
			{
				if (WorldGen.genRand.Next(7) == 0)
				{
					base.TileType = 160;
					base.WallType = 44;
				}
			}
			else if (WorldGen.genRand.Next(2) == 0)
			{
				base.TileType = 160;
				base.WallType = 44;
			}
		}
		base.PlatformStyle = 0;
		base.DoorStyle = 0;
		base.TableStyle = 0;
		base.WorkbenchStyle = 0;
		base.PianoStyle = 0;
		base.BookcaseStyle = 0;
		base.ChairStyle = 0;
		base.ChestStyle = 1;
	}

	protected override void AgeRoom(Rectangle room)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < room.Width * room.Height / 16; i++)
		{
			int num = WorldGen.genRand.Next(1, room.Width - 1) + room.X;
			int y = WorldGen.genRand.Next(1, room.Height - 1) + room.Y;
			WorldUtils.Gen(new Point(num, y), new Shapes.Rectangle(2, 2), Actions.Chain(new Modifiers.Dither(), new Modifiers.Blotches(2, 2), new Modifiers.IsEmpty(), new Actions.SetTile(51, setSelfFrames: true)));
		}
		WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.85), new Modifiers.Blotches(), new Modifiers.OnlyWalls(base.WallType), new Modifiers.SkipTiles(SkipTilesDuringWallAging), ((double)room.Y > Main.worldSurface) ? ((GenAction)new Actions.ClearWall(frameNeighbors: true)) : ((GenAction)new Actions.PlaceWall(2))));
		WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.Dither(0.95), new Modifiers.OnlyTiles(30, 321, 158), new Actions.ClearTile(frameNeighbors: true)));
	}

	public override void Place(HouseBuilderContext context, StructureMap structures)
	{
		base.Place(context, structures);
		RainbowifyOnTenthAnniversaryWorlds();
	}

	private void RainbowifyOnTenthAnniversaryWorlds()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.tenthAnniversaryWorld || (base.TileType == 160 && WorldGen.genRand.Next(2) == 0))
		{
			return;
		}
		foreach (Rectangle room in base.Rooms)
		{
			WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), new Actions.SetTileAndWallRainbowPaint());
		}
	}
}
