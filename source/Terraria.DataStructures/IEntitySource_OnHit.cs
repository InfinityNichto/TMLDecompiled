namespace Terraria.DataStructures;

/// <summary>
/// Used along with <see cref="T:Terraria.DataStructures.EntitySource_Parent" />. The <see cref="P:Terraria.DataStructures.IEntitySource_OnHit.Attacker" /> is also the <see cref="P:Terraria.DataStructures.EntitySource_Parent.Entity" /> (owner of the effect)
/// </summary>
public interface IEntitySource_OnHit
{
	/// <summary>
	/// The attacking entity. Note that this may be a <see cref="T:Terraria.Projectile" /> (possibly owned by a player), a <see cref="T:Terraria.Player" /> or even a <see cref="T:Terraria.NPC" />
	/// </summary>
	Entity Attacker { get; }

	/// <summary>
	/// The entity being attacked. Normally an NPC, but could be an Player if a mod decides to use this source in such a way
	/// </summary>
	Entity Victim { get; }
}
