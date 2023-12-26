using Terraria.DataStructures;

namespace Terraria.ModLoader.Default.Patreon;

internal class FrostyPlayer : ModPlayer
{
	public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (drawInfo.drawPlayer.body == EquipLoader.GetEquipSlot(base.Mod, "Frosty_Hoodie", EquipType.Body))
		{
			drawInfo.colorArmorBody = drawInfo.colorUnderShirt;
		}
	}
}
