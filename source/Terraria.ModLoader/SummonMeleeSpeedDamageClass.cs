namespace Terraria.ModLoader;

public class SummonMeleeSpeedDamageClass : VanillaDamageClass
{
	protected override string LangKey => "LegacyTooltip.53";

	public override bool UseStandardCritCalcs => false;

	public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
	{
		if (damageClass == DamageClass.Melee)
		{
			return new StatInheritanceData(0f, 0f, 1f);
		}
		if (damageClass == DamageClass.Generic || damageClass == DamageClass.Summon)
		{
			return StatInheritanceData.Full;
		}
		return StatInheritanceData.None;
	}

	public override bool GetEffectInheritance(DamageClass damageClass)
	{
		return damageClass == DamageClass.Summon;
	}

	public override bool ShowStatTooltipLine(Player player, string lineName)
	{
		return lineName != "CritChance";
	}
}
