using System;

namespace Terraria.ModLoader.Config;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public abstract class ConfigArgsAttribute : Attribute
{
	internal readonly object[] args;

	public ConfigArgsAttribute(params object[] args)
	{
		this.args = args;
	}
}
