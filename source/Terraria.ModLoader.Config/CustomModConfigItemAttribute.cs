using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Use this attribute to specify a custom UI element to be used for the annotated property, field, or class in the ModConfig UI.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field)]
public class CustomModConfigItemAttribute : Attribute
{
	public Type Type { get; }

	public CustomModConfigItemAttribute(Type type)
	{
		Type = type;
	}
}
