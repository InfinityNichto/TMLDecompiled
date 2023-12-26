using Microsoft.Xna.Framework;
using Terraria.GameContent.Achievements;
using Terraria.ID;

namespace Terraria.GameContent;

public class MinecartDiggerHelper
{
	public static MinecartDiggerHelper Instance = new MinecartDiggerHelper();

	public void TryDigging(Player player, Vector2 trackWorldPosition, int digDirectionX, int digDirectionY)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		digDirectionY = 0;
		Point point = trackWorldPosition.ToTileCoordinates();
		if (Framing.GetTileSafely(point).type != 314 || (double)point.Y < Main.worldSurface)
		{
			return;
		}
		Point point2 = point;
		point2.X += digDirectionX;
		point2.Y += digDirectionY;
		if (AlreadyLeadsIntoWantedTrack(point, point2) || (digDirectionY == 0 && (AlreadyLeadsIntoWantedTrack(point, new Point(point2.X, point2.Y - 1)) || AlreadyLeadsIntoWantedTrack(point, new Point(point2.X, point2.Y + 1)))))
		{
			return;
		}
		int num = 5;
		if (digDirectionY != 0)
		{
			num = 5;
		}
		Point point3 = point2;
		Point point4 = point3;
		point4.Y -= num - 1;
		int x = point4.X;
		for (int i = point4.Y; i <= point3.Y; i++)
		{
			if (!CanGetPastTile(x, i) || !HasPickPower(player, x, i))
			{
				return;
			}
		}
		if (CanConsumeATrackItem(player))
		{
			int x2 = point4.X;
			for (int j = point4.Y; j <= point3.Y; j++)
			{
				MineTheTileIfNecessary(x2, j);
			}
			ConsumeATrackItem(player);
			PlaceATrack(point2.X, point2.Y);
			player.velocity.X = MathHelper.Clamp(player.velocity.X, -1f, 1f);
			if (!DoTheTracksConnectProperly(point, point2))
			{
				CorrectTrackConnections(point, point2);
			}
		}
	}

	private bool CanConsumeATrackItem(Player player)
	{
		return FindMinecartTrackItem(player) != null;
	}

	private void ConsumeATrackItem(Player player)
	{
		Item item = FindMinecartTrackItem(player);
		item.stack--;
		if (item.stack == 0)
		{
			item.TurnToAir();
		}
	}

	private Item FindMinecartTrackItem(Player player)
	{
		Item result = null;
		for (int i = 0; i < 58; i++)
		{
			if (player.selectedItem != i || (player.itemAnimation <= 0 && player.reuseDelay <= 0 && player.itemTime <= 0))
			{
				Item item = player.inventory[i];
				if (item.type == 2340 && item.stack > 0)
				{
					result = item;
					break;
				}
			}
		}
		return result;
	}

	private void PoundTrack(Point spot)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (Main.tile[spot.X, spot.Y].type == 314 && Minecart.FrameTrack(spot.X, spot.Y, pound: true) && Main.netMode == 1)
		{
			NetMessage.SendData(17, -1, -1, null, 15, spot.X, spot.Y, 1f);
		}
	}

	private bool AlreadyLeadsIntoWantedTrack(Point tileCoordsOfFrontWheel, Point tileCoordsWeWantToReach)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Tile tileSafely = Framing.GetTileSafely(tileCoordsOfFrontWheel);
		Tile tileSafely2 = Framing.GetTileSafely(tileCoordsWeWantToReach);
		if (!tileSafely.active() || tileSafely.type != 314)
		{
			return false;
		}
		if (!tileSafely2.active() || tileSafely2.type != 314)
		{
			return false;
		}
		GetExpectedDirections(tileCoordsOfFrontWheel, tileCoordsWeWantToReach, out var expectedStartLeft, out var expectedStartRight, out var expectedEndLeft, out var expectedEndRight);
		if (!Minecart.GetAreExpectationsForSidesMet(tileCoordsOfFrontWheel, expectedStartLeft, expectedStartRight))
		{
			return false;
		}
		if (!Minecart.GetAreExpectationsForSidesMet(tileCoordsWeWantToReach, expectedEndLeft, expectedEndRight))
		{
			return false;
		}
		return true;
	}

	private static void GetExpectedDirections(Point startCoords, Point endCoords, out int? expectedStartLeft, out int? expectedStartRight, out int? expectedEndLeft, out int? expectedEndRight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		int num = endCoords.Y - startCoords.Y;
		int num2 = endCoords.X - startCoords.X;
		expectedStartLeft = null;
		expectedStartRight = null;
		expectedEndLeft = null;
		expectedEndRight = null;
		if (num2 == -1)
		{
			expectedStartLeft = num;
			expectedEndRight = -num;
		}
		if (num2 == 1)
		{
			expectedStartRight = num;
			expectedEndLeft = -num;
		}
	}

	private bool DoTheTracksConnectProperly(Point tileCoordsOfFrontWheel, Point tileCoordsWeWantToReach)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return AlreadyLeadsIntoWantedTrack(tileCoordsOfFrontWheel, tileCoordsWeWantToReach);
	}

	private void CorrectTrackConnections(Point startCoords, Point endCoords)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		GetExpectedDirections(startCoords, endCoords, out var expectedStartLeft, out var expectedStartRight, out var expectedEndLeft, out var expectedEndRight);
		Tile tileSafely = Framing.GetTileSafely(startCoords);
		Tile tileSafely2 = Framing.GetTileSafely(endCoords);
		if (tileSafely.active() && tileSafely.type == 314)
		{
			Minecart.TryFittingTileOrientation(startCoords, expectedStartLeft, expectedStartRight);
		}
		if (tileSafely2.active() && tileSafely2.type == 314)
		{
			Minecart.TryFittingTileOrientation(endCoords, expectedEndLeft, expectedEndRight);
		}
	}

	private bool HasPickPower(Player player, int x, int y)
	{
		if (player.HasEnoughPickPowerToHurtTile(x, y))
		{
			return true;
		}
		return false;
	}

	private bool CanGetPastTile(int x, int y)
	{
		if (WorldGen.CheckTileBreakability(x, y) != 0)
		{
			return false;
		}
		if (WorldGen.CheckTileBreakability2_ShouldTileSurvive(x, y))
		{
			return false;
		}
		Tile tile = Main.tile[x, y];
		if (tile.active() && (TileID.Sets.Falling[tile.type] || (tile.type == 26 && !Main.hardMode) || !WorldGen.CanKillTile(x, y)))
		{
			return false;
		}
		return true;
	}

	private void PlaceATrack(int x, int y)
	{
		int num = 314;
		int num2 = 0;
		if (WorldGen.PlaceTile(x, y, num, mute: false, forced: false, Main.myPlayer, num2))
		{
			NetMessage.SendData(17, -1, -1, null, 1, x, y, num, num2);
		}
	}

	private void MineTheTileIfNecessary(int x, int y)
	{
		AchievementsHelper.CurrentlyMining = true;
		if (Main.tile[x, y].active())
		{
			WorldGen.KillTile(x, y);
			NetMessage.SendData(17, -1, -1, null, 0, x, y);
		}
		AchievementsHelper.CurrentlyMining = false;
	}
}
