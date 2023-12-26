namespace Terraria.ModLoader;

public class WatchesInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_0";

	protected override string LangKey => "LegacyInterface.95";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accWatch > 0;
	}
}
