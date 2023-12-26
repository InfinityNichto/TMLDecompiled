namespace Terraria.DataStructures;

/// <summary>
/// Use the interface, <see cref="T:Terraria.DataStructures.IEntitySource_OnHit" /> instead when checking entity sources in <c>OnSpawn</c> <br /><br />
///
/// Recommend setting <see cref="P:Terraria.DataStructures.IEntitySource.Context" /> to indicate the effect. Many vanilla set bonuses or accessories use this source.
/// </summary>
public class EntitySource_OnHit : EntitySource_Parent, IEntitySource_OnHit
{
	public Entity Attacker => base.Entity;

	public Entity Victim { get; }

	public EntitySource_OnHit(Entity attacker, Entity victim, string? context = null)
		: base(attacker, context)
	{
		Victim = victim;
	}
}
