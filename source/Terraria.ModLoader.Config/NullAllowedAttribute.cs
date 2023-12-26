using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// This attribute means the annotated item can possibly be null. This will allow the UI to make the item null. It is up to the modder to make sure the item isn't null in the ModConfig constructor and nested classes.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class NullAllowedAttribute : Attribute
{
}
