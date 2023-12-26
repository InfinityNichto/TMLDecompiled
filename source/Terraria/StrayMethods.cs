using System;
using Microsoft.Xna.Framework;

namespace Terraria;

public class StrayMethods
{
	public static bool CountSandHorizontally(int i, int j, bool[] fittingTypes, int requiredTotalSpread = 4, int spreadInEachAxis = 5)
	{
		if (!WorldGen.InWorld(i, j, 2))
		{
			return false;
		}
		int num = 0;
		int num2 = 0;
		int num3 = i - 1;
		while (num < spreadInEachAxis && num3 > 0)
		{
			Tile tile = Main.tile[num3, j];
			if (tile.active() && fittingTypes[tile.type] && !WorldGen.SolidTileAllowBottomSlope(num3, j - 1))
			{
				num++;
			}
			else if (!tile.active())
			{
				break;
			}
			num3--;
		}
		num3 = i + 1;
		while (num2 < spreadInEachAxis && num3 < Main.maxTilesX - 1)
		{
			Tile tile2 = Main.tile[num3, j];
			if (tile2.active() && fittingTypes[tile2.type] && !WorldGen.SolidTileAllowBottomSlope(num3, j - 1))
			{
				num2++;
			}
			else if (!tile2.active())
			{
				break;
			}
			num3++;
		}
		return num + num2 + 1 >= requiredTotalSpread;
	}

	public static bool CanSpawnSandstormHostile(Vector2 position, int expandUp, int expandDown)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		bool result = true;
		Point point = position.ToTileCoordinates();
		for (int i = -1; i <= 1; i++)
		{
			Collision.ExpandVertically(point.X + i, point.Y, out var topY, out var bottomY, expandUp, expandDown);
			topY++;
			bottomY--;
			if (bottomY - topY < 20)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public static bool CanSpawnSandstormFriendly(Vector2 position, int expandUp, int expandDown)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		bool result = true;
		Point point = position.ToTileCoordinates();
		for (int i = -1; i <= 1; i++)
		{
			Collision.ExpandVertically(point.X + i, point.Y, out var topY, out var bottomY, expandUp, expandDown);
			topY++;
			bottomY--;
			if (bottomY - topY < 10)
			{
				result = false;
				break;
			}
		}
		return result;
	}

	public static void CheckArenaScore(Vector2 arenaCenter, out Point xLeftEnd, out Point xRightEnd, int walkerWidthInTiles = 5, int walkerHeightInTiles = 10)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		Point point = arenaCenter.ToTileCoordinates();
		xLeftEnd = (xRightEnd = point);
		Collision.ExpandVertically(point.X, point.Y, out var topY, out var bottomY, 0, 4);
		point.Y = bottomY;
		if (flag)
		{
			Dust.QuickDust(point, Color.Blue).scale = 5f;
		}
		SendWalker(point, walkerHeightInTiles, -1, out topY, out var lastIteratedFloorSpot, 120, flag);
		SendWalker(point, walkerHeightInTiles, 1, out topY, out var lastIteratedFloorSpot2, 120, flag);
		lastIteratedFloorSpot.X++;
		lastIteratedFloorSpot2.X--;
		if (flag)
		{
			Dust.QuickDustLine(lastIteratedFloorSpot.ToWorldCoordinates(), lastIteratedFloorSpot2.ToWorldCoordinates(), 50f, Color.Pink);
		}
		xLeftEnd = lastIteratedFloorSpot;
		xRightEnd = lastIteratedFloorSpot2;
	}

	public static void SendWalker(Point startFloorPosition, int height, int direction, out int distanceCoveredInTiles, out Point lastIteratedFloorSpot, int maxDistance = 100, bool showDebug = false)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		distanceCoveredInTiles = 0;
		startFloorPosition.Y--;
		lastIteratedFloorSpot = startFloorPosition;
		for (int i = 0; i < maxDistance; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (!WorldGen.SolidTile3(startFloorPosition.X, startFloorPosition.Y))
				{
					break;
				}
				startFloorPosition.Y--;
			}
			Collision.ExpandVertically(startFloorPosition.X, startFloorPosition.Y, out var topY, out var bottomY, height, 2);
			topY++;
			bottomY--;
			if (!WorldGen.SolidTile3(startFloorPosition.X, bottomY + 1))
			{
				Collision.ExpandVertically(startFloorPosition.X, bottomY, out var topY2, out var bottomY2, 0, 6);
				if (showDebug)
				{
					Dust.QuickBox(new Vector2((float)(startFloorPosition.X * 16 + 8), (float)(topY2 * 16)), new Vector2((float)(startFloorPosition.X * 16 + 8), (float)(bottomY2 * 16)), 1, Color.Blue, null);
				}
				if (!WorldGen.SolidTile3(startFloorPosition.X, bottomY2))
				{
					break;
				}
			}
			if (bottomY - topY < height - 1)
			{
				break;
			}
			if (showDebug)
			{
				Dust.QuickDust(startFloorPosition, Color.Green).scale = 1f;
				Dust.QuickBox(new Vector2((float)(startFloorPosition.X * 16 + 8), (float)(topY * 16)), new Vector2((float)(startFloorPosition.X * 16 + 8), (float)(bottomY * 16 + 16)), 1, Color.Red, null);
			}
			distanceCoveredInTiles += direction;
			startFloorPosition.X += direction;
			startFloorPosition.Y = bottomY;
			lastIteratedFloorSpot = startFloorPosition;
			if (Math.Abs(distanceCoveredInTiles) >= maxDistance)
			{
				break;
			}
		}
		distanceCoveredInTiles = Math.Abs(distanceCoveredInTiles);
	}
}
