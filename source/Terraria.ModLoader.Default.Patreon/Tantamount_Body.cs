using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Body })]
internal class Tantamount_Body : PatreonItem
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		ArmorIDs.Body.Sets.HidesArms[base.Item.bodySlot] = true;
		ArmorIDs.Body.Sets.HidesHands[base.Item.bodySlot] = false;
		ArmorIDs.Body.Sets.HidesTopSkin[base.Item.bodySlot] = false;
		ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[base.Item.bodySlot] = true;
	}

	public override void SetDefaults()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		base.SetDefaults();
		base.Item.Size = new Vector2(26f, 24f);
	}
}
