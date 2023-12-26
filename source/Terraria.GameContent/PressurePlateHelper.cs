using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public class PressurePlateHelper
{
	public static object EntityCreationLock = new object();

	public static Dictionary<Point, bool[]> PressurePlatesPressed = new Dictionary<Point, bool[]>();

	public static bool NeedsFirstUpdate;

	private static Vector2[] PlayerLastPosition = (Vector2[])(object)new Vector2[255];

	private static Rectangle pressurePlateBounds = new Rectangle(0, 0, 16, 10);

	public static void Update()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (!NeedsFirstUpdate)
		{
			return;
		}
		foreach (Point key in PressurePlatesPressed.Keys)
		{
			PokeLocation(key);
		}
		PressurePlatesPressed.Clear();
		NeedsFirstUpdate = false;
	}

	public static void Reset()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		PressurePlatesPressed.Clear();
		for (int i = 0; i < PlayerLastPosition.Length; i++)
		{
			PlayerLastPosition[i] = Vector2.Zero;
		}
	}

	public static void ResetPlayer(int player)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Point[] array = PressurePlatesPressed.Keys.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			MoveAwayFrom(array[i], player);
		}
	}

	public static void UpdatePlayerPosition(Player player)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		Point p = default(Point);
		((Point)(ref p))._002Ector(1, 1);
		Vector2 vector = p.ToVector2();
		List<Point> tilesIn = Collision.GetTilesIn(PlayerLastPosition[player.whoAmI] + vector, PlayerLastPosition[player.whoAmI] + player.Size - vector * 2f);
		List<Point> tilesIn2 = Collision.GetTilesIn(player.TopLeft + vector, player.BottomRight - vector * 2f);
		Rectangle hitbox = player.Hitbox;
		Rectangle hitbox2 = player.Hitbox;
		((Rectangle)(ref hitbox)).Inflate(-p.X, -p.Y);
		((Rectangle)(ref hitbox2)).Inflate(-p.X, -p.Y);
		hitbox2.X = (int)PlayerLastPosition[player.whoAmI].X;
		hitbox2.Y = (int)PlayerLastPosition[player.whoAmI].Y;
		for (int i = 0; i < tilesIn.Count; i++)
		{
			Point point = tilesIn[i];
			Tile tile = Main.tile[point.X, point.Y];
			if (tile.active() && tile.type == 428)
			{
				pressurePlateBounds.X = point.X * 16;
				pressurePlateBounds.Y = point.Y * 16 + 16 - pressurePlateBounds.Height;
				if (!((Rectangle)(ref hitbox)).Intersects(pressurePlateBounds) && !tilesIn2.Contains(point))
				{
					MoveAwayFrom(point, player.whoAmI);
				}
			}
		}
		for (int j = 0; j < tilesIn2.Count; j++)
		{
			Point point2 = tilesIn2[j];
			Tile tile2 = Main.tile[point2.X, point2.Y];
			if (tile2.active() && tile2.type == 428)
			{
				pressurePlateBounds.X = point2.X * 16;
				pressurePlateBounds.Y = point2.Y * 16 + 16 - pressurePlateBounds.Height;
				if (((Rectangle)(ref hitbox)).Intersects(pressurePlateBounds) && (!tilesIn.Contains(point2) || !((Rectangle)(ref hitbox2)).Intersects(pressurePlateBounds)))
				{
					MoveInto(point2, player.whoAmI);
				}
			}
		}
		PlayerLastPosition[player.whoAmI] = player.position;
	}

	public static void DestroyPlate(Point location)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (PressurePlatesPressed.TryGetValue(location, out var _))
		{
			PressurePlatesPressed.Remove(location);
			PokeLocation(location);
		}
	}

	private static void UpdatePlatePosition(Point location, int player, bool onIt)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		if (onIt)
		{
			MoveInto(location, player);
		}
		else
		{
			MoveAwayFrom(location, player);
		}
	}

	private static void MoveInto(Point location, int player)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (PressurePlatesPressed.TryGetValue(location, out var value))
		{
			value[player] = true;
			return;
		}
		lock (EntityCreationLock)
		{
			PressurePlatesPressed[location] = new bool[255];
		}
		PressurePlatesPressed[location][player] = true;
		PokeLocation(location);
	}

	private static void MoveAwayFrom(Point location, int player)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		if (!PressurePlatesPressed.TryGetValue(location, out var value))
		{
			return;
		}
		value[player] = false;
		bool flag = false;
		for (int i = 0; i < value.Length; i++)
		{
			if (value[i])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			lock (EntityCreationLock)
			{
				PressurePlatesPressed.Remove(location);
			}
			PokeLocation(location);
		}
	}

	private static void PokeLocation(Point location)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode != 1)
		{
			Wiring.blockPlayerTeleportationForOneIteration = true;
			Wiring.HitSwitch(location.X, location.Y);
			NetMessage.SendData(59, -1, -1, null, location.X, location.Y);
		}
	}
}
