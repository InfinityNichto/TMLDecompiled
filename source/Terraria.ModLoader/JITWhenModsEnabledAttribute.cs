using System;
using System.Linq;
using System.Reflection;

namespace Terraria.ModLoader;

public sealed class JITWhenModsEnabledAttribute : MemberJitAttribute
{
	public readonly string[] Names;

	public JITWhenModsEnabledAttribute(params string[] names)
	{
		Names = names ?? throw new ArgumentNullException("names");
	}

	public override bool ShouldJIT(MemberInfo member)
	{
		return Names.All(ModLoader.HasMod);
	}
}
