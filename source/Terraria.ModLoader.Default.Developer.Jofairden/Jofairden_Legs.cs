using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.Default.Developer.Jofairden;

[LegacyName(new string[] { "PowerRanger_Legs" })]
[AutoloadEquip(new EquipType[] { EquipType.Legs })]
internal class Jofairden_Legs : JofairdenArmorItem
{
	public override void SetDefaults()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		base.SetDefaults();
		base.Item.Size = new Vector2(22f, 18f);
	}
}
