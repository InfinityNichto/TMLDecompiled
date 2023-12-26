namespace Terraria.DataStructures;

/// <summary>
/// Used when an NPC is spawned from <see cref="M:Terraria.NPC.SpawnNPC" /> as part of natural biome/event based spawning. <br /><br />
/// Note that some bosses incorrectly use this source to spawn minions, and remix world pots use it to spawn slimes. (to be fixed in 1.4.5)
/// </summary>
public class EntitySource_SpawnNPC : IEntitySource
{
	public string? Context { get; }

	public EntitySource_SpawnNPC(string? context = null)
	{
		Context = context;
	}
}
