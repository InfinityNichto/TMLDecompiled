using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Head })]
internal class Glory_Head : PatreonItem
{
	public override void SetDefaults()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		base.SetDefaults();
		base.Item.Size = new Vector2(30f, 32f);
	}
}
