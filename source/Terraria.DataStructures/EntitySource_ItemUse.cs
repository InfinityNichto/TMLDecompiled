namespace Terraria.DataStructures;

/// <summary>
/// Use the interface, <see cref="T:Terraria.DataStructures.IEntitySource_WithStatsFromItem" /> instead when checking entity sources in <c>OnSpawn</c> <br /><br />
///
/// Used when a player uses an item or an accessory.
/// </summary>
public class EntitySource_ItemUse : EntitySource_Parent, IEntitySource_WithStatsFromItem
{
	public Player Player => (Player)base.Entity;

	public Item Item { get; }

	public EntitySource_ItemUse(Player player, Item item, string? context = null)
		: base(player, context)
	{
		Item = item;
	}
}
