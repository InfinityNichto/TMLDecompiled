using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

[Autoload(false)]
public abstract class VanillaBuilderToggle : BuilderToggle
{
	public override string Texture => "Terraria/Images/UI/BuilderIcons";

	public override int NumberOfStates => 2;

	public override string DisplayValue()
	{
		return "";
	}

	public override Color DisplayColorTexture()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		if (base.CurrentState != 0)
		{
			return new Color(127, 127, 127);
		}
		return Color.White;
	}
}
