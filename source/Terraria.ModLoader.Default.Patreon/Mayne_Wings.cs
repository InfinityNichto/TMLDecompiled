using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Wings })]
internal class Mayne_Wings : PatreonItem
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		ArmorIDs.Wing.Sets.Stats[base.Item.wingSlot] = new WingStats(150, 7f);
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.vanity = false;
		base.Item.width = 34;
		base.Item.height = 20;
		base.Item.accessory = true;
	}
}
