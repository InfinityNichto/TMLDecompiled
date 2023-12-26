namespace Terraria.ModLoader;

public class GenericDamageClass : VanillaDamageClass
{
	protected override string LangKey => "LegacyTooltip.55";

	public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
	{
		return StatInheritanceData.None;
	}

	public override void SetDefaultStats(Player player)
	{
		player.GetCritChance(this) = 4f;
	}
}
