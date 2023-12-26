using Terraria.Localization;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class xAqult_Mask : PatreonItem
{
	public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "");

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 30;
		base.Item.height = 34;
	}
}
