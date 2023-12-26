using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Similar to JsonDefaultListValueAttribute, but for assigning to the Dictionary Key rather than the Value.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class JsonDefaultDictionaryKeyValueAttribute : Attribute
{
	public string Json { get; }

	public JsonDefaultDictionaryKeyValueAttribute(string json)
	{
		Json = json;
	}
}
