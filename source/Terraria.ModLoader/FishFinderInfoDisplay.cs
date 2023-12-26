namespace Terraria.ModLoader;

public class FishFinderInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_2";

	protected override string LangKey => "LegacyInterface.97";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accFishFinder;
	}
}
