namespace Terraria.ModLoader;

public class DefaultDamageClass : VanillaDamageClass
{
	protected override string LangKey => "LegacyTooltip.55";

	public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
	{
		return StatInheritanceData.None;
	}
}
