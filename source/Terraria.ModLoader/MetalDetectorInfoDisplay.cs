namespace Terraria.ModLoader;

public class MetalDetectorInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_10";

	protected override string LangKey => "LegacyInterface.104";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accOreFinder;
	}
}
