namespace Terraria.DataStructures;

public class EntitySource_OldOnesArmy : IEntitySource
{
	public string? Context { get; }

	public EntitySource_OldOnesArmy(string? context = null)
	{
		Context = context;
	}
}
