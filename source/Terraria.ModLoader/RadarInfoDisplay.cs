namespace Terraria.ModLoader;

public class RadarInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_5";

	protected override string LangKey => "LegacyInterface.100";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accThirdEye;
	}
}
