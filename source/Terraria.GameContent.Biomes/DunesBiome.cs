using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using ReLogic.Utilities;
using Terraria.GameContent.Biomes.Desert;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class DunesBiome : MicroBiome
{
	private class DunesDescription
	{
		public bool IsValid { get; private set; }

		public SurfaceMap Surface { get; private set; }

		public Rectangle Area { get; private set; }

		public WindDirection WindDirection { get; private set; }

		private DunesDescription()
		{
		}

		public static DunesDescription CreateFromPlacement(Point origin, int width, int height)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			Rectangle area = default(Rectangle);
			((Rectangle)(ref area))._002Ector(origin.X - width / 2, origin.Y - height / 2, width, height);
			return new DunesDescription
			{
				Area = area,
				IsValid = true,
				Surface = SurfaceMap.FromArea(((Rectangle)(ref area)).Left - 20, area.Width + 40),
				WindDirection = ((WorldGen.genRand.Next(2) != 0) ? WindDirection.Right : WindDirection.Left)
			};
		}
	}

	private enum WindDirection
	{
		Left,
		Right
	}

	[JsonProperty("SingleDunesWidth")]
	private WorldGenRange _singleDunesWidth = WorldGenRange.Empty;

	[JsonProperty("HeightScale")]
	private double _heightScale = 1.0;

	public int MaximumWidth => _singleDunesWidth.ScaledMaximum * 2;

	public override bool Place(Point origin, StructureMap structures)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		int height = (int)((double)GenBase._random.Next(60, 100) * _heightScale);
		int height2 = (int)((double)GenBase._random.Next(60, 100) * _heightScale);
		int random = _singleDunesWidth.GetRandom(GenBase._random);
		int random2 = _singleDunesWidth.GetRandom(GenBase._random);
		DunesDescription description = DunesDescription.CreateFromPlacement(new Point(origin.X - random / 2 + 30, origin.Y), random, height);
		DunesDescription description2 = DunesDescription.CreateFromPlacement(new Point(origin.X + random2 / 2 - 30, origin.Y), random2, height2);
		PlaceSingle(description, structures);
		PlaceSingle(description2, structures);
		return true;
	}

	private void PlaceSingle(DunesDescription description, StructureMap structures)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		int num = GenBase._random.Next(3) + 8;
		Rectangle area;
		for (int i = 0; i < num - 1; i++)
		{
			int num2 = (int)(2.0 / (double)num * (double)description.Area.Width);
			double num8 = (double)i / (double)num * (double)description.Area.Width;
			area = description.Area;
			int num3 = (int)(num8 + (double)((Rectangle)(ref area)).Left) + num2 * 2 / 5;
			num3 += GenBase._random.Next(-5, 6);
			double num4 = (double)i / (double)(num - 2);
			double num5 = 1.0 - Math.Abs(num4 - 0.5) * 2.0;
			PlaceHill(num3 - num2 / 2, num3 + num2 / 2, (num5 * 0.3 + 0.2) * _heightScale, description);
		}
		int num6 = GenBase._random.Next(2) + 1;
		for (int j = 0; j < num6; j++)
		{
			int num7 = description.Area.Width / 2;
			area = description.Area;
			int x = ((Rectangle)(ref area)).Center.X;
			x += GenBase._random.Next(-10, 11);
			PlaceHill(x - num7 / 2, x + num7 / 2, 0.8 * _heightScale, description);
		}
		structures.AddStructure(description.Area, 20);
	}

	private static void PlaceHill(int startX, int endX, double scale, DunesDescription description)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		Point startPoint = default(Point);
		((Point)(ref startPoint))._002Ector(startX, (int)description.Surface[startX]);
		Point endPoint = default(Point);
		((Point)(ref endPoint))._002Ector(endX, (int)description.Surface[endX]);
		Point point = default(Point);
		((Point)(ref point))._002Ector((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2 - (int)(35.0 * scale));
		int num = (endPoint.X - point.X) / 4;
		int minValue = (endPoint.X - point.X) / 16;
		if (description.WindDirection == WindDirection.Left)
		{
			point.X -= WorldGen.genRand.Next(minValue, num + 1);
		}
		else
		{
			point.X += WorldGen.genRand.Next(minValue, num + 1);
		}
		Point point2 = default(Point);
		((Point)(ref point2))._002Ector(0, (int)(scale * 12.0));
		Point point3 = default(Point);
		((Point)(ref point3))._002Ector(point2.X / -2, point2.Y / -2);
		PlaceCurvedLine(startPoint, point, (description.WindDirection != 0) ? point3 : point2, description);
		PlaceCurvedLine(point, endPoint, (description.WindDirection == WindDirection.Left) ? point3 : point2, description);
	}

	private static void PlaceCurvedLine(Point startPoint, Point endPoint, Point anchorOffset, DunesDescription description)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		Point p = default(Point);
		((Point)(ref p))._002Ector((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);
		p.X += anchorOffset.X;
		p.Y += anchorOffset.Y;
		Vector2D value = startPoint.ToVector2D();
		Vector2D value2 = endPoint.ToVector2D();
		Vector2D vector2D = p.ToVector2D();
		double num = 0.5 / (value2.X - value.X);
		Point point = default(Point);
		((Point)(ref point))._002Ector(-1, -1);
		for (double num2 = 0.0; num2 <= 1.0; num2 += num)
		{
			Vector2D value4 = Vector2D.Lerp(value, vector2D, num2);
			Vector2D value3 = Vector2D.Lerp(vector2D, value2, num2);
			Point point2 = Vector2D.Lerp(value4, value3, num2).ToPoint();
			if (point2 == point)
			{
				continue;
			}
			point = point2;
			int num5 = description.Area.Width / 2;
			int x = point2.X;
			Rectangle area = description.Area;
			int num3 = num5 - Math.Abs(x - ((Rectangle)(ref area)).Center.X);
			int num4 = description.Surface[point2.X] + (int)(Math.Sqrt(num3) * 3.0);
			for (int i = point2.Y - 10; i < point2.Y; i++)
			{
				if (GenBase._tiles[point2.X, i].active() && GenBase._tiles[point2.X, i].type != 53)
				{
					GenBase._tiles[point2.X, i].ClearEverything();
				}
			}
			for (int j = point2.Y; j < num4; j++)
			{
				GenBase._tiles[point2.X, j].ResetToType(53);
				Tile.SmoothSlope(point2.X, j);
			}
		}
	}
}
