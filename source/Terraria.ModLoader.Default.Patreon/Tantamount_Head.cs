using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class Tantamount_Head : PatreonItem
{
	public override void OnCreated(ItemCreationContext context)
	{
		base.OnCreated(context);
		if (context is InitializationItemCreationContext)
		{
			EquipLoader.AddEquipTexture(base.Mod, Texture + "_Head", EquipType.Face, this);
		}
	}

	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		ArmorIDs.Head.Sets.DrawFullHair[base.Item.headSlot] = true;
	}

	public override void SetDefaults()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		base.SetDefaults();
		base.Item.accessory = true;
		base.Item.Size = new Vector2(26f);
	}
}
