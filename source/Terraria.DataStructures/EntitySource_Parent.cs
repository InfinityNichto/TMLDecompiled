namespace Terraria.DataStructures;

/// <summary>
/// Use when the parent can be considered the 'owner' or 'responsible for' the effect. <br />
/// This is the <b>most important</b> entity source. If in doubt, use it.
/// </summary>
public class EntitySource_Parent : IEntitySource
{
	/// <summary>
	/// The entity which is the source of the effect or action.
	/// In many cases, it makes sense to consider buffs or effects applied to the parent entity, and make changes to the spawned entity.
	/// </summary>
	public Entity Entity { get; }

	public string? Context { get; }

	public EntitySource_Parent(Entity entity, string? context = null)
	{
		Entity = entity;
		Context = context;
	}
}
