using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace Terraria.ModLoader.Default;

public class UnloadedSemiSolidTile : UnloadedTile
{
	public override string Texture => "ModLoader/UnloadedSemiSolidTile";

	public override void SetStaticDefaults()
	{
		TileIO.Tiles.unloadedTypes.Add(base.Type);
		Main.tileFrameImportant[base.Type] = true;
		TileID.Sets.DisableSmartCursor[base.Type] = true;
		Main.tileNoAttach[base.Type] = true;
		Main.tileTable[base.Type] = true;
		Main.tileSolidTop[base.Type] = true;
		TileID.Sets.Platforms[base.Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.addTile(base.Type);
		base.AdjTiles = new int[1] { 19 };
	}
}
