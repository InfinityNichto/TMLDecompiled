using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace Terraria.ModLoader.Default;

public class UnloadedDresser : UnloadedTile
{
	public override string Texture => "ModLoader/UnloadedDresser";

	public override void SetStaticDefaults()
	{
		TileIO.Tiles.unloadedTypes.Add(base.Type);
		Main.tileFrameImportant[base.Type] = true;
		TileID.Sets.DisableSmartCursor[base.Type] = true;
		Main.tileNoAttach[base.Type] = true;
		Main.tileTable[base.Type] = true;
		Main.tileSolidTop[base.Type] = true;
		Main.tileContainer[base.Type] = true;
		TileID.Sets.BasicDresser[base.Type] = true;
		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
		TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
		TileObjectData.addTile(base.Type);
		base.AdjTiles = new int[1] { 88 };
	}
}
