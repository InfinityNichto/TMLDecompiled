using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Similar to DefaultListValueAttribute but for reference types. It uses a json string that will be used populate new instances list elements. Defines the default value, expressed as json, to be added when using the ModConfig UI to add elements to a Collection (List, Set, or Dictionary value).
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JsonDefaultListValueAttribute : Attribute
{
	public string Json { get; }

	public JsonDefaultListValueAttribute(string json)
	{
		Json = json;
	}
}
