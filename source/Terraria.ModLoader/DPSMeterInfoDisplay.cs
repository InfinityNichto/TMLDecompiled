namespace Terraria.ModLoader;

public class DPSMeterInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_12";

	protected override string LangKey => "LegacyInterface.106";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accDreamCatcher;
	}
}
