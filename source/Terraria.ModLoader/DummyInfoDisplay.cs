namespace Terraria.ModLoader;

public class DummyInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_8";

	protected override string LangKey => "LegacyInterface.101";

	public override bool Active()
	{
		return false;
	}
}
