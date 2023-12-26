using Terraria.Localization;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Legs })]
internal class Linus_Legs : PatreonItem
{
	public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "");

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 22;
		base.Item.height = 18;
	}
}
