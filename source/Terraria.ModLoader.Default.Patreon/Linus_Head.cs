using Terraria.Localization;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class Linus_Head : PatreonItem
{
	public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "");

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 34;
		base.Item.height = 30;
	}
}
