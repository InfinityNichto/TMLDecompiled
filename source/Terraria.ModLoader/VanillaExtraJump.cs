using System.Collections.Generic;

namespace Terraria.ModLoader;

[Autoload(false)]
public abstract class VanillaExtraJump : ExtraJump
{
	public sealed override Position GetDefaultPosition()
	{
		return null;
	}

	public sealed override IEnumerable<Position> GetModdedConstraints()
	{
		return null;
	}
}
