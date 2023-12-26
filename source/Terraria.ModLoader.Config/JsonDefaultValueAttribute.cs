using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Similar to DefaultValueAttribute but for reference types. It uses a json string that will be used populate this element when initialized. Defines the default value, expressed as json, to be used to populate an object with the NullAllowed attribute. Modders should only use this in conjunction with NullAllowed, as simply initializing the field with a default value is preferred.
/// </summary>
public class JsonDefaultValueAttribute : Attribute
{
	public string Json { get; }

	public JsonDefaultValueAttribute(string json)
	{
		Json = json;
	}
}
