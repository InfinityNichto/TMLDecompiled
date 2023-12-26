using System;

namespace Terraria.ModLoader.Config;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public abstract class ConfigKeyAttribute : Attribute
{
	internal readonly string key;

	internal readonly bool malformed;

	public ConfigKeyAttribute(string key)
	{
		if (!key.StartsWith("$"))
		{
			malformed = true;
			this.key = key;
		}
		else
		{
			this.key = key.Substring(1);
		}
	}
}
