namespace Terraria.DataStructures;

/// <summary>
/// Used for most vanilla bosses, conveys their initial target, which is normally the closest player.
/// </summary>
public class EntitySource_BossSpawn : IEntitySource
{
	/// <summary>
	/// The entity which this boss spawn on. <br /><br />
	/// In the vast majority of cases, this will be a <see cref="T:Terraria.Player" />. Often <see cref="F:Terraria.NPC.target" /> will be set to <c>Player.whoAmI</c>
	/// </summary>
	public Entity Target { get; }

	public string? Context { get; }

	public EntitySource_BossSpawn(Entity target, string? context = null)
	{
		Target = target;
		Context = context;
	}
}
