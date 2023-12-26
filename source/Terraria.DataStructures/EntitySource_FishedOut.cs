namespace Terraria.DataStructures;

/// <summary>
/// Used when an NPC spawns as a result of fishing.
/// </summary>
public class EntitySource_FishedOut : IEntitySource
{
	/// <summary>
	/// The entity which was fishing. Normally a <see cref="T:Terraria.Player" />
	/// </summary>
	public Entity Fisher { get; }

	public string? Context { get; }

	public EntitySource_FishedOut(Entity fisher, string? context = null)
	{
		Fisher = fisher;
		Context = context;
	}
}
