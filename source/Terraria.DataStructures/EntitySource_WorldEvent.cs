namespace Terraria.DataStructures;

/// <summary>
/// Inconsistently used by vanilla when spawning more exotic things like the lunatic pillars, cultist ritual, fairies etc. <br />
/// Don't expect this to distinguish between NPCs spawned due to an event/invasion. Most of those use <see cref="T:Terraria.DataStructures.EntitySource_SpawnNPC" />
/// </summary>
public class EntitySource_WorldEvent : IEntitySource
{
	public string? Context { get; }

	public EntitySource_WorldEvent(string? context = null)
	{
		Context = context;
	}
}
