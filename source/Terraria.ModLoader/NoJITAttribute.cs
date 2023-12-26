using System.Reflection;

namespace Terraria.ModLoader;

public class NoJITAttribute : MemberJitAttribute
{
	public override bool ShouldJIT(MemberInfo member)
	{
		return false;
	}
}
