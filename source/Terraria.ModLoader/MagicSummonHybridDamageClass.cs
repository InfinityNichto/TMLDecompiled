namespace Terraria.ModLoader;

public class MagicSummonHybridDamageClass : VanillaDamageClass
{
	protected override string LangKey => "magic or summon damage";

	public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
	{
		if (damageClass == DamageClass.Generic || damageClass == DamageClass.Magic || damageClass == DamageClass.Summon)
		{
			return StatInheritanceData.Full;
		}
		return StatInheritanceData.None;
	}

	public override bool GetEffectInheritance(DamageClass damageClass)
	{
		if (damageClass != DamageClass.Magic)
		{
			return damageClass == DamageClass.Summon;
		}
		return true;
	}
}
