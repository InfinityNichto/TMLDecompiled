using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.GameContent;

public struct PlayerSittingHelper
{
	public const int ChairSittingMaxDistance = 40;

	public bool isSitting;

	public ExtraSeatInfo details;

	public Vector2 offsetForSeat;

	public int sittingIndex;

	public void GetSittingOffsetInfo(Player player, out Vector2 posOffset, out float seatAdjustment)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		if (isSitting)
		{
			posOffset = new Vector2((float)(sittingIndex * player.direction * 8), (float)sittingIndex * player.gravDir * -4f);
			seatAdjustment = -4f;
			seatAdjustment += (int)offsetForSeat.Y;
			posOffset += offsetForSeat * player.Directions;
		}
		else
		{
			posOffset = Vector2.Zero;
			seatAdjustment = 0f;
		}
	}

	public bool TryGetSittingBlock(Player player, out Tile tile)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		tile = default(Tile);
		if (!isSitting)
		{
			return false;
		}
		Point pt = (player.Bottom + new Vector2(0f, -2f)).ToTileCoordinates();
		if (!GetSittingTargetInfo(player, pt.X, pt.Y, out var _, out var _, out var _, out var _))
		{
			return false;
		}
		tile = Framing.GetTileSafely(pt);
		return true;
	}

	public void UpdateSitting(Player player)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		if (!isSitting)
		{
			return;
		}
		Point coords = (player.Bottom + new Vector2(0f, -2f)).ToTileCoordinates();
		if (!GetSittingTargetInfo(player, coords.X, coords.Y, out var targetDirection, out var _, out var seatDownOffset, out var extraInfo))
		{
			SitUp(player);
			return;
		}
		if (player.controlLeft || player.controlRight || player.controlUp || player.controlDown || player.controlJump || player.pulley || player.mount.Active || targetDirection != player.direction)
		{
			SitUp(player);
		}
		if (Main.sittingManager.GetNextPlayerStackIndexInCoords(coords) >= 2)
		{
			SitUp(player);
		}
		if (isSitting)
		{
			offsetForSeat = seatDownOffset;
			details = extraInfo;
			Main.sittingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, coords, out sittingIndex);
		}
	}

	public void SitUp(Player player, bool multiplayerBroadcast = true)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (isSitting)
		{
			isSitting = false;
			offsetForSeat = Vector2.Zero;
			sittingIndex = -1;
			details = default(ExtraSeatInfo);
			if (multiplayerBroadcast && Main.myPlayer == player.whoAmI)
			{
				NetMessage.SendData(13, -1, -1, null, player.whoAmI);
			}
		}
	}

	public void SitDown(Player player, int x, int y)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		if (!GetSittingTargetInfo(player, x, y, out var targetDirection, out var playerSittingPosition, out var seatDownOffset, out var extraInfo))
		{
			return;
		}
		Vector2 offset = playerSittingPosition - player.Bottom;
		bool flag = player.CanSnapToPosition(offset);
		if (flag)
		{
			flag &= Main.sittingManager.GetNextPlayerStackIndexInCoords((playerSittingPosition + new Vector2(0f, -2f)).ToTileCoordinates()) < 2;
		}
		if (!flag)
		{
			return;
		}
		if (isSitting && player.Bottom == playerSittingPosition)
		{
			SitUp(player);
			return;
		}
		player.StopVanityActions();
		player.RemoveAllGrapplingHooks();
		if (player.mount.Active)
		{
			player.mount.Dismount(player);
		}
		player.Bottom = playerSittingPosition;
		player.ChangeDir(targetDirection);
		isSitting = true;
		details = extraInfo;
		offsetForSeat = seatDownOffset;
		Main.sittingManager.AddPlayerAndGetItsStackedIndexInCoords(player.whoAmI, new Point(x, y), out sittingIndex);
		player.velocity = Vector2.Zero;
		player.gravDir = 1f;
		if (Main.myPlayer == player.whoAmI)
		{
			NetMessage.SendData(13, -1, -1, null, player.whoAmI);
		}
	}

	public static bool GetSittingTargetInfo(Player player, int x, int y, out int targetDirection, out Vector2 playerSittingPosition, out Vector2 seatDownOffset, out ExtraSeatInfo extraInfo)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		extraInfo = default(ExtraSeatInfo);
		Tile tileSafely = Framing.GetTileSafely(x, y);
		if (!TileID.Sets.CanBeSatOnForPlayers[tileSafely.type] || !tileSafely.active())
		{
			targetDirection = 1;
			seatDownOffset = Vector2.Zero;
			playerSittingPosition = default(Vector2);
			return false;
		}
		int num = x;
		int num2 = y;
		targetDirection = 1;
		seatDownOffset = Vector2.Zero;
		int num3 = 6;
		Vector2 zero = Vector2.Zero;
		switch (tileSafely.type)
		{
		case 15:
		case 497:
		{
			bool num7 = tileSafely.type == 15 && (tileSafely.frameY / 40 == 1 || tileSafely.frameY / 40 == 20);
			bool value = tileSafely.type == 15 && tileSafely.frameY / 40 == 27;
			seatDownOffset.Y = value.ToInt() * 4;
			if (tileSafely.frameY % 40 != 0)
			{
				num2--;
			}
			targetDirection = -1;
			if (tileSafely.frameX != 0)
			{
				targetDirection = 1;
			}
			if (num7 || tileSafely.type == 497)
			{
				extraInfo.IsAToilet = true;
			}
			break;
		}
		case 102:
		{
			int num5 = tileSafely.frameX / 18;
			if (num5 == 0)
			{
				num++;
			}
			if (num5 == 2)
			{
				num--;
			}
			int num6 = tileSafely.frameY / 18;
			if (num6 == 0)
			{
				num2 += 2;
			}
			if (num6 == 1)
			{
				num2++;
			}
			if (num6 == 3)
			{
				num2--;
			}
			targetDirection = player.direction;
			num3 = 0;
			break;
		}
		case 487:
		{
			int num4 = tileSafely.frameX % 72 / 18;
			if (num4 == 1)
			{
				num--;
			}
			if (num4 == 2)
			{
				num++;
			}
			if (tileSafely.frameY / 18 != 0)
			{
				num2--;
			}
			targetDirection = (num4 <= 1).ToDirectionInt();
			num3 = 0;
			seatDownOffset.Y -= 1f;
			break;
		}
		case 89:
		{
			targetDirection = player.direction;
			num3 = 0;
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector(-4f, 2f);
			Vector2 vector2 = default(Vector2);
			((Vector2)(ref vector2))._002Ector(4f, 2f);
			Vector2 vector3 = default(Vector2);
			((Vector2)(ref vector3))._002Ector(0f, 2f);
			Vector2 zero2 = Vector2.Zero;
			zero2.X = 1f;
			zero.X = -1f;
			switch (tileSafely.frameX / 54)
			{
			case 0:
				vector3.Y = (vector.Y = (vector2.Y = 1f));
				break;
			case 1:
				vector3.Y = 1f;
				break;
			case 2:
			case 14:
			case 15:
			case 17:
			case 20:
			case 21:
			case 22:
			case 23:
			case 25:
			case 26:
			case 27:
			case 28:
			case 35:
			case 37:
			case 38:
			case 39:
			case 40:
			case 41:
			case 42:
				vector3.Y = (vector.Y = (vector2.Y = 1f));
				break;
			case 3:
			case 4:
			case 5:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 16:
			case 18:
			case 19:
			case 36:
				vector3.Y = (vector.Y = (vector2.Y = 0f));
				break;
			case 6:
				vector3.Y = (vector.Y = (vector2.Y = -1f));
				break;
			case 24:
				vector3.Y = 0f;
				vector.Y = -4f;
				vector.X = 0f;
				vector2.X = 0f;
				vector2.Y = -4f;
				break;
			}
			if (tileSafely.frameY % 40 != 0)
			{
				num2--;
			}
			if ((tileSafely.frameX % 54 == 0 && targetDirection == -1) || (tileSafely.frameX % 54 == 36 && targetDirection == 1))
			{
				seatDownOffset = vector;
			}
			else if ((tileSafely.frameX % 54 == 0 && targetDirection == 1) || (tileSafely.frameX % 54 == 36 && targetDirection == -1))
			{
				seatDownOffset = vector2;
			}
			else
			{
				seatDownOffset = vector3;
			}
			seatDownOffset += zero2;
			break;
		}
		}
		TileRestingInfo info = new TileRestingInfo(player, new Point(num, num2), seatDownOffset, targetDirection, num3, zero, extraInfo);
		TileLoader.ModifySittingTargetInfo(x, y, tileSafely.type, ref info);
		num = info.AnchorTilePosition.X;
		num2 = info.AnchorTilePosition.Y;
		num3 = info.DirectionOffset;
		targetDirection = info.TargetDirection;
		seatDownOffset = info.VisualOffset;
		zero = info.FinalOffset;
		extraInfo = info.ExtraInfo;
		playerSittingPosition = Utils.ToWorldCoordinates(new Point(num, num2), 8f, 16f);
		playerSittingPosition.X += targetDirection * num3;
		playerSittingPosition += zero;
		return true;
	}
}
