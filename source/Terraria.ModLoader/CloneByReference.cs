using System;

namespace Terraria.ModLoader;

/// <summary>
/// Indicates that references to this object can be shared between clones.
/// When applied to a class, applies to all fields/properties of that type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class CloneByReference : Attribute
{
}
