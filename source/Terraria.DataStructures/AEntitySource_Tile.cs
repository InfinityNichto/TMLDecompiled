using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

/// <summary>
/// Abstract base class for entities which come from a tile. <br /><br />
///
/// If the entity comes from a Player/NPC/Projectile, and a tile is only incidentally involved, consider making your own subclass of <see cref="T:Terraria.DataStructures.EntitySource_Parent" /> instead
/// </summary>
public class AEntitySource_Tile : IEntitySource
{
	public Point TileCoords { get; }

	public string? Context { get; }

	public AEntitySource_Tile(int tileCoordsX, int tileCoordsY, string? context)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		TileCoords = new Point(tileCoordsX, tileCoordsY);
		Context = context;
	}
}
