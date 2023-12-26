namespace Terraria.DataStructures;

/// <summary>
/// Used when an NPC or Projectile is killed and drops loot. <br />
/// Vanilla projectile examples are geodes and life crystal boulders
/// </summary>
public class EntitySource_Loot : EntitySource_Parent
{
	public EntitySource_Loot(Entity entity, string? context = null)
		: base(entity, context)
	{
	}
}
