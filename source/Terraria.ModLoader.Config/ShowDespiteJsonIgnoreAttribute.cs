using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// This attribute indicates that the field or property should be shown in the ModConfig UI despite having a <see cref="T:Newtonsoft.Json.JsonIgnoreAttribute" /> annotation.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ShowDespiteJsonIgnoreAttribute : Attribute
{
}
