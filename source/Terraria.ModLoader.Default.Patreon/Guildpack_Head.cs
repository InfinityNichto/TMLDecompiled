namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class Guildpack_Head : PatreonItem
{
	private static int[]? equipSlots;

	public override bool IsVanitySet(int head, int body, int legs)
	{
		if (equipSlots == null)
		{
			equipSlots = new int[3]
			{
				EquipLoader.GetEquipSlot(base.Mod, $"{base.InternalSetName}_{0}", EquipType.Head),
				EquipLoader.GetEquipSlot(base.Mod, $"{base.InternalSetName}_{1}", EquipType.Body),
				EquipLoader.GetEquipSlot(base.Mod, $"{base.InternalSetName}_{2}", EquipType.Legs)
			};
		}
		if (head == equipSlots[0] && body == equipSlots[1])
		{
			return legs == equipSlots[2];
		}
		return false;
	}

	public override void UpdateVanitySet(Player player)
	{
		if (player.TryGetModPlayer<GuildpackSetEffectPlayer>(out var modPlayer))
		{
			modPlayer.IsActive = true;
		}
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 34;
		base.Item.height = 22;
	}
}
