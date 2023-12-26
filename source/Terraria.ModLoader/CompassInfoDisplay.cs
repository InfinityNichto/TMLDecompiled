namespace Terraria.ModLoader;

public class CompassInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_3";

	protected override string LangKey => "LegacyInterface.98";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accCompass > 0;
	}
}
