namespace Terraria.DataStructures;

/// <summary>
/// Used when NPCs or pets/minions give gifts or rewards to a player.
/// </summary>
public class EntitySource_Gift : EntitySource_Parent
{
	public EntitySource_Gift(Entity entity, string? context = null)
		: base(entity, context)
	{
	}
}
