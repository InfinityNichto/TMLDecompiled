namespace Terraria.DataStructures;

/// <summary>
/// Used when items/bomb projectiles/npcs fall out of trees being cut down
/// </summary>
public class EntitySource_ShakeTree : AEntitySource_Tile
{
	public EntitySource_ShakeTree(int tileCoordsX, int tileCoordsY, string? context = null)
		: base(tileCoordsX, tileCoordsY, context)
	{
	}
}
