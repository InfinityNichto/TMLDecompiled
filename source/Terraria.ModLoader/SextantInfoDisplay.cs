namespace Terraria.ModLoader;

public class SextantInfoDisplay : VanillaInfoDisplay
{
	public override string Texture
	{
		get
		{
			int index = 7;
			if ((Main.bloodMoon && !Main.dayTime) || (Main.eclipse && Main.dayTime))
			{
				index = 8;
			}
			return "Terraria/Images/UI/InfoIcon_" + index;
		}
	}

	protected override string LangKey => "LegacyInterface.102";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accCalendar;
	}
}
