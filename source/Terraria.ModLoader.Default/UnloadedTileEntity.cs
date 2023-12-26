using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Default;

public class UnloadedTileEntity : ModTileEntity
{
	private string modName;

	private string tileEntityName;

	private TagCompound data;

	internal void SetData(TagCompound tag)
	{
		modName = tag.GetString("mod");
		tileEntityName = tag.GetString("name");
		if (tag.ContainsKey("data"))
		{
			data = tag.GetCompound("data");
		}
	}

	public override bool IsTileValidForEntity(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		if (tile.HasTile)
		{
			return TileLoader.GetTile(tile.TileType) is UnloadedTile;
		}
		return false;
	}

	public override void SaveData(TagCompound tag)
	{
		tag["mod"] = modName;
		tag["name"] = tileEntityName;
		TagCompound tagCompound = data;
		if (tagCompound != null && tagCompound.Count > 0)
		{
			tag["data"] = data;
		}
	}

	public override void LoadData(TagCompound tag)
	{
		SetData(tag);
	}

	internal bool TryRestore(out ModTileEntity newEntity)
	{
		newEntity = null;
		if (ModContent.TryFind<ModTileEntity>(modName, tileEntityName, out var tileEntity))
		{
			newEntity = ModTileEntity.ConstructFromBase(tileEntity);
			newEntity.type = (byte)tileEntity.Type;
			newEntity.Position = Position;
			TagCompound tagCompound = data;
			if (tagCompound != null && tagCompound.Count > 0)
			{
				newEntity.LoadData(data);
			}
		}
		return newEntity != null;
	}
}
