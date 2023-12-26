namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Body })]
internal class HER0zero_Body : PatreonItem
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 32;
		base.Item.height = 20;
	}

	public override bool IsVanitySet(int head, int body, int legs)
	{
		if (head == EquipLoader.GetEquipSlot(base.Mod, "HER0zero_Head", EquipType.Head) && body == EquipLoader.GetEquipSlot(base.Mod, "HER0zero_Body", EquipType.Body))
		{
			return legs == EquipLoader.GetEquipSlot(base.Mod, "HER0zero_Legs", EquipType.Legs);
		}
		return false;
	}

	public override void UpdateVanitySet(Player player)
	{
		player.GetModPlayer<HER0zeroPlayer>().glowEffect = true;
	}
}
