using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace Terraria.ModLoader.Default;

public class UnloadedSolidTile : UnloadedTile
{
	public override string Texture => "ModLoader/UnloadedSolidTile";

	public override void SetStaticDefaults()
	{
		TileIO.Tiles.unloadedTypes.Add(base.Type);
		Main.tileFrameImportant[base.Type] = true;
		TileID.Sets.DisableSmartCursor[base.Type] = true;
		Main.tileSolid[base.Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.addTile(base.Type);
	}
}
