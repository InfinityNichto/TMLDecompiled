using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.Default.Developer.Jofairden;

[LegacyName(new string[] { "PowerRanger_Head" })]
[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class Jofairden_Head : JofairdenArmorItem
{
	public override void SetDefaults()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		base.SetDefaults();
		base.Item.Size = new Vector2(18f, 20f);
	}

	public override bool IsVanitySet(int head, int body, int legs)
	{
		if (head == EquipLoader.GetEquipSlot(base.Mod, "Jofairden_Head", EquipType.Head) && body == EquipLoader.GetEquipSlot(base.Mod, "Jofairden_Body", EquipType.Body))
		{
			return legs == EquipLoader.GetEquipSlot(base.Mod, "Jofairden_Legs", EquipType.Legs);
		}
		return false;
	}

	public override void UpdateVanitySet(Player player)
	{
		player.GetModPlayer<JofairdenArmorEffectPlayer>().HasSetBonus = true;
	}
}
