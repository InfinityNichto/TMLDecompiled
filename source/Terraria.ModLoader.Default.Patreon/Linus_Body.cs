using Terraria.Localization;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Body })]
internal class Linus_Body : PatreonItem
{
	public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "");

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 34;
		base.Item.height = 24;
	}
}
