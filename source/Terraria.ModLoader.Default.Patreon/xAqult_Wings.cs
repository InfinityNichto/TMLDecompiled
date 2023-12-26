using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Wings })]
internal class xAqult_Wings : PatreonItem
{
	public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "");

	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		ArmorIDs.Wing.Sets.Stats[base.Item.wingSlot] = new WingStats(150, 7f);
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.vanity = false;
		base.Item.width = 28;
		base.Item.height = 52;
		base.Item.accessory = true;
	}
}
