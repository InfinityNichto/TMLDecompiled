namespace Terraria.ModLoader;

public class StopwatchInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_9";

	protected override string LangKey => "LegacyInterface.103";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accStopwatch;
	}
}
