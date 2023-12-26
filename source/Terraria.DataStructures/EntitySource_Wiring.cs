namespace Terraria.DataStructures;

/// <summary>
/// Used when wiring activates an effect like a cannon or fireworks
/// </summary>
public class EntitySource_Wiring : AEntitySource_Tile
{
	public EntitySource_Wiring(int tileCoordsX, int tileCoordsY, string? context = null)
		: base(tileCoordsX, tileCoordsY, context)
	{
	}
}
