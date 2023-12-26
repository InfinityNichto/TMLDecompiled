namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class Orian_Head : PatreonItem
{
	public override bool IsVanitySet(int head, int body, int legs)
	{
		if (head == EquipLoader.GetEquipSlot(base.Mod, "Orian_Head", EquipType.Head) && body == EquipLoader.GetEquipSlot(base.Mod, "Orian_Body", EquipType.Body))
		{
			return legs == EquipLoader.GetEquipSlot(base.Mod, "Orian_Legs", EquipType.Legs);
		}
		return false;
	}

	public override void UpdateVanitySet(Player player)
	{
		player.GetModPlayer<OrianSetEffectPlayer>().IsActive = true;
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 24;
		base.Item.height = 24;
	}
}
