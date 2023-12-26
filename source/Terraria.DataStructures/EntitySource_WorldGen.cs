namespace Terraria.DataStructures;

/// <summary>
/// Used when spawning Town NPCs during world generation
/// </summary>
public class EntitySource_WorldGen : IEntitySource
{
	public string? Context { get; }

	public EntitySource_WorldGen(string? context = null)
	{
		Context = context;
	}
}
