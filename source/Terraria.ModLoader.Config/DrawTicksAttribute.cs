using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Add this attribute and the sliders will show white tick marks at each increment.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DrawTicksAttribute : Attribute
{
}
