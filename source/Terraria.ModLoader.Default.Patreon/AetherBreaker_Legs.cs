namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Legs })]
internal class AetherBreaker_Legs : PatreonItem
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 22;
		base.Item.height = 18;
	}
}
