namespace Terraria.ModLoader;

public class MeleeNoSpeedDamageClass : VanillaDamageClass
{
	protected override string LangKey => "LegacyTooltip.2";

	public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
	{
		if (damageClass == DamageClass.Generic || damageClass == DamageClass.Melee)
		{
			StatInheritanceData full = StatInheritanceData.Full;
			full.attackSpeedInheritance = 0f;
			return full;
		}
		return StatInheritanceData.None;
	}

	public override bool GetEffectInheritance(DamageClass damageClass)
	{
		return damageClass == DamageClass.Melee;
	}
}
