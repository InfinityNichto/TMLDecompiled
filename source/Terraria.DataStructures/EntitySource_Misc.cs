using System;

namespace Terraria.DataStructures;

/// <summary>
/// To be used in cases where no entity is present. See <see cref="T:Terraria.ID.ItemSourceID" /> and <see cref="T:Terraria.ID.ProjectileSourceID" /> for vanilla values<para />
/// <b>NOTE:</b> Unlike most other entity sources, this one requires <see cref="P:Terraria.DataStructures.EntitySource_Misc.Context" /> to be specified.
/// </summary>
public class EntitySource_Misc : IEntitySource
{
	public string Context { get; }

	public EntitySource_Misc(string context)
	{
		Context = context ?? throw new ArgumentNullException("context", "The EntitySource_Misc type always expects a context string to be present.");
	}
}
