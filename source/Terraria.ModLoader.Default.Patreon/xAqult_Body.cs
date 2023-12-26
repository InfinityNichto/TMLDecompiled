using Terraria.ID;
using Terraria.Localization;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Body })]
internal class xAqult_Body : PatreonItem
{
	public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "");

	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		ArmorIDs.Body.Sets.HidesHands[base.Item.bodySlot] = false;
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 34;
		base.Item.height = 26;
	}
}
