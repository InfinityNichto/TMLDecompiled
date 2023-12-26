namespace Terraria.DataStructures;

public class EntitySource_DebugCommand : IEntitySource
{
	public string Context { get; }

	public EntitySource_DebugCommand(string context)
	{
		Context = context;
	}
}
