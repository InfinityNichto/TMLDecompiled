namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class dinidini_Head : PatreonItem
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 28;
		base.Item.height = 20;
	}
}
