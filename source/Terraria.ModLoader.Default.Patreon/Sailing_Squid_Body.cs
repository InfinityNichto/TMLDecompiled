namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Body })]
internal class Sailing_Squid_Body : PatreonItem
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 42;
		base.Item.height = 24;
	}
}
