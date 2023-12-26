namespace Terraria.DataStructures;

/// <summary>
/// Use the interface, <see cref="T:Terraria.DataStructures.IEntitySource_OnHurt" /> instead when checking entity sources in <c>OnSpawn</c> <br /><br />
///
/// Recommend setting <see cref="P:Terraria.DataStructures.IEntitySource.Context" /> to indicate the effect.
/// </summary>
public class EntitySource_OnHurt : EntitySource_Parent, IEntitySource_OnHurt
{
	public Entity? Attacker { get; }

	public Entity Victim => base.Entity;

	public EntitySource_OnHurt(Entity victim, Entity? attacker, string? context = null)
		: base(victim, context)
	{
		Attacker = attacker;
	}
}
