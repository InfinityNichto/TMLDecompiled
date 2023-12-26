namespace Terraria.DataStructures;

/// <summary>
/// Used when opening bags or gifts. Passed to <see cref="M:Terraria.Player.QuickSpawnItem(Terraria.DataStructures.IEntitySource,Terraria.Item,System.Int32)" />
/// </summary>
public class EntitySource_ItemOpen : IEntitySource
{
	/// <summary>
	/// The player opening the item
	/// </summary>
	public Player Player { get; }

	/// <summary>
	/// The type of item being opened
	/// </summary>
	public int ItemType { get; }

	public string? Context { get; }

	public EntitySource_ItemOpen(Player player, int itemType, string? context = null)
	{
		Player = player;
		ItemType = itemType;
		Context = context;
	}
}
