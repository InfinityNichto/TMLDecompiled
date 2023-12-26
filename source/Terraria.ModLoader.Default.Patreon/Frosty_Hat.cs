using Terraria.ID;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class Frosty_Hat : PatreonItem
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		ArmorIDs.Head.Sets.DrawHatHair[base.Item.headSlot] = true;
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 28;
		base.Item.height = 16;
	}
}
