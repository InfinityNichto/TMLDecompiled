using Terraria.ID;
using Terraria.Localization;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class xAqult_Head : PatreonItem
{
	public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "");

	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		ArmorIDs.Head.Sets.DrawFullHair[base.Item.headSlot] = true;
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 22;
		base.Item.height = 10;
	}
}
