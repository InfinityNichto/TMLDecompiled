using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Affects whether this data will be expanded by default. The default value currently is true. Use the constructor with 2 parameters to control if list elements should be collapsed or expanded.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class ExpandAttribute : Attribute
{
	public bool Expand { get; }

	public bool? ExpandListElements { get; }

	public ExpandAttribute(bool expand = true)
	{
		Expand = expand;
		ExpandListElements = null;
	}

	public ExpandAttribute(bool expand = true, bool expandListElements = true)
	{
		Expand = expand;
		ExpandListElements = expandListElements;
	}
}
