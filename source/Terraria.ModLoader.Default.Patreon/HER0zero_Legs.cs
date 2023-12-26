namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Legs })]
internal class HER0zero_Legs : PatreonItem
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 24;
		base.Item.height = 16;
	}
}
