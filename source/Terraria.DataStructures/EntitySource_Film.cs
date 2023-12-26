namespace Terraria.DataStructures;

public class EntitySource_Film : IEntitySource
{
	public string? Context { get; }

	public EntitySource_Film(string? context = null)
	{
		Context = context;
	}
}
