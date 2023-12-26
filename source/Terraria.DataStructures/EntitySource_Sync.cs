namespace Terraria.DataStructures;

/// <summary>
/// Used when an <see cref="T:Terraria.Item" /> is spawned on the server as a result of <see cref="F:Terraria.ID.MessageID.SyncItemCannotBeTakenByEnemies" /> <br />
/// Only used by vanilla for lucky coin. Note that the item is spawned client-side with <see cref="P:Terraria.DataStructures.IEntitySource.Context" /> = <see cref="F:Terraria.ID.ItemSourceID.LuckyCoin" />
/// </summary>
public class EntitySource_Sync : IEntitySource
{
	public string? Context { get; }

	public EntitySource_Sync(string? context = null)
	{
		Context = context;
	}
}
