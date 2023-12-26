namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Back })]
internal class AlejandroAkbal_Back : PatreonItem
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 22;
		base.Item.height = 18;
		base.Item.accessory = true;
	}
}
