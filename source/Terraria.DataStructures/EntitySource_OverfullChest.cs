namespace Terraria.DataStructures;

/// <summary>
/// Used when attempting to add an item to a chest, but the chest is full so it spills into the world. <br />
/// Only vanilla use is the gas trap.
/// </summary>
public class EntitySource_OverfullChest : AEntitySource_Tile
{
	public Chest Chest { get; }

	public EntitySource_OverfullChest(int tileCoordsX, int tileCoordsY, Chest chest, string? context)
		: base(tileCoordsX, tileCoordsY, context)
	{
		Chest = chest;
	}
}
