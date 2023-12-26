using System;
using System.Reflection;

namespace Terraria.ModLoader;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = true)]
public abstract class MemberJitAttribute : Attribute
{
	public virtual bool ShouldJIT(MemberInfo member)
	{
		return true;
	}
}
