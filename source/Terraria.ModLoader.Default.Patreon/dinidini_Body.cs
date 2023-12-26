using Terraria.ID;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Body })]
internal class dinidini_Body : PatreonItem
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		ArmorIDs.Body.Sets.HidesTopSkin[base.Item.bodySlot] = true;
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 28;
		base.Item.height = 24;
	}
}
