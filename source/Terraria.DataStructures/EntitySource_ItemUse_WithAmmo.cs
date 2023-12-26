namespace Terraria.DataStructures;

public class EntitySource_ItemUse_WithAmmo : EntitySource_ItemUse
{
	/// <summary>
	/// A <see cref="T:Terraria.ID.ItemID" /> or <see cref="M:Terraria.ModLoader.ModContent.ItemType``1" />
	/// </summary>
	public int AmmoItemIdUsed { get; }

	public EntitySource_ItemUse_WithAmmo(Player player, Item item, int ammoItemId, string? context = null)
		: base(player, item, context)
	{
		AmmoItemIdUsed = ammoItemId;
	}
}
