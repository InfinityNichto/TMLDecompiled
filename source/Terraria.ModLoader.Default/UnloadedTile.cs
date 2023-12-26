using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Default;

public abstract class UnloadedTile : ModTile
{
	public override void MouseOver(int i, int j)
	{
		if (Main.netMode == 0)
		{
			Tile tile = Main.tile[i, j];
			if (!(tile == null) && tile.type == base.Type)
			{
				Player localPlayer = Main.LocalPlayer;
				ushort type = TileIO.Tiles.unloadedEntryLookup.Lookup(i, j);
				TileEntry info = TileIO.Tiles.entries[type];
				localPlayer.cursorItemIconEnabled = true;
				localPlayer.cursorItemIconID = -1;
				localPlayer.cursorItemIconText = info.modName + ": " + info.name;
			}
		}
	}
}
