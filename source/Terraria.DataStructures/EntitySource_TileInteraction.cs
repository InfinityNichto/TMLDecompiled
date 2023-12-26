namespace Terraria.DataStructures;

/// <summary>
/// Used when an entity interacts with a tile. <br />
/// Used by vanilla when players activate mechanisms like cannons and fireworks
/// </summary>
public class EntitySource_TileInteraction : AEntitySource_Tile
{
	/// <summary>
	/// The entity interacting with the tile. <br />
	/// Normally a <see cref="T:Terraria.Player" />
	/// </summary>
	public Entity Entity { get; }

	public EntitySource_TileInteraction(Entity entity, int tileCoordsX, int tileCoordsY, string? context = null)
		: base(tileCoordsX, tileCoordsY, context)
	{
		Entity = entity;
	}
}
