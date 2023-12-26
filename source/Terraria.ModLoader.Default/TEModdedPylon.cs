using System.Linq;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria.ModLoader.Default;

/// <summary>
/// This is a TML provided Tile Entity that acts extremely similar to vanilla's pylon TEs by default. If you plan
/// to make a pylon tile in any capacity, you must extend this TE at least once.
/// </summary>
public abstract class TEModdedPylon : ModTileEntity, IPylonTileEntity
{
	public override void NetPlaceEntityAttempt(int x, int y)
	{
		if (!GetModPylonFromCoords(x, y, out var pylon))
		{
			RejectPlacementFromNet(x, y);
			return;
		}
		if (!(PylonLoader.PreCanPlacePylon(x, y, pylon.Type, pylon.PylonType) ?? pylon.CanPlacePylon()))
		{
			RejectPlacementFromNet(x, y);
			return;
		}
		int ID = Place(x, y);
		NetMessage.SendData(86, -1, -1, null, ID, x, y);
	}

	public bool TryGetModPylon(out ModPylon modPylon)
	{
		return GetModPylonFromCoords(Position.X, Position.Y, out modPylon);
	}

	private static void RejectPlacementFromNet(int x, int y)
	{
		WorldGen.KillTile(x, y);
		if (Main.netMode == 2)
		{
			NetMessage.SendData(17, -1, -1, null, 0, x, y);
		}
	}

	public new int Place(int i, int j)
	{
		int result = base.Place(i, j);
		Main.PylonSystem.RequestImmediateUpdate();
		return result;
	}

	public new void Kill(int x, int y)
	{
		base.Kill(x, y);
		Main.PylonSystem.RequestImmediateUpdate();
	}

	public override string ToString()
	{
		return Position.X + "x  " + Position.Y + "y";
	}

	public override bool IsTileValidForEntity(int x, int y)
	{
		TileObjectData tileData = TileObjectData.GetTileData(Main.tile[x, y]);
		if (Main.tile[x, y].active() && TileID.Sets.CountsAsPylon.Contains(Main.tile[x, y].type) && Main.tile[x, y].frameY == 0)
		{
			return Main.tile[x, y].frameX % tileData.CoordinateFullWidth == 0;
		}
		return false;
	}

	public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
	{
		TileObjectData tileData = TileObjectData.GetTileData(type, style, alternate);
		int topLeftX = i - tileData.Origin.X;
		int topLeftY = j - tileData.Origin.Y;
		if (Main.netMode == 1)
		{
			NetMessage.SendTileSquare(Main.myPlayer, topLeftX, topLeftY, tileData.Width, tileData.Height);
			NetMessage.SendData(87, -1, -1, null, topLeftX, topLeftY, base.Type);
			return -1;
		}
		return Place(topLeftX, topLeftY);
	}

	public int PlacementPreviewHook_CheckIfCanPlace(int x, int y, int type, int style = 0, int direction = 1, int alternate = 0)
	{
		ModPylon pylon = TileLoader.GetTile(type) as ModPylon;
		bool? flag = PylonLoader.PreCanPlacePylon(x, y, type, pylon.PylonType);
		if (flag.HasValue)
		{
			if (!flag.GetValueOrDefault())
			{
				return 1;
			}
			return 0;
		}
		if (!pylon.CanPlacePylon())
		{
			return 1;
		}
		return 0;
	}

	public static bool GetModPylonFromCoords(int x, int y, out ModPylon modPylon)
	{
		Tile tile = Main.tile[x, y];
		if (tile.active() && TileLoader.GetTile(tile.type) is ModPylon p)
		{
			modPylon = p;
			return true;
		}
		modPylon = null;
		return false;
	}
}
