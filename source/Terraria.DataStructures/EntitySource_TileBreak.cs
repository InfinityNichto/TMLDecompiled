namespace Terraria.DataStructures;

/// <summary>
/// Used when a tile or wall is broken
/// </summary>
public class EntitySource_TileBreak : AEntitySource_Tile
{
	public EntitySource_TileBreak(int tileCoordsX, int tileCoordsY, string? context = null)
		: base(tileCoordsX, tileCoordsY, context)
	{
	}
}
