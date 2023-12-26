namespace Terraria.DataStructures;

/// <summary>
/// Used when projectiles convert themselves to items in the world as a result of hitting a block.
/// </summary>
public class EntitySource_DropAsItem : EntitySource_Parent
{
	public EntitySource_DropAsItem(Entity entity, string? context = null)
		: base(entity, context)
	{
	}
}
