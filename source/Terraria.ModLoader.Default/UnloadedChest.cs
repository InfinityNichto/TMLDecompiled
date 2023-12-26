using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace Terraria.ModLoader.Default;

public class UnloadedChest : UnloadedTile
{
	public override string Texture => "ModLoader/UnloadedChest";

	public override void SetStaticDefaults()
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		TileIO.Tiles.unloadedTypes.Add(base.Type);
		Main.tileFrameImportant[base.Type] = true;
		TileID.Sets.DisableSmartCursor[base.Type] = true;
		Main.tileSolid[base.Type] = false;
		Main.tileNoAttach[base.Type] = true;
		TileID.Sets.BasicChest[base.Type] = true;
		TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
		TileObjectData.addTile(base.Type);
		Main.tileSpelunker[base.Type] = true;
		Main.tileContainer[base.Type] = true;
		Main.tileShine2[base.Type] = true;
		Main.tileShine[base.Type] = 1200;
		Main.tileOreFinderPriority[base.Type] = 500;
		base.AdjTiles = new int[1] { 21 };
		AddMapEntry(new Color(200, 200, 200), this.GetLocalization("MapEntry0"), MapChestName);
		AddMapEntry(new Color(0, 141, 63), this.GetLocalization("MapEntry1"), MapChestName);
	}

	public override LocalizedText DefaultContainerName(int frameX, int frameY)
	{
		return Language.GetText(this.GetLocalizationKey("MapEntry0"));
	}

	public override ushort GetMapOption(int i, int j)
	{
		return 0;
	}

	public static string MapChestName(string name, int i, int j)
	{
		int left = i;
		int top = j;
		Tile tile = Main.tile[i, j];
		if (tile.frameX % 36 != 0)
		{
			left--;
		}
		if (tile.frameY != 0)
		{
			top--;
		}
		int chest = Chest.FindChest(left, top);
		if (chest < 0)
		{
			return Language.GetTextValue("LegacyChestType.0");
		}
		if (Main.chest[chest].name == "")
		{
			return name;
		}
		return name + ": " + Main.chest[chest].name;
	}
}
