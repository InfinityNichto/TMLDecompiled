using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public class TeleportHelpers
{
	public static bool RequestMagicConchTeleportPosition(Player player, int crawlOffsetX, int startX, out Point landingPoint)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		landingPoint = default(Point);
		Point point = default(Point);
		((Point)(ref point))._002Ector(startX, 50);
		int num = 1;
		int num2 = -1;
		int num3 = 1;
		int num4 = 0;
		int num5 = 5000;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)player.width * 0.5f, (float)player.height);
		int num6 = 40;
		bool flag = WorldGen.SolidOrSlopedTile(Main.tile[point.X, point.Y]);
		int num7 = 0;
		int num8 = 400;
		while (num4 < num5 && num7 < num8)
		{
			num4++;
			Tile tile = Main.tile[point.X, point.Y];
			Tile tile2 = Main.tile[point.X, point.Y + num3];
			bool flag2 = WorldGen.SolidOrSlopedTile(tile) || tile.liquid > 0;
			bool flag3 = WorldGen.SolidOrSlopedTile(tile2) || tile2.liquid > 0;
			if (IsInSolidTilesExtended(new Vector2((float)(point.X * 16 + 8), (float)(point.Y * 16 + 15)) - vector, player.velocity, player.width, player.height, (int)player.gravDir))
			{
				if (flag)
				{
					point.Y += num;
				}
				else
				{
					point.Y += num2;
				}
				continue;
			}
			if (flag2)
			{
				if (flag)
				{
					point.Y += num;
				}
				else
				{
					point.Y += num2;
				}
				continue;
			}
			flag = false;
			if (!IsInSolidTilesExtended(new Vector2((float)(point.X * 16 + 8), (float)(point.Y * 16 + 15 + 16)) - vector, player.velocity, player.width, player.height, (int)player.gravDir) && !flag3 && (double)point.Y < Main.worldSurface)
			{
				point.Y += num;
				continue;
			}
			if (tile2.liquid > 0)
			{
				point.X += crawlOffsetX;
				num7++;
				continue;
			}
			if (TileIsDangerous(point.X, point.Y))
			{
				point.X += crawlOffsetX;
				num7++;
				continue;
			}
			if (TileIsDangerous(point.X, point.Y + num3))
			{
				point.X += crawlOffsetX;
				num7++;
				continue;
			}
			if (point.Y >= num6)
			{
				break;
			}
			point.Y += num;
		}
		if (num4 == num5 || num7 >= num8)
		{
			return false;
		}
		if (!WorldGen.InWorld(point.X, point.Y, 40))
		{
			return false;
		}
		landingPoint = point;
		return true;
	}

	private static bool TileIsDangerous(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile.liquid > 0 && tile.lava())
		{
			return true;
		}
		if (tile.wall == 87 && (double)y > Main.worldSurface && !NPC.downedPlantBoss)
		{
			return true;
		}
		if (Main.wallDungeon[tile.wall] && (double)y > Main.worldSurface && !NPC.downedBoss3)
		{
			return true;
		}
		return false;
	}

	private static bool IsInSolidTilesExtended(Vector2 testPosition, Vector2 playerVelocity, int width, int height, int gravDir)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		if (Collision.LavaCollision(testPosition, width, height))
		{
			return true;
		}
		if (Collision.AnyHurtingTiles(testPosition, width, height))
		{
			return true;
		}
		if (Collision.SolidCollision(testPosition, width, height))
		{
			return true;
		}
		Vector2 vector = Vector2.UnitX * 16f;
		if (Collision.TileCollision(testPosition - vector, vector, width, height, fallThrough: false, fall2: false, gravDir) != vector)
		{
			return true;
		}
		vector = -Vector2.UnitX * 16f;
		if (Collision.TileCollision(testPosition - vector, vector, width, height, fallThrough: false, fall2: false, gravDir) != vector)
		{
			return true;
		}
		vector = Vector2.UnitY * 16f;
		if (Collision.TileCollision(testPosition - vector, vector, width, height, fallThrough: false, fall2: false, gravDir) != vector)
		{
			return true;
		}
		vector = -Vector2.UnitY * 16f;
		if (Collision.TileCollision(testPosition - vector, vector, width, height, fallThrough: false, fall2: false, gravDir) != vector)
		{
			return true;
		}
		return false;
	}
}
