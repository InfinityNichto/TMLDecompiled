using System.Linq;
using System.Reflection;

namespace Terraria.ModLoader;

public class PreJITFilter
{
	public virtual bool ShouldJIT(MemberInfo member)
	{
		return member.GetCustomAttributes<MemberJitAttribute>().All((MemberJitAttribute a) => a.ShouldJIT(member));
	}
}
