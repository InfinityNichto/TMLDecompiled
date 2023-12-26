namespace Terraria.DataStructures;

/// <summary>
/// Used along with <see cref="T:Terraria.DataStructures.EntitySource_Parent" /> to indicate that stats from the item should be transferred to spawned entities. <br />
/// When used to spawn projectiles, a snapshot of the Player and Item stats will be stored on the projectile. See <see cref="M:Terraria.Projectile.ApplyStatsFromSource(Terraria.DataStructures.IEntitySource)" /> for implementation
/// </summary>
public interface IEntitySource_WithStatsFromItem
{
	/// <summary>
	/// The Player using the Item. Equal to <see cref="P:Terraria.DataStructures.EntitySource_Parent.Entity" />
	/// </summary>
	Player Player { get; }

	/// <summary>
	/// The item being used
	/// </summary>
	Item Item { get; }
}
