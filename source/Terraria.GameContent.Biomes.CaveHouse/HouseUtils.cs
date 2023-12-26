using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes.CaveHouse;

public static class HouseUtils
{
	internal static bool[] BlacklistedTiles = TileID.Sets.Factory.CreateBoolSet(true, 225, 41, 43, 44, 226, 203, 112, 25, 151, 21, 467);

	internal static bool[] BeelistedTiles = TileID.Sets.Factory.CreateBoolSet(true, 41, 43, 44, 226, 203, 112, 25, 151, 21, 467);

	public static HouseBuilder CreateBuilder(Point origin, StructureMap structures)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		List<Rectangle> list = CreateRooms(origin);
		if (list.Count == 0 || !AreRoomLocationsValid(list))
		{
			return HouseBuilder.Invalid;
		}
		HouseType houseType = GetHouseType(list);
		if (!AreRoomsValid(list, structures, houseType))
		{
			return HouseBuilder.Invalid;
		}
		return houseType switch
		{
			HouseType.Wood => new WoodHouseBuilder(list), 
			HouseType.Desert => new DesertHouseBuilder(list), 
			HouseType.Granite => new GraniteHouseBuilder(list), 
			HouseType.Ice => new IceHouseBuilder(list), 
			HouseType.Jungle => new JungleHouseBuilder(list), 
			HouseType.Marble => new MarbleHouseBuilder(list), 
			HouseType.Mushroom => new MushroomHouseBuilder(list), 
			_ => new WoodHouseBuilder(list), 
		};
	}

	private static List<Rectangle> CreateRooms(Point origin)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(200), new Conditions.IsSolid()), out var result) || result == origin)
		{
			return new List<Rectangle>();
		}
		Rectangle item = FindRoom(result);
		Rectangle rectangle = FindRoom(new Point(((Rectangle)(ref item)).Center.X, item.Y + 1));
		Rectangle rectangle2 = FindRoom(new Point(((Rectangle)(ref item)).Center.X, item.Y + item.Height + 10));
		rectangle2.Y = item.Y + item.Height - 1;
		double roomSolidPrecentage = GetRoomSolidPrecentage(rectangle);
		double roomSolidPrecentage2 = GetRoomSolidPrecentage(rectangle2);
		item.Y += 3;
		rectangle.Y += 3;
		rectangle2.Y += 3;
		List<Rectangle> list = new List<Rectangle>();
		if (WorldGen.genRand.NextDouble() > roomSolidPrecentage + 0.2)
		{
			list.Add(rectangle);
		}
		list.Add(item);
		if (WorldGen.genRand.NextDouble() > roomSolidPrecentage2 + 0.2)
		{
			list.Add(rectangle2);
		}
		return list;
	}

	private static Rectangle FindRoom(Point origin)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		Point result;
		bool flag = WorldUtils.Find(origin, Searches.Chain(new Searches.Left(25), new Conditions.IsSolid()), out result);
		Point result2;
		bool num = WorldUtils.Find(origin, Searches.Chain(new Searches.Right(25), new Conditions.IsSolid()), out result2);
		if (!flag)
		{
			((Point)(ref result))._002Ector(origin.X - 25, origin.Y);
		}
		if (!num)
		{
			((Point)(ref result2))._002Ector(origin.X + 25, origin.Y);
		}
		Rectangle result3 = default(Rectangle);
		((Rectangle)(ref result3))._002Ector(origin.X, origin.Y, 0, 0);
		if (origin.X - result.X > result2.X - origin.X)
		{
			result3.X = result.X;
			result3.Width = Utils.Clamp(result2.X - result.X, 15, 30);
		}
		else
		{
			result3.Width = Utils.Clamp(result2.X - result.X, 15, 30);
			result3.X = result2.X - result3.Width;
		}
		Point result4;
		bool flag2 = WorldUtils.Find(result, Searches.Chain(new Searches.Up(10), new Conditions.IsSolid()), out result4);
		Point result5;
		bool num2 = WorldUtils.Find(result2, Searches.Chain(new Searches.Up(10), new Conditions.IsSolid()), out result5);
		if (!flag2)
		{
			((Point)(ref result4))._002Ector(origin.X, origin.Y - 10);
		}
		if (!num2)
		{
			((Point)(ref result5))._002Ector(origin.X, origin.Y - 10);
		}
		result3.Height = Utils.Clamp(Math.Max(origin.Y - result4.Y, origin.Y - result5.Y), 8, 12);
		result3.Y -= result3.Height;
		return result3;
	}

	private static double GetRoomSolidPrecentage(Rectangle room)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		double num = room.Width * room.Height;
		Ref<int> @ref = new Ref<int>(0);
		WorldUtils.Gen(new Point(room.X, room.Y), new Shapes.Rectangle(room.Width, room.Height), Actions.Chain(new Modifiers.IsSolid(), new Actions.Count(@ref)));
		return (double)@ref.Value / num;
	}

	private static int SortBiomeResults(Tuple<HouseType, int> item1, Tuple<HouseType, int> item2)
	{
		return item2.Item2.CompareTo(item1.Item2);
	}

	private static bool AreRoomLocationsValid(IEnumerable<Rectangle> rooms)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		foreach (Rectangle room in rooms)
		{
			if (room.Y + room.Height > Main.maxTilesY - 220)
			{
				return false;
			}
		}
		return true;
	}

	private static HouseType GetHouseType(IEnumerable<Rectangle> rooms)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<ushort, int> dictionary = new Dictionary<ushort, int>();
		foreach (Rectangle room in rooms)
		{
			WorldUtils.Gen(new Point(room.X - 10, room.Y - 10), new Shapes.Rectangle(room.Width + 20, room.Height + 20), new Actions.TileScanner(0, 59, 147, 1, 161, 53, 396, 397, 368, 367, 60, 70).Output(dictionary));
		}
		List<Tuple<HouseType, int>> list = new List<Tuple<HouseType, int>>();
		list.Add(Tuple.Create(HouseType.Wood, dictionary[0] + dictionary[1]));
		list.Add(Tuple.Create(HouseType.Jungle, dictionary[59] + dictionary[60] * 10));
		list.Add(Tuple.Create(HouseType.Mushroom, dictionary[59] + dictionary[70] * 10));
		list.Add(Tuple.Create(HouseType.Ice, dictionary[147] + dictionary[161]));
		list.Add(Tuple.Create(HouseType.Desert, dictionary[397] + dictionary[396] + dictionary[53]));
		list.Add(Tuple.Create(HouseType.Granite, dictionary[368]));
		list.Add(Tuple.Create(HouseType.Marble, dictionary[367]));
		list.Sort(SortBiomeResults);
		return list[0].Item1;
	}

	private static bool AreRoomsValid(IEnumerable<Rectangle> rooms, StructureMap structures, HouseType style)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		foreach (Rectangle room in rooms)
		{
			if (style != HouseType.Granite && WorldUtils.Find(new Point(room.X - 2, room.Y - 2), Searches.Chain(new Searches.Rectangle(room.Width + 4, room.Height + 4).RequireAll(mode: false), new Conditions.HasLava()), out var _))
			{
				return false;
			}
			if (WorldGen.notTheBees)
			{
				if (!structures.CanPlace(room, BeelistedTiles, 5))
				{
					return false;
				}
			}
			else if (!structures.CanPlace(room, BlacklistedTiles, 5))
			{
				return false;
			}
		}
		return true;
	}
}
