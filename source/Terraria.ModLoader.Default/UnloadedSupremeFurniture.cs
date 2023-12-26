using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace Terraria.ModLoader.Default;

public class UnloadedSupremeFurniture : UnloadedTile
{
	public override string Texture => "ModLoader/UnloadedSupremeFurniture";

	public override void SetStaticDefaults()
	{
		TileIO.Tiles.unloadedTypes.Add(base.Type);
		Main.tileFrameImportant[base.Type] = true;
		TileID.Sets.DisableSmartCursor[base.Type] = true;
		TileID.Sets.IgnoredByNpcStepUp[base.Type] = true;
		Main.tileSolidTop[base.Type] = true;
		Main.tileSolid[base.Type] = true;
		Main.tileTable[base.Type] = true;
		Main.tileNoAttach[base.Type] = true;
		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsChair);
		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
		AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
		TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
		TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.None, 0, 0);
		TileObjectData.addTile(base.Type);
	}
}
