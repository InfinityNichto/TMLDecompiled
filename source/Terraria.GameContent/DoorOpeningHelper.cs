using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.GameContent;

public class DoorOpeningHelper
{
	public enum DoorAutoOpeningPreference
	{
		Disabled,
		EnabledForGamepadOnly,
		EnabledForEverything
	}

	private enum DoorCloseAttemptResult
	{
		StillInDoorArea,
		ClosedDoor,
		FailedToCloseDoor,
		DoorIsInvalidated
	}

	private struct DoorOpenCloseTogglingInfo
	{
		public Point tileCoordsForToggling;

		public DoorAutoHandler handler;
	}

	private struct PlayerInfoForOpeningDoors
	{
		public Rectangle hitboxToOpenDoor;

		public int intendedOpeningDirection;

		public int playerGravityDirection;

		public Rectangle tileCoordSpaceForCheckingForDoors;
	}

	private struct PlayerInfoForClosingDoors
	{
		public Rectangle hitboxToNotCloseDoor;
	}

	private interface DoorAutoHandler
	{
		DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords);

		bool TryOpenDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForOpeningDoors playerInfo);

		DoorCloseAttemptResult TryCloseDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForClosingDoors playerInfo);
	}

	private class CommonDoorOpeningInfoProvider : DoorAutoHandler
	{
		public DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
			Point tileCoordsForToggling = tileCoords;
			tileCoordsForToggling.Y -= tile.frameY % 54 / 18;
			DoorOpenCloseTogglingInfo result = default(DoorOpenCloseTogglingInfo);
			result.handler = this;
			result.tileCoordsForToggling = tileCoordsForToggling;
			return result;
		}

		public bool TryOpenDoor(DoorOpenCloseTogglingInfo doorInfo, PlayerInfoForOpeningDoors playerInfo)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			Point tileCoordsForToggling = doorInfo.tileCoordsForToggling;
			int intendedOpeningDirection = playerInfo.intendedOpeningDirection;
			Rectangle rectangle = default(Rectangle);
			((Rectangle)(ref rectangle))._002Ector(doorInfo.tileCoordsForToggling.X * 16, doorInfo.tileCoordsForToggling.Y * 16, 16, 48);
			switch (playerInfo.playerGravityDirection)
			{
			case 1:
				rectangle.Height += 16;
				break;
			case -1:
				rectangle.Y -= 16;
				rectangle.Height += 16;
				break;
			}
			if (!((Rectangle)(ref rectangle)).Intersects(playerInfo.hitboxToOpenDoor))
			{
				return false;
			}
			if (((Rectangle)(ref playerInfo.hitboxToOpenDoor)).Top < ((Rectangle)(ref rectangle)).Top || ((Rectangle)(ref playerInfo.hitboxToOpenDoor)).Bottom > ((Rectangle)(ref rectangle)).Bottom)
			{
				return false;
			}
			int originalClosedDoorType = 10;
			ModTile modTile = ModContent.GetModTile(Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y].type);
			if (modTile != null && TileID.Sets.OpenDoorID[modTile.Type] > -1)
			{
				originalClosedDoorType = modTile.Type;
			}
			WorldGen.OpenDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y, intendedOpeningDirection);
			if (Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y].type != originalClosedDoorType)
			{
				NetMessage.SendData(19, -1, -1, null, 0, tileCoordsForToggling.X, tileCoordsForToggling.Y, intendedOpeningDirection);
				return true;
			}
			WorldGen.OpenDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y, -intendedOpeningDirection);
			if (Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y].type != originalClosedDoorType)
			{
				NetMessage.SendData(19, -1, -1, null, 0, tileCoordsForToggling.X, tileCoordsForToggling.Y, -intendedOpeningDirection);
				return true;
			}
			return false;
		}

		public DoorCloseAttemptResult TryCloseDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForClosingDoors playerInfo)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			Point tileCoordsForToggling = info.tileCoordsForToggling;
			Tile tile = Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y];
			if (!tile.active() || TileLoader.IsClosedDoor(tile))
			{
				return DoorCloseAttemptResult.DoorIsInvalidated;
			}
			int num = tile.frameX % 72 / 18;
			Rectangle value = default(Rectangle);
			((Rectangle)(ref value))._002Ector(tileCoordsForToggling.X * 16, tileCoordsForToggling.Y * 16, 16, 48);
			switch (num)
			{
			case 1:
				value.X -= 16;
				break;
			case 2:
				value.X += 16;
				break;
			}
			((Rectangle)(ref value)).Inflate(1, 0);
			Rectangle rectangle = Rectangle.Intersect(value, playerInfo.hitboxToNotCloseDoor);
			if (rectangle.Width > 0 || rectangle.Height > 0)
			{
				return DoorCloseAttemptResult.StillInDoorArea;
			}
			if (WorldGen.CloseDoor(tileCoordsForToggling.X, tileCoordsForToggling.Y))
			{
				NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
				NetMessage.SendData(19, -1, -1, null, 1, tileCoordsForToggling.X, tileCoordsForToggling.Y, 1f);
				return DoorCloseAttemptResult.ClosedDoor;
			}
			return DoorCloseAttemptResult.FailedToCloseDoor;
		}
	}

	private class TallGateOpeningInfoProvider : DoorAutoHandler
	{
		public DoorOpenCloseTogglingInfo ProvideInfo(Point tileCoords)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
			Point tileCoordsForToggling = tileCoords;
			tileCoordsForToggling.Y -= tile.frameY % 90 / 18;
			DoorOpenCloseTogglingInfo result = default(DoorOpenCloseTogglingInfo);
			result.handler = this;
			result.tileCoordsForToggling = tileCoordsForToggling;
			return result;
		}

		public bool TryOpenDoor(DoorOpenCloseTogglingInfo doorInfo, PlayerInfoForOpeningDoors playerInfo)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			Point tileCoordsForToggling = doorInfo.tileCoordsForToggling;
			Rectangle rectangle = default(Rectangle);
			((Rectangle)(ref rectangle))._002Ector(doorInfo.tileCoordsForToggling.X * 16, doorInfo.tileCoordsForToggling.Y * 16, 16, 80);
			switch (playerInfo.playerGravityDirection)
			{
			case 1:
				rectangle.Height += 16;
				break;
			case -1:
				rectangle.Y -= 16;
				rectangle.Height += 16;
				break;
			}
			if (!((Rectangle)(ref rectangle)).Intersects(playerInfo.hitboxToOpenDoor))
			{
				return false;
			}
			if (((Rectangle)(ref playerInfo.hitboxToOpenDoor)).Top < ((Rectangle)(ref rectangle)).Top || ((Rectangle)(ref playerInfo.hitboxToOpenDoor)).Bottom > ((Rectangle)(ref rectangle)).Bottom)
			{
				return false;
			}
			bool flag = false;
			if (WorldGen.ShiftTallGate(tileCoordsForToggling.X, tileCoordsForToggling.Y, flag))
			{
				NetMessage.SendData(19, -1, -1, null, 4 + flag.ToInt(), tileCoordsForToggling.X, tileCoordsForToggling.Y);
				return true;
			}
			return false;
		}

		public DoorCloseAttemptResult TryCloseDoor(DoorOpenCloseTogglingInfo info, PlayerInfoForClosingDoors playerInfo)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			Point tileCoordsForToggling = info.tileCoordsForToggling;
			Tile tile = Main.tile[tileCoordsForToggling.X, tileCoordsForToggling.Y];
			if (!tile.active() || tile.type != 389)
			{
				return DoorCloseAttemptResult.DoorIsInvalidated;
			}
			_ = tile.frameY % 90 / 18;
			Rectangle value = default(Rectangle);
			((Rectangle)(ref value))._002Ector(tileCoordsForToggling.X * 16, tileCoordsForToggling.Y * 16, 16, 80);
			((Rectangle)(ref value)).Inflate(1, 0);
			Rectangle rectangle = Rectangle.Intersect(value, playerInfo.hitboxToNotCloseDoor);
			if (rectangle.Width > 0 || rectangle.Height > 0)
			{
				return DoorCloseAttemptResult.StillInDoorArea;
			}
			bool flag = true;
			if (WorldGen.ShiftTallGate(tileCoordsForToggling.X, tileCoordsForToggling.Y, flag))
			{
				NetMessage.SendData(13, -1, -1, null, Main.myPlayer);
				NetMessage.SendData(19, -1, -1, null, 4 + flag.ToInt(), tileCoordsForToggling.X, tileCoordsForToggling.Y);
				return DoorCloseAttemptResult.ClosedDoor;
			}
			return DoorCloseAttemptResult.FailedToCloseDoor;
		}
	}

	public static DoorAutoOpeningPreference PreferenceSettings = DoorAutoOpeningPreference.EnabledForEverything;

	private Dictionary<int, DoorAutoHandler> _handlerByTileType = new Dictionary<int, DoorAutoHandler>
	{
		{
			10,
			new CommonDoorOpeningInfoProvider()
		},
		{
			388,
			new TallGateOpeningInfoProvider()
		}
	};

	private List<DoorOpenCloseTogglingInfo> _ongoingOpenDoors = new List<DoorOpenCloseTogglingInfo>();

	private int _timeWeCanOpenDoorsUsingVelocityAlone;

	public void AllowOpeningDoorsByVelocityAloneForATime(int timeInFramesToAllow)
	{
		_timeWeCanOpenDoorsUsingVelocityAlone = timeInFramesToAllow;
	}

	public void Update(Player player)
	{
		LookForDoorsToClose(player);
		if (ShouldTryOpeningDoors())
		{
			LookForDoorsToOpen(player);
		}
		if (_timeWeCanOpenDoorsUsingVelocityAlone > 0)
		{
			_timeWeCanOpenDoorsUsingVelocityAlone--;
		}
	}

	private bool ShouldTryOpeningDoors()
	{
		return PreferenceSettings switch
		{
			DoorAutoOpeningPreference.EnabledForEverything => true, 
			DoorAutoOpeningPreference.EnabledForGamepadOnly => PlayerInput.UsingGamepad, 
			_ => false, 
		};
	}

	public static void CyclePreferences()
	{
		switch (PreferenceSettings)
		{
		case DoorAutoOpeningPreference.Disabled:
			PreferenceSettings = DoorAutoOpeningPreference.EnabledForEverything;
			break;
		case DoorAutoOpeningPreference.EnabledForEverything:
			PreferenceSettings = DoorAutoOpeningPreference.EnabledForGamepadOnly;
			break;
		case DoorAutoOpeningPreference.EnabledForGamepadOnly:
			PreferenceSettings = DoorAutoOpeningPreference.Disabled;
			break;
		}
	}

	public void LookForDoorsToClose(Player player)
	{
		PlayerInfoForClosingDoors playerInfoForClosingDoor = GetPlayerInfoForClosingDoor(player);
		for (int num = _ongoingOpenDoors.Count - 1; num >= 0; num--)
		{
			DoorOpenCloseTogglingInfo info = _ongoingOpenDoors[num];
			if (info.handler.TryCloseDoor(info, playerInfoForClosingDoor) != 0)
			{
				_ongoingOpenDoors.RemoveAt(num);
			}
		}
	}

	private PlayerInfoForClosingDoors GetPlayerInfoForClosingDoor(Player player)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		PlayerInfoForClosingDoors result = default(PlayerInfoForClosingDoors);
		result.hitboxToNotCloseDoor = player.Hitbox;
		return result;
	}

	public void LookForDoorsToOpen(Player player)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		PlayerInfoForOpeningDoors playerInfoForOpeningDoor = GetPlayerInfoForOpeningDoor(player);
		if (playerInfoForOpeningDoor.intendedOpeningDirection == 0 && player.velocity.X == 0f)
		{
			return;
		}
		Point tileCoords = default(Point);
		for (int i = ((Rectangle)(ref playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors)).Left; i <= ((Rectangle)(ref playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors)).Right; i++)
		{
			for (int j = ((Rectangle)(ref playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors)).Top; j <= ((Rectangle)(ref playerInfoForOpeningDoor.tileCoordSpaceForCheckingForDoors)).Bottom; j++)
			{
				tileCoords.X = i;
				tileCoords.Y = j;
				TryAutoOpeningDoor(tileCoords, playerInfoForOpeningDoor);
			}
		}
	}

	private PlayerInfoForOpeningDoors GetPlayerInfoForOpeningDoor(Player player)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		int num = player.controlRight.ToInt() - player.controlLeft.ToInt();
		int playerGravityDirection = (int)player.gravDir;
		Rectangle hitbox = player.Hitbox;
		hitbox.Y -= -1;
		hitbox.Height += -2;
		float num2 = player.velocity.X;
		if (num == 0 && _timeWeCanOpenDoorsUsingVelocityAlone == 0)
		{
			num2 = 0f;
		}
		float value = (float)num + num2;
		int num3 = Math.Sign(value) * (int)Math.Ceiling(Math.Abs(value));
		hitbox.X += num3;
		if (num == 0)
		{
			num = Math.Sign(value);
		}
		Rectangle hitbox2;
		Rectangle val = (hitbox2 = player.Hitbox);
		hitbox2.X += num3;
		Rectangle r = Rectangle.Union(val, hitbox2);
		Point point = r.TopLeft().ToTileCoordinates();
		Point point2 = r.BottomRight().ToTileCoordinates();
		Rectangle tileCoordSpaceForCheckingForDoors = default(Rectangle);
		((Rectangle)(ref tileCoordSpaceForCheckingForDoors))._002Ector(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
		PlayerInfoForOpeningDoors result = default(PlayerInfoForOpeningDoors);
		result.hitboxToOpenDoor = hitbox;
		result.intendedOpeningDirection = num;
		result.playerGravityDirection = playerGravityDirection;
		result.tileCoordSpaceForCheckingForDoors = tileCoordSpaceForCheckingForDoors;
		return result;
	}

	private void TryAutoOpeningDoor(Point tileCoords, PlayerInfoForOpeningDoors playerInfo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetHandler(tileCoords, out var infoProvider))
		{
			DoorOpenCloseTogglingInfo doorOpenCloseTogglingInfo = infoProvider.ProvideInfo(tileCoords);
			if (infoProvider.TryOpenDoor(doorOpenCloseTogglingInfo, playerInfo))
			{
				_ongoingOpenDoors.Add(doorOpenCloseTogglingInfo);
			}
		}
	}

	private bool TryGetHandler(Point tileCoords, out DoorAutoHandler infoProvider)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		infoProvider = null;
		if (!WorldGen.InWorld(tileCoords.X, tileCoords.Y, 3))
		{
			return false;
		}
		Tile tile = Main.tile[tileCoords.X, tileCoords.Y];
		if (tile == null)
		{
			return false;
		}
		int type = tile.type;
		ModTile modTile = ModContent.GetModTile(type);
		if (modTile != null && TileID.Sets.OpenDoorID[modTile.Type] > -1)
		{
			type = 10;
		}
		return _handlerByTileType.TryGetValue(type, out infoProvider);
	}
}
