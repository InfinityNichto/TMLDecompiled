using System;

namespace Terraria.DataStructures;

[Obsolete("Provides no utility over EntitySource_Parent, use that instead.")]
public class EntitySource_HitEffect : EntitySource_Parent
{
	public EntitySource_HitEffect(Entity entity, string context = null)
		: base(entity, context)
	{
	}
}
