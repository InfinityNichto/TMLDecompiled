namespace Terraria.ModLoader;

public class WeatherRadioInfoDisplay : VanillaInfoDisplay
{
	public override string Texture => "Terraria/Images/UI/InfoIcon_1";

	protected override string LangKey => "LegacyInterface.96";

	public override bool Active()
	{
		return Main.player[Main.myPlayer].accWeatherRadio;
	}
}
