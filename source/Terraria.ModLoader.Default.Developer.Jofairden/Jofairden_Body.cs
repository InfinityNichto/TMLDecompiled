using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.Default.Developer.Jofairden;

[LegacyName(new string[] { "PowerRanger_Body" })]
[AutoloadEquip(new EquipType[] { EquipType.Body })]
internal class Jofairden_Body : JofairdenArmorItem
{
	public override void SetDefaults()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		base.SetDefaults();
		base.Item.Size = new Vector2(34f, 22f);
	}
}
