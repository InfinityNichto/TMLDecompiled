namespace Terraria.ModLoader;

public class DepthMeterInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_4";

	protected override string LangKey => "LegacyInterface.99";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accDepthMeter > 0;
	}
}
