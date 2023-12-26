namespace Terraria.ModLoader;

public class LifeformAnalyzerInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_11";

	protected override string LangKey => "LegacyInterface.105";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accCritterGuide;
	}
}
