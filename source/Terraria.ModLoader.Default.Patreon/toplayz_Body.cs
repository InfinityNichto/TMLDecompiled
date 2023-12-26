using Terraria.ID;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Body })]
internal class toplayz_Body : PatreonItem
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		ArmorIDs.Body.Sets.HidesTopSkin[base.Item.bodySlot] = true;
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 30;
		base.Item.height = 20;
	}
}
