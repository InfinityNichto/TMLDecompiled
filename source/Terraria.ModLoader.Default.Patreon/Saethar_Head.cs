using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class Saethar_Head : PatreonItem
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
		return head == equipSlots[0];
	}

	public override void UpdateVanitySet(Player player)
	{
		if (player.TryGetModPlayer<SaetharSetEffectPlayer>(out var modPlayer))
		{
			modPlayer.IsActive = true;
		}
	}

	public override void SetDefaults()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		base.SetDefaults();
		base.Item.Size = new Vector2(34f);
	}
}
