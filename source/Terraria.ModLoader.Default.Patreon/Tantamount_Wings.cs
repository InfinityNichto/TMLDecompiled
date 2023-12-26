using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Wings })]
internal class Tantamount_Wings : PatreonItem
{
	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		ArmorIDs.Wing.Sets.Stats[base.Item.wingSlot] = new WingStats(150, 7f);
	}

	public override void SetDefaults()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		base.SetDefaults();
		base.Item.vanity = false;
		base.Item.Size = new Vector2(24f, 26f);
		base.Item.accessory = true;
	}
}
