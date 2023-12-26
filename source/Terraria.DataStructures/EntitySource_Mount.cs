namespace Terraria.DataStructures;

/// <summary>
/// Used for a effects from a mounted player
/// </summary>
public class EntitySource_Mount : EntitySource_Parent
{
	/// <summary>
	/// A <see cref="T:Terraria.ID.MountID" /> or <see cref="M:Terraria.ModLoader.ModContent.MountType``1" />
	/// </summary>
	public int MountId { get; }

	public EntitySource_Mount(Player player, int mountId, string? context = null)
		: base(player, context)
	{
		MountId = mountId;
	}
}
