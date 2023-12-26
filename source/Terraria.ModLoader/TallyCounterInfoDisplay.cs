namespace Terraria.ModLoader;

public class TallyCounterInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_6";

	protected override string LangKey => "LegacyInterface.101";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accJarOfSouls;
	}
}
