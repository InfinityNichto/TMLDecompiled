namespace Terraria.ModLoader;

public class SummonDamageClass : VanillaDamageClass
{
	protected override string LangKey => "LegacyTooltip.53";

	public override bool UseStandardCritCalcs => false;

	public override bool ShowStatTooltipLine(Player player, string lineName)
	{
		if (lineName != "CritChance")
		{
			return lineName != "Speed";
		}
		return false;
	}
}
