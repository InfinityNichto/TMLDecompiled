namespace Terraria.DataStructures;

/// <summary>
/// Intended for mods to use when spawning projectiles periodically from buffs. <br /><br />
///
/// Note that this can be used for both NPCs and Players
/// </summary>
public class EntitySource_Buff : EntitySource_Parent
{
	/// <summary>
	/// The type of the buff (<see cref="T:Terraria.ID.BuffID" /> or <see cref="M:Terraria.ModLoader.ModContent.BuffType``1" />)
	/// </summary>
	public int BuffId { get; }

	/// <summary>
	/// The index of the buff in the entity's <c>buffType</c> and <c>buffTime</c> arrays.
	/// </summary>
	public int BuffIndex { get; }

	public EntitySource_Buff(Entity entity, int buffId, int buffIndex, string? context = null)
		: base(entity, context)
	{
		BuffId = buffId;
		BuffIndex = buffIndex;
	}
}
